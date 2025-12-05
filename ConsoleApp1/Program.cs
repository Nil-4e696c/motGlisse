using System;
using motGlisse;

namespace motGlisse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var j = new Joueur("Maxence");
            j.Add_Mot("maison");
            j.Add_Mot("MAISON");
            j.Add_Score(10);

            Console.WriteLine(j.toString());
        }
    }
}
