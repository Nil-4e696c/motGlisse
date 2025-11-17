using System;
using System.Collections.Generic;

namespace motGlisse
{
    public class Plateau
    {
        private char[,] grille;
        private int nbLignes;
        private int nbColonnes;

        public Plateau()
        {
            // Taille par défaut ou définie après lecture CSV
        }

        public void ToRead(string fichier)
        {
            // Import CSV -> remplir grille
        }

        public void ToFile(string fichier)
        {
            // Export CSV
        }

        public void GenererAleatoire(string fichierLettres)
        {
            // lire lettres.txt
            // générer plateau aléatoire
        }

        public bool EstVide()
        {
            // vérifier si toutes les cases sont vides
            return false;
        }

        public List<(int x, int y)> Recherche_Mot(string mot)
        {
            // Recherche horizontale, verticale, diagonale
            return null;
        }

        public int CalculerScore(List<(int x, int y)> positions)
        {
            // valeur des lettres (à lire depuis Lettres.txt)
            return 0;
        }

        public void Maj_Plateau(List<(int x, int y)> positions)
        {
            // vider les positions et faire glisser les lettres
        }

        public override string ToString()
        {
            // affichage du plateau
            return "";
        }
    }
}
