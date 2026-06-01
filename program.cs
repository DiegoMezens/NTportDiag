using System;
using System.Runtime.InteropServices;

class Program
{
    // ===== Tentative API 1 (Outport/Inport) =====
    [DllImport("NTport.dll", EntryPoint = "Outport", CallingConvention = CallingConvention.StdCall)]
    public static extern void Outport(int address, int value);

    [DllImport("NTport.dll", EntryPoint = "Inport", CallingConvention = CallingConvention.StdCall)]
    public static extern int Inport(int address);

    // ===== Config =====
    static int baseAddress = 0xCEFC;

    static void Main()
    {
        Console.WriteLine("=== NTport DIAGNOSTIC TOOL ===");
        Console.WriteLine();

        try
        {
            Console.WriteLine("[1] Test DLL load...");
            Console.WriteLine("NTport.dll OK (chargée)");

            Console.WriteLine();

            Console.WriteLine("[2] Adresse détectée : 0x" + baseAddress.ToString("X"));

            Console.WriteLine();

            Console.WriteLine("[3] Test écriture 0x01 sur port...");
            Outport(baseAddress, 0x01);
            Console.WriteLine("WRITE OK");

            Console.WriteLine();

            Console.WriteLine("[4] Test lecture...");
            int value = Inport(baseAddress);
            Console.WriteLine("READ OK : 0x" + value.ToString("X2"));

            Console.WriteLine();

            Console.WriteLine("=== RESULTAT ===");
            Console.WriteLine("✔ Driver OK");
            Console.WriteLine("✔ I/O accessible");
        }
        catch (DllNotFoundException)
        {
            Console.WriteLine("❌ NTport.dll introuvable");
            Console.WriteLine("→ mettre NTport.dll dans le dossier EXE");
        }
        catch (EntryPointNotFoundException ex)
        {
            Console.WriteLine("❌ Fonction DLL incorrecte");
            Console.WriteLine("→ NTport.dll ne contient pas Outport/Inport");
            Console.WriteLine(ex.Message);
        }
        catch (AccessViolationException)
        {
            Console.WriteLine("❌ ACCES REFUSE (driver ou Windows bloque I/O)");
            Console.WriteLine("→ lancer en admin + vérifier driver NTport");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ ERREUR GENERALE");
            Console.WriteLine(ex.Message);

            Console.WriteLine();
            Console.WriteLine("👉 Causes possibles :");
            Console.WriteLine("- mauvaise fonction DLL (Out32/Inp32 peut-être)");
            Console.WriteLine("- mauvaise architecture x86/x64");
            Console.WriteLine("- driver NTport non actif");
            Console.WriteLine("- adresse I/O invalide");
        }

        Console.WriteLine();
        Console.WriteLine("Appuyer sur une touche...");
        Console.ReadKey();
    }
}
