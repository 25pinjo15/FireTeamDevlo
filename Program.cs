// See https://aka.ms/new-console-template for more information
// les fichiers candies.data , board.cs ont ete modifiés
using System.Runtime.InteropServices;
using CandyMachine;

namespace MyNamespace
{ 
    class Program
    {
        public static Data dataManager = new Data();//declaration de mon tableau
        public static Candy[] candies = dataManager.LoadCandies();
        public static void Main() //appel de mon main
        {
            decimal argent = 0m;
            int selection = GetSelection(25); // declaration de ma variable avec getselection
            Candy candy = GetCandy(selection); // declaration avec mon getcandy
            
            if (candy.Stock == 0) // veifiation du stock
            {
                Board.Print("vide", result: "enter pour autre choix");
                Console.ReadLine();
                Main();
            }
            else
            {
                Board.Print($"{candy.Name}", selection, candy.Price, 0m, 0m,
                    $"{candy.Stock} en stock");
            }
            
            do  // boucle pour mon argent
            {
                argent = GetCoin(argent);
                Board.Print($"{candy.Name}", selection, candy.Price, argent, 0m,
                    $"{candy.Stock} en stock");
            } while (argent < candy.Price);

            if (argent >= candy.Price) // condition quand jai largent necessaire
            {
                candy.Stock--;
                Board.Print($"prenez votre bonbon", selection, candy.Price, argent, argent - candy.Price,
                    $"{candy.Stock} en stock");
                
                BuyOrQuit();
            }
        }
        public static int GetSelection(int maxi) //fonction pour allez chercher la selection du user
        {
            int selection;
            do
            {
                selection = UserNumberInput("-> votre selection (1-25) : ");
            } while (selection is < 1 || selection > maxi); // tant que la selection est pas dans les parametre

            return selection;
        }
        public static int UserNumberInput(string texte) //fonction pour enlever erreur quand autre chose quun nombre est entrer
        { // === Variable declaration
            int output;
            bool tryParse;
            // === Function main
            do
            {
                Console.Clear();
                Board.Print();
                Console.Write(texte);
                tryParse = int.TryParse(Console.ReadLine(), out output); 
            } while (!tryParse);

            return output;
        }
        public static Candy GetCandy(int input) // fonction pour retourner la selection dans le tableau 
        {
            return candies[input - 1];
        }
        public static decimal GetCoin(decimal cash) //fonction accumuler lentrer dargent 
        {
            Console.WriteLine("[0] = Annuler\n" +
                              "[1] = 5c\n" +
                              "[2] = 10c\n" +
                              "[3] = 25c\n" +
                              "[4] = 1$\n" +
                              "[5] = 2$\n");
            string userinput = Console.ReadLine();


            switch (userinput)
            {
                case "0":
                    if (cash > 0)
                    {
                        Console.Clear();
                        Board.Print($"remboursé ", returned: cash);
                        BuyOrQuit();
                    }
                    else 
                    {
                        Console.Clear();
                        Board.Print("annuler");
                        BuyOrQuit();
                    }
                    break;
                case "1":
                    cash += 0.05m;
                    break;
                case "2":
                    cash += 0.10m;
                    break;
                case "3":
                    cash += 0.25m;
                    break;
                case "4":
                    cash += 1m;
                    break;
                case "5":
                    cash += 2m;
                    break;
            }
            return cash;
        }

        public static void BuyOrQuit() // fonction pour acheter a nouveau ou quitter sans avoir derreur
        {
            
            Console.Write("autre achat ? (y/n) : ");
            string quit = Console.ReadLine();
                
            if (quit == "n")
            {
               Board.Print(result:"bonne fin de journée");
               Thread.Sleep(1000);
               Environment.Exit(0);
            }
            else if (quit == "y")
            {
                Main();
            }
            else
            {
                Console.Clear();
                Board.Print();
                BuyOrQuit();
            }
        }
    }
} 