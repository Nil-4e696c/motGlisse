using System;
using System.Collections.Generic;
using System.IO;

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

        // ------------------------------
        //        LECTURE FICHIER
        // ------------------------------
        public void ToRead(string nomFichier)
        {
            string[] lignes = File.ReadAllLines(nomFichier);

            foreach (string ligne in lignes)
            {
                if (string.IsNullOrWhiteSpace(ligne))
                    continue;

                string l = ligne.Trim();

                //
                // Ton fichier est au format :
                // 2
                // AA AH AI AN AS...
                // 3
                // AAS ACE ADA...
                //
                // Donc : si la ligne contient uniquement un nombre → on SKIP
                //
                bool estUnNombre = true;
                foreach (char c in l)
                {
                    if (!char.IsDigit(c))
                    {
                        estUnNombre = false;
                        break;
                    }
                }
                if (estUnNombre)
                    continue;

                // Ici : la ligne contient une liste de mots
                string[] mots = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (string mot in mots)
                {
                    string m = mot.Trim().ToUpper();

                    if (m.Length == 0)
                        continue;

                    char first = m[0];
                    if (first < 'A' || first > 'Z')
                        continue;

                    int index = first - 'A';
                    tabMotsParLettre[index].Add(m);
                }
            }
        }

        // ------------------------------
        //            TRI FUSION
        // ------------------------------
        public void Tri_XXX()
        {
            for (int i = 0; i < 26; i++)
            {
                var liste = tabMotsParLettre[i];
                if (liste.Count > 1)
                {
                    tabMotsParLettre[i] = MergeSort(liste);
                }
            }
        }

        private List<string> MergeSort(List<string> liste)
        {
            if (liste.Count <= 1)
                return liste;

            int mid = liste.Count / 2;

            var gauche = MergeSort(liste.GetRange(0, mid));
            var droite = MergeSort(liste.GetRange(mid, liste.Count - mid));

            return Fusion(gauche, droite);
        }

        private List<string> Fusion(List<string> a, List<string> b)
        {
            List<string> resultat = new List<string>();
            int i = 0, j = 0;

            while (i < a.Count && j < b.Count)
            {
                if (string.Compare(a[i], b[j], StringComparison.Ordinal) < 0)
                {
                    resultat.Add(a[i]);
                    i++;
                }
                else
                {
                    resultat.Add(b[j]);
                    j++;
                }
            }

            while (i < a.Count) resultat.Add(a[i++]);
            while (j < b.Count) resultat.Add(b[j++]);

            return resultat;
        }

        // ------------------------------
        //     RECHERCHE DICHOTOMIQUE
        // ------------------------------
        public bool RechDichoRecursif(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot))
                return false;

            mot = mot.Trim().ToUpper();
            int index = LettreToIndex(mot[0]);

            var liste = tabMotsParLettre[index];
            if (liste.Count == 0) return false;

            return RechDicho(mot, liste, 0, liste.Count - 1);
        }

        private bool RechDicho(string mot, List<string> liste, int gauche, int droite)
        {
            if (gauche > droite)
                return false;

            int mid = (gauche + droite) / 2;
            int cmp = string.Compare(mot, liste[mid], StringComparison.Ordinal);

            if (cmp == 0)
                return true;
            else if (cmp < 0)
                return RechDicho(mot, liste, gauche, mid - 1);
            else
                return RechDicho(mot, liste, mid + 1, droite);
        }

        private int LettreToIndex(char c)
        {
            return char.ToUpper(c) - 'A';
        }

        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < 26; i++)
            {
                char lettre = (char)('A' + i);
                s += $"{lettre} : {tabMotsParLettre[i].Count} mots\n";
            }

            return s;
        }
    }
}
