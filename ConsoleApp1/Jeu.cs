using System;
using System.IO;

namespace motGlisse
{
    public class Jeu
    {
        private Dictionnaire dico;
        private Plateau? plateau;
        private Joueur? joueur1;
        private Joueur? joueur2;

        private TimeSpan dureeTour;
        private TimeSpan dureePartie;

        public Jeu()
        {
            dico = new Dictionnaire();
            dureeTour = TimeSpan.FromSeconds(30);    // 30 sec par tour
            dureePartie = TimeSpan.FromMinutes(2);   // 2 minutes par partie
        }

        // --------------------------------------------------------------
        //                      DEMARRAGE DU JEU
        // --------------------------------------------------------------
        public void Demarrer()
        {
            Console.WriteLine("=== JEU DES MOTS GLISSES ===");

            InitialiserJoueurs();
            ChargerDictionnaire();
            MenuPrincipal();
        }

        // --------------------------------------------------------------
        //                   INITIALISATION DES JOUEURS
        // --------------------------------------------------------------
        private void InitialiserJoueurs()
        {
            string nom1 = "";
            while (string.IsNullOrWhiteSpace(nom1))
            {
                Console.Write("Nom du joueur 1 : ");
                nom1 = Console.ReadLine()!.Trim();
            }

            joueur1 = new Joueur(nom1);

            string nom2 = "";
            while (string.IsNullOrWhiteSpace(nom2))
            {
                Console.Write("Nom du joueur 2 : ");
                nom2 = Console.ReadLine()!.Trim();
            }

            joueur2 = new Joueur(nom2);

            Console.WriteLine($"Joueurs enregistrés : {joueur1.Nom} VS {joueur2.Nom}\n");
        }

        // --------------------------------------------------------------
        //                  LECTURE DU DICTIONNAIRE
        // --------------------------------------------------------------
        private void ChargerDictionnaire()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "MotsFrancais.txt");

            if (!File.Exists(path))
            {
                Console.WriteLine("ERREUR : Le dictionnaire MotsFrancais.txt est introuvable !");
                return;
            }

            dico.ToRead(path);
            dico.Tri_XXX();

            Console.WriteLine("Dictionnaire chargé avec succès.\n");
        }

        // --------------------------------------------------------------
        //                       MENU PRINCIPAL
        // --------------------------------------------------------------
        private void MenuPrincipal()
        {
            while (true)
            {
                Console.WriteLine("\n=== MENU PRINCIPAL ===");
                Console.WriteLine("1 - Jouer à partir d'un fichier CSV");
                Console.WriteLine("2 - Générer un plateau aléatoire");
                Console.WriteLine("3 - Quitter");

                Console.Write("Votre choix : ");
                string? choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ChargerPlateauDepuisFichier();
                        LancerPartie();
                        break;

                    case "2":
                        GenererPlateauAleatoire();
                        LancerPartie();
                        break;

                    case "3":
                        Console.WriteLine("A bientôt !");
                        return;

                    default:
                        Console.WriteLine("Choix incorrect. Réessayez.");
                        break;
                }
            }
        }

        // --------------------------------------------------------------
        //                   CHARGER PLATEAU CSV
        // --------------------------------------------------------------
        private void ChargerPlateauDepuisFichier()
        {
            Console.Write("Chemin du fichier CSV : ");
            string? fichier = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fichier) || !File.Exists(fichier))
            {
                Console.WriteLine("Fichier invalide.");
                return;
            }

            plateau = new Plateau();
            plateau.ToRead(fichier);

            Console.WriteLine("Plateau chargé avec succès.\n");
            Console.WriteLine(plateau.ToString());
        }

        // --------------------------------------------------------------
        //                GENERATION ALEATOIRE PLATEAU
        // --------------------------------------------------------------
        private void GenererPlateauAleatoire()
        {
            Console.Write("Nombre de lignes du plateau : ");
            int lignes = LireEntierPositif();

            Console.Write("Nombre de colonnes du plateau : ");
            int colonnes = LireEntierPositif();

            string path = Path.Combine(AppContext.BaseDirectory, "Data", "Lettres.txt");

            if (!File.Exists(path))
            {
                Console.WriteLine("ERREUR : Fichier Lettres.txt introuvable !");
                return;
            }

            // Utilisation du constructeur dynamique de Plateau pour tailles personnalisées
            plateau = new Plateau(lignes, colonnes);
            plateau.GenererAleatoire(path);

            Console.WriteLine("\nPlateau généré :\n");
            Console.WriteLine(plateau.ToString());
        }

        private int LireEntierPositif()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int valeur) && valeur > 0)
                    return valeur;

                Console.Write("Valeur invalide. Entrez un entier positif : ");
            }
        }

        // --------------------------------------------------------------
        //               BOUCLE DE PARTIE (2 MIN)
        // --------------------------------------------------------------
        private void LancerPartie()
        {
            if (plateau == null || joueur1 == null || joueur2 == null)
            {
                Console.WriteLine("ERREUR : Partie impossible (éléments non initialisés).");
                return;
            }

            Console.WriteLine("\n=== La partie commence ! ===");

            DateTime debut = DateTime.Now;

            while (DateTime.Now - debut < dureePartie && !plateau.EstVide())
            {
                LancerTour(joueur1);
                if (DateTime.Now - debut >= dureePartie || plateau.EstVide()) break;

                LancerTour(joueur2);
                if (DateTime.Now - debut >= dureePartie || plateau.EstVide()) break;
            }

            Console.WriteLine("\n=== FIN DE PARTIE ===");
            Console.WriteLine(joueur1.toString());
            Console.WriteLine(joueur2.toString());

            if (joueur1.Score > joueur2.Score)
                Console.WriteLine($"{joueur1.Nom} a gagné !");
            else if (joueur2.Score > joueur1.Score)
                Console.WriteLine($"{joueur2.Nom} a gagné !");
            else
                Console.WriteLine("Égalité parfaite !");
        }

        // --------------------------------------------------------------
        //                     TOUR D'UN JOUEUR
        // --------------------------------------------------------------
        private void LancerTour(Joueur joueur)
        {
            if (plateau == null) return;

            Console.WriteLine($"\nTour de {joueur.Nom}");
            Console.WriteLine(plateau.ToString());

            DateTime debutTour = DateTime.Now;

            while (DateTime.Now - debutTour < dureeTour)
            {
                Console.Write("Entrez un mot : ");
                string? mot = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(mot))
                {
                    Console.WriteLine("Mot vide. Réessayez.");
                    continue;
                }

                mot = mot.Trim().ToUpper();

                if (joueur.Contient(mot))
                {
                    Console.WriteLine("Mot déjà trouvé !");
                    continue;
                }

                if (!dico.RechDichoRecursif(mot))
                {
                    Console.WriteLine("Mot inexistant dans le dictionnaire.");
                    continue;
                }

                var positions = plateau.Recherche_Mot(mot);

                if (positions == null)
                {
                    Console.WriteLine("Mot non présent sur le plateau.");
                    continue;
                }

                int score = plateau.CalculerScore(positions);
                joueur.Add_Score(score);
                joueur.Add_Mot(mot);

                plateau.Maj_Plateau(positions);

                Console.WriteLine($"Mot accepté ! +{score} points");
                Console.WriteLine("Plateau mis à jour :");
                Console.WriteLine(plateau.ToString());

                return;
            }

            Console.WriteLine("⏳ Temps écoulé pour ce tour !");
        }
    }
}
