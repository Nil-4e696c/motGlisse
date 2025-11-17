using ConsoleApp1;
using System;

namespace motGlisse
{
    internal class Jeu
    {
        private Dictionnaire dico;
        private Plateau plateau;

        private Joueur joueur1;
        private Joueur joueur2;

        private int dureeTour = 30;
        private int dureePartie = 120;

        public Jeu()
        {
            dico = new Dictionnaire();
        }

        public void Demarrer()
        {
            InitialiserJoueurs();
            ChargerDictionnaire();
            MenuPrincipal();
        }

        private void InitialiserJoueurs()
        {
            // À compléter
        }

        private void MenuPrincipal()
        {
            // À compléter
        }

        private void ChargerDictionnaire()
        {
            // dico.ToRead();
            // dico.Tri_XXX();
        }

        private void ChargerPlateauDepuisFichier()
        {
            // plateau = new Plateau();
            // plateau.ToRead();
        }

        private void GenererPlateauAleatoire()
        {
            // plateau = new Plateau();
            // plateau.GenererAleatoire();
        }

        private void LancerPartie()
        {
            // boucle de jeu
        }

        private void LancerTour(Joueur joueur)
        {
            // recherche mot, validation, scoring, maj plateau
        }
    }
}
