using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LPTControl
{
    public partial class Form1 : Form
    {
        [DllImport("NTport.dll")]
        public static extern void Outport(int address, int value);

        [DllImport("NTport.dll")]
        public static extern int Inport(int address);

        CheckBox[] bits;
        TextBox addressBox;
        int baseAddress;

        public Form1()
        {
            InitializeComponent();
            InitUI();
            LoadConfig();
        }

        void InitUI()
        {
            this.Text = "NTport LPT Control";
            this.Width = 500;
            this.Height = 200;

            Label lbl = new Label();
            lbl.Text = "Adresse (HEX):";
            lbl.Left = 20;
            lbl.Top = 10;
            this.Controls.Add(lbl);

            addressBox = new TextBox();
            addressBox.Left = 120;
            addressBox.Top = 10;
            addressBox.Width = 100;
            this.Controls.Add(addressBox);

            Button saveBtn = new Button();
            saveBtn.Text = "Save";
            saveBtn.Left = 240;
            saveBtn.Top = 8;
            saveBtn.Click += SaveBtn_Click;
            this.Controls.Add(saveBtn);

            bits = new CheckBox[8];

            for (int i = 0; i < 8; i++)
            {
                bits[i] = new CheckBox();
                bits[i].Text = "D" + i;
                bits[i].Left = 20 + i * 50;
                bits[i].Top = 50;
                this.Controls.Add(bits[i]);
            }

            Button writeBtn = new Button();
            writeBtn.Text = "Write";
            writeBtn.Top = 100;
            writeBtn.Left = 20;
            writeBtn.Click += WriteBtn_Click;
            this.Controls.Add(writeBtn);

            Button readBtn = new Button();
            readBtn.Text = "Read";
            readBtn.Top = 100;
            readBtn.Left = 120;
            readBtn.Click += ReadBtn_Click;
            this.Controls.Add(readBtn);
        }

        void LoadConfig()
        {
            try
            {
                string text = File.ReadAllText("config.txt").Trim();

                if (text.StartsWith("0x"))
                    text = text.Substring(2);

                baseAddress = Convert.ToInt32(text, 16);

                addressBox.Text = baseAddress.ToString("X");
            }
            catch
            {
                baseAddress = 0x378;
                addressBox.Text = "378";
            }
        }

        void SaveConfig()
        {
            File.WriteAllText("config.txt", addressBox.Text.Trim());
        }

        int BuildByte()
        {
            int value = 0;

            for (int i = 0; i < 8; i++)
                if (bits[i].Checked)
                    value |= (1 << i);

            return value;
        }

        void WriteBtn_Click(object sender, EventArgs e)
        {
            baseAddress = Convert.ToInt32(addressBox.Text, 16);
            Outport(baseAddress, BuildByte());
        }

        void ReadBtn_Click(object sender, EventArgs e)
        {
            baseAddress = Convert.ToInt32(addressBox.Text, 16);

            int value = Inport(baseAddress);

            for (int i = 0; i < 8; i++)
                bits[i].Checked = (value & (1 << i)) != 0;
        }

        void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveConfig();
            MessageBox.Show("Config sauvegardée");
        }
    }
}
