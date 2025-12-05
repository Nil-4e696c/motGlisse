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
            string[] lignes = File.ReadAllLines(nomFichier);
            for (int i = 0; i < lignes.Length && i < 26; i++)
            {
                string ligne = lignes[i];
                string[] mots = ligne.Split(' ');
                foreach(string mot in mots)
                {
                    if (mot != "")
                    {
                        tabMotsParLettre[i].Add(mot);
                    }
                }
            }
            // lire fichier et remplir tabMotsParLettre
        }

        public void Tri_XXX()
        {
            for(int i = 0; i < 26; i++)
            {
                tabMotsParLettre[i].Sort();
                // trier tabMotsParLettre[i] avec XXX
            }
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
