using System.Collections.Generic;

namespace motGlisse
{
    internal class Joueur
    {
        private string nom;
        private List<string> motsTrouves;
        private int score;

        public Joueur(string nom)
        {
            this.nom = nom;
            motsTrouves = new List<string>();
            score = 0;
        }

        public string Nom { get { return nom; } }

        public void Add_Mot(string mot)
        {
            // À compléter
        }

        public void Add_Score(int points)
        {
            // À compléter
        }

        public bool Contient(string mot)
        {
            // À compléter
            return false;
        }

        public string toString()
        {
            // retour string propre des infos joueur
            return "";
        }
    }
}
