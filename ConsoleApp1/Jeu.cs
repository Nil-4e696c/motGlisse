using System;
using motGlisse; // ✔ ton vrai namespace
// ❌ using ConsoleApp1; à supprimer

namespace motGlisse
{
    public class Jeu
    {
        private Dictionnaire dico;
        private Plateau? plateau;     // ✔ autorisé à être null au lancement

        private Joueur? joueur1;      // ✔ idem
        private Joueur? joueur2;

        private int dureeTour = 30;
        private int dureePartie = 120;

        public Jeu()
        {
            dico = new Dictionnaire();
            // plateau, joueur1, joueur2 seront initialisés PLUS TARD,
            // donc on les laisse null pour l'instant.
        }

        public void Demarrer()
        {
            InitialiserJoueurs();
            ChargerDictionnaire();
            MenuPrincipal();
        }

        private void InitialiserJoueurs()
        {
            // À compléter plus tard
        }

        private void MenuPrincipal()
        {
            // À compléter plus tard
        }

        private void ChargerDictionnaire()
        {
            // dico.ToRead("MotsFrancais.txt");
            // dico.Tri_XXX();
        }

        private void ChargerPlateauDepuisFichier()
        {
            // plateau = new Plateau();
            // plateau.ToRead("monFichier.csv");
        }

        private void GenererPlateauAleatoire()
        {
            // plateau = new Plateau();
            // plateau.GenererAleatoire();
        }

        private void LancerPartie()
        {
            // boucle principale du jeu
        }

        private void LancerTour(Joueur joueur)
        {
            // gestion d'un tour
        }
    }
}
