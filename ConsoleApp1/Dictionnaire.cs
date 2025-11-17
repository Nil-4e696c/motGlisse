using System;
using System.Collections.Generic;

namespace motGlisse
{
    public class Dictionnaire
    {
        private List<string>[] tabMotsParLettre;

        public Dictionnaire()
        {
            tabMotsParLettre = new List<string>[26];
            for (int i = 0; i < 26; i++)
                tabMotsParLettre[i] = new List<string>();
        }

        public void ToRead(string nomFichier)
        {
            // lire fichier et remplir tabMotsParLettre
        }

        public void Tri_XXX()
        {
            // quicksort / fusion
        }

        public bool RechDichoRecursif(string mot)
        {
            // recherche dichotomique
            return false;
        }

        private int LettreToIndex(char c)
        {
            // Retourne 0–25
            return char.ToUpper(c) - 'A';
        }
    }
}
