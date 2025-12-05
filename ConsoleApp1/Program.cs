using System;
using System.IO;

namespace motGlisse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Jeu jeu = new Jeu();
                jeu.Demarrer();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n=== ERREUR FATALE ===");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nAppuyez sur une touche pour fermer...");
            Console.ReadKey();
        }
    }
}

