using System;
using System.IO;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("NTport.dll")]
    public static extern void Outport(int address, int value);

    [DllImport("NTport.dll")]
    public static extern int Inport(int address);

    static int baseAddress;

    static void Main()
    {
        LoadConfig();

        Console.WriteLine("=== NTport CONSOLE TOOL ===");
        Console.WriteLine("Adresse : 0x" + baseAddress.ToString("X"));
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("[1] Write byte");
            Console.WriteLine("[2] Read byte");
            Console.WriteLine("[3] Set bits (0-7)");
            Console.WriteLine("[0] Exit");
            Console.Write("> ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    WriteByte();
                    break;

                case "2":
                    ReadByte();
                    break;

                case "3":
                    SetBits();
                    break;

                case "0":
                    return;
            }

            Console.WriteLine();
        }
    }

    static void LoadConfig()
    {
        string text = File.ReadAllText("config.txt").Trim();

        if (text.StartsWith("0x"))
            text = text.Substring(2);

        baseAddress = Convert.ToInt32(text, 16);
    }

    static void WriteByte()
    {
        Console.Write("Value (0-255): ");
        int v = Convert.ToInt32(Console.ReadLine());

        Outport(baseAddress, v);

        Console.WriteLine("OK write : 0x" + v.ToString("X2"));
    }

    static void ReadByte()
    {
        int v = Inport(baseAddress);

        Console.WriteLine("Read : 0x" + v.ToString("X2"));
    }

    static void SetBits()
    {
        int v = 0;

        for (int i = 0; i < 8; i++)
        {
            Console.Write($"Bit {i} (0/1): ");
            if (Console.ReadLine() == "1")
                v |= (1 << i);
        }

        Outport(baseAddress, v);

        Console.WriteLine("Written : 0x" + v.ToString("X2"));
    }
}
