using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace motGlisse
{
    public class Plateau
    {
        private char[,] grille;
        private int nbLignes;
        private int nbColonnes;

        // Poids des lettres (chargés depuis Lettres.txt)
        private static readonly Dictionary<char, int> poidsLettres = new Dictionary<char, int>();

        // Random unique pour tout le programme
        private static readonly Random rand = new Random();

        public Plateau()
        {
            nbLignes = 8;
            nbColonnes = 8;
            grille = new char[nbLignes, nbColonnes];
        }

        public Plateau(int lignes, int colonnes)
        {
            if (lignes <= 0 || colonnes <= 0)
                throw new ArgumentException("Les dimensions du plateau doivent être positives.");

            nbLignes = lignes;
            nbColonnes = colonnes;
            grille = new char[nbLignes, nbColonnes];
        }

        public int NbLignes => nbLignes;
        public int NbColonnes => nbColonnes;
        public char[,] Grille => grille;

        // -------------------------------------------------
        //                LECTURE / ECRITURE CSV
        // -------------------------------------------------
        public void ToRead(string fichier)
        {
            string[] lignes = File.ReadAllLines(fichier);

            nbLignes = lignes.Length;
            string[] first = lignes[0].Split(';', StringSplitOptions.None);
            nbColonnes = first.Length;

            grille = new char[nbLignes, nbColonnes];

            for (int i = 0; i < nbLignes; i++)
            {
                string[] parts = lignes[i].Split(';', StringSplitOptions.None);
                for (int j = 0; j < nbColonnes; j++)
                {
                    if (j < parts.Length && !string.IsNullOrWhiteSpace(parts[j]))
                        grille[i, j] = parts[j].Trim()[0];
                    else
                        grille[i, j] = ' ';
                }
            }
        }

        public void ToFile(string fichier)
        {
            string[] lignes = new string[nbLignes];

            for (int i = 0; i < nbLignes; i++)
            {
                string[] parts = new string[nbColonnes];
                for (int j = 0; j < nbColonnes; j++)
                {
                    char c = grille[i, j];
                    if (c == '\0') c = ' ';
                    parts[j] = c.ToString();
                }

                lignes[i] = string.Join(";", parts);
            }

            File.WriteAllLines(fichier, lignes);
        }

        // -------------------------------------------------
        //           GENERATION ALEATOIRE DU PLATEAU
        // -------------------------------------------------
        public void GenererAleatoire(string fichierLettres)
        {
            if (string.IsNullOrWhiteSpace(fichierLettres))
                throw new ArgumentException("Chemin du fichier de lettres invalide.", nameof(fichierLettres));

            if (!File.Exists(fichierLettres))
                throw new FileNotFoundException($"Fichier de lettres introuvable : {fichierLettres}", fichierLettres);

            int[] maxOccur = new int[26];
            int[] usedOccur = new int[26];

            poidsLettres.Clear();

            string[] lignes = File.ReadAllLines(fichierLettres);
            foreach (string ligne in lignes)
            {
                if (string.IsNullOrWhiteSpace(ligne)) continue;

                string[] parts = ligne.Split(',', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3) continue;

                char lettre = parts[0].Trim().ToUpper()[0];
                int idx = lettre - 'A';

                if (!int.TryParse(parts[1].Trim(), out int max))
                    continue;
                if (!int.TryParse(parts[2].Trim(), out int poids))
                    continue;

                maxOccur[idx] = max;
                poidsLettres[lettre] = poids;
            }

            grille = new char[nbLignes, nbColonnes];

            for (int i = 0; i < nbLignes; i++)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    List<int> dispo = new List<int>();
                    for (int k = 0; k < 26; k++)
                    {
                        if (maxOccur[k] == 0) continue;
                        if (usedOccur[k] < maxOccur[k])
                            dispo.Add(k);
                    }

                    if (dispo.Count == 0)
                    {
                        grille[i, j] = ' ';
                        continue;
                    }

                    int choiceIndex = dispo[rand.Next(dispo.Count)];
                    char c = (char)('A' + choiceIndex);
                    grille[i, j] = c;
                    usedOccur[choiceIndex]++;
                }
            }
        }

        // -------------------------------------------------
        //                  ETAT DU PLATEAU
        // -------------------------------------------------
        public bool EstVide()
        {
            for (int i = 0; i < nbLignes; i++)
                for (int j = 0; j < nbColonnes; j++)
                    if (grille[i, j] != ' ' && grille[i, j] != '\0')
                        return false;

            return true;
        }

        // -------------------------------------------------
        //                RECHERCHE D'UN MOT
        // -------------------------------------------------
        // Directions autorisées : →, ←, ↓, ↘, ↙
        public List<(int x, int y)>? Recherche_Mot(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot) || mot.Length < 2)
                return null;

            mot = mot.Trim().ToUpper();

            (int dx, int dy)[] directions =
            {
                (0, 1),   // droite
                (0, -1),  // gauche
                (1, 0),   // bas
                (1, 1),   // diagonale bas-droite
                (1, -1)   // diagonale bas-gauche
            };

            for (int x = 0; x < nbLignes; x++)
            {
                for (int y = 0; y < nbColonnes; y++)
                {
                    if (grille[x, y] != mot[0])
                        continue;

                    foreach (var (dx, dy) in directions)
                    {
                        List<(int x, int y)> chemin = new List<(int x, int y)>();
                        if (RechercheRec(mot, 0, x, y, dx, dy, chemin))
                            return chemin;
                    }
                }
            }

            return null;
        }

        private bool RechercheRec(string mot, int index, int x, int y,
                                  int dx, int dy, List<(int x, int y)> chemin)
        {
            if (x < 0 || x >= nbLignes || y < 0 || y >= nbColonnes)
                return false;

            if (grille[x, y] != mot[index])
                return false;

            chemin.Add((x, y));

            if (index == mot.Length - 1)
                return true;

            return RechercheRec(mot, index + 1, x + dx, y + dy, dx, dy, chemin);
        }

        // -------------------------------------------------
        //                  CALCUL DU SCORE
        // -------------------------------------------------
        public int CalculerScore(List<(int x, int y)> positions)
        {
            if (positions == null || positions.Count == 0)
                return 0;

            int score = 0;

            foreach (var (x, y) in positions)
            {
                char c = grille[x, y];
                if (poidsLettres.TryGetValue(c, out int p))
                    score += p;
                else
                    score += 1; // fallback minimal
            }

            // On privilégie les mots longs
            score *= positions.Count;
            return score;
        }

        // -------------------------------------------------
        //                 MISE A JOUR PLATEAU
        // -------------------------------------------------
        public void Maj_Plateau(List<(int x, int y)> positions)
        {
            if (positions == null || positions.Count == 0)
                return;

            bool[,] suppr = new bool[nbLignes, nbColonnes];

            foreach (var (x, y) in positions)
            {
                if (x >= 0 && x < nbLignes && y >= 0 && y < nbColonnes)
                    suppr[x, y] = true;
            }

            for (int col = 0; col < nbColonnes; col++)
            {
                List<char> lettres = new List<char>();

                // on remonte la colonne
                for (int lig = nbLignes - 1; lig >= 0; lig--)
                {
                    if (!suppr[lig, col] && grille[lig, col] != ' ' && grille[lig, col] != '\0')
                        lettres.Add(grille[lig, col]);
                }

                int index = 0;
                for (int lig = nbLignes - 1; lig >= 0; lig--)
                {
                    if (index < lettres.Count)
                    {
                        grille[lig, col] = lettres[index];
                        index++;
                    }
                    else
                    {
                        grille[lig, col] = ' ';
                    }
                }
            }
        }

        // -------------------------------------------------
        //                  AFFICHAGE
        // -------------------------------------------------
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nbLignes; i++)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    char c = grille[i, j];
                    if (c == '\0' || c == ' ')
                        c = '.';

                    sb.Append(c);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
