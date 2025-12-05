using System;
using System.Collections.Generic;

namespace motGlisse
{
    public class Joueur
    {
        private string nom;
        private List<string> motsTrouves;
        private int score;

        public Joueur(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom du joueur ne peut pas être vide.");

            this.nom = nom.Trim();
            motsTrouves = new List<string>();
            score = 0;
        }

        // Propriété en lecture seule sur le nom
        public string Nom
        {
            get { return nom; }
        }

        // Propriété en lecture seule sur le score
        public int Score
        {
            get { return score; }
        }

        // Propriété pour lire la liste des mots trouvés (en lecture seule)
        public IReadOnlyList<string> MotsTrouves
        {
            get { return motsTrouves.AsReadOnly(); }
        }

        /// <summary>
        /// Ajoute un mot à la liste des mots trouvés si :
        /// - il n'est pas null/vides
        /// - il n'est pas déjà présent (test insensible à la casse)
        /// </summary>
        public void Add_Mot(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot))
                return;

            mot = mot.Trim();

            // On évite les doublons (maison == MAISON == Maison)
            if (!Contient(mot))
            {
                motsTrouves.Add(mot);
            }
        }

        /// <summary>
        /// Ajoute des points au score du joueur.
        /// </summary>
        public void Add_Score(int points)
        {
            // Si jamais on passe des points négatifs, on les applique quand même :
            // c'est le Jeu qui décidera si c'est autorisé ou pas.
            score += points;
        }

        /// <summary>
        /// Indique si le mot a déjà été trouvé par le joueur.
        /// Comparaison insensible à la casse.
        /// </summary>
        public bool Contient(string mot)
        {
            if (string.IsNullOrWhiteSpace(mot))
                return false;

            mot = mot.Trim();

            foreach (string m in motsTrouves)
            {
                if (string.Equals(m, mot, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Retourne une description lisible du joueur :
        /// Nom, score et liste des mots trouvés.
        /// </summary>
        public string toString()
        {
            string mots = motsTrouves.Count > 0
                ? string.Join(", ", motsTrouves)
                : "aucun mot trouvé";

            return $"Joueur : {nom} | Score : {score} | Mots trouvés : {mots}";
        }
    }
}
