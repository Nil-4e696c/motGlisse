using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace motGlisse
{
    public class Plateau
    {
        private const int TAILLE_LIGNES = 8;
        private const int TAILLE_COLONNES = 8;

        private char[,] grille;
        private int nbLignes;
        private int nbColonnes;

        // Poids des lettres (chargés depuis Lettres.txt)
        private static Dictionary<char, int> poidsLettres = new Dictionary<char, int>();

        // Random unique pour tout le programme (exigence du sujet)
        private static readonly Random rand = new Random();

        public Plateau()
        {
            nbLignes = TAILLE_LIGNES;
            nbColonnes = TAILLE_COLONNES;
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

        // Propriétés si besoin
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
            string[] first = lignes[0].Split(';');
            nbColonnes = first.Length;

            grille = new char[nbLignes, nbColonnes];

            for (int i = 0; i < nbLignes; i++)
            {
                string[] parts = lignes[i].Split(';');
                for (int j = 0; j < nbColonnes; j++)
                {
                    if (j < parts.Length && !string.IsNullOrWhiteSpace(parts[j]))
                        grille[i, j] = parts[j].Trim()[0];
                    else
                        grille[i, j] = ' ';
                }
            }
        }

        // -------------------------------------------------
        //           GENERATION ALEATOIRE DU PLATEAU
        // -------------------------------------------------
        public void GenererAleatoire(string fichierLettres)
        {
            if (string.IsNullOrWhiteSpace(fichierLettres))
                throw new ArgumentException("Chemin du fichier de lettres invalide.", nameof(fichierLettres));

            if (!File.Exists(fichierLettres))
                throw new FileNotFoundException($"Fichier de lettres introuvable: {fichierLettres}", fichierLettres);

            // Lecture des contraintes + poids
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
                int max = int.Parse(parts[1].Trim());
                int poids = int.Parse(parts[2].Trim());

                maxOccur[idx] = max;
                if (!poidsLettres.ContainsKey(lettre))
                    poidsLettres[lettre] = poids;
            }

            grille = new char[nbLignes, nbColonnes];

            // Remplissage en respectant les occurrences max
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
        public List<(int x, int y)>? Recherche_Mot(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot) || mot.Length < 2)
                return null;

            mot = mot.Trim().ToUpper();

            (int dx, int dy)[] directions = new (int, int)[]
            {
                (-1, 0), // haut
                (0, -1), // gauche
                (0, 1),  // droite
                (-1, -1),// diagonale haut-gauche
                (-1, 1)  // diagonale haut-droite
            };

            int ligneBase = nbLignes - 1;

            for (int col = 0; col < nbColonnes; col++)
            {
                if (grille[ligneBase, col] != mot[0])
                    continue;

                foreach (var dir in directions)
                {
                    List<(int x, int y)> chemin = new List<(int x, int y)>();

                    if (RechercheRec(mot, 0, ligneBase, col, dir.dx, dir.dy, chemin))
                        return chemin;
                }
            }

            return null;
        }

        private bool RechercheRec(string mot, int index, int x, int y, int dx, int dy, List<(int x, int y)> chemin)
        {
            if (x < 0 || x >= nbLignes || y < 0 || y >= nbColonnes)
                return false;

            if (grille[x, y] != mot[index])
                return false;

            chemin.Add((x, y));

            if (index == mot.Length - 1)
                return true;

            int nx = x + dx;
            int ny = y + dy;

            if (RechercheRec(mot, index + 1, nx, ny, dx, dy, chemin))
                return true;

            chemin.RemoveAt(chemin.Count - 1);
            return false;
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
                        c = '.'; // pour bien voir les cases vides

                    sb.Append(c);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
