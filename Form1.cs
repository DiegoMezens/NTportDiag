using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NTportDiag
{
    public class Form1 : Form
    {
        [DllImport("NTport.dll")]
        public static extern void Outport(int address, int value);

        [DllImport("NTport.dll")]
        public static extern int Inport(int address);

        CheckBox[] bits = new CheckBox[8];
        TextBox addrBox;
        int baseAddress;

        public Form1()
        {
            Text = "NTport Control";
            Width = 450;
            Height = 200;

            InitUI();
            LoadConfig();
        }

        void InitUI()
        {
            addrBox = new TextBox();
            addrBox.Left = 20;
            addrBox.Top = 20;
            addrBox.Width = 100;
            Controls.Add(addrBox);

            Button save = new Button();
            save.Text = "Save";
            save.Left = 140;
            save.Top = 18;
            save.Click += (s, e) => File.WriteAllText("config.txt", addrBox.Text);
            Controls.Add(save);

            for (int i = 0; i < 8; i++)
            {
                bits[i] = new CheckBox();
                bits[i].Text = "D" + i;
                bits[i].Left = 20 + i * 45;
                bits[i].Top = 60;
                Controls.Add(bits[i]);
            }

            Button write = new Button();
            write.Text = "Write";
            write.Left = 20;
            write.Top = 100;
            write.Click += (s, e) => Outport(baseAddress, Build());
            Controls.Add(write);

            Button read = new Button();
            read.Text = "Read";
            read.Left = 120;
            read.Top = 100;
            read.Click += (s, e) =>
            {
                int v = Inport(baseAddress);
                for (int i = 0; i < 8; i++)
                    bits[i].Checked = (v & (1 << i)) != 0;
            };
            Controls.Add(read);
        }

        void LoadConfig()
        {
            try
            {
                string t = File.ReadAllText("config.txt").Trim();
                if (t.StartsWith("0x")) t = t.Substring(2);
                baseAddress = Convert.ToInt32(t, 16);
                addrBox.Text = baseAddress.ToString("X");
            }
            catch
            {
                baseAddress = 0x378;
                addrBox.Text = "378";
            }
        }

        int Build()
        {
            int v = 0;
            for (int i = 0; i < 8; i++)
                if (bits[i].Checked)
                    v |= (1 << i);
            return v;
        }
    }
}
