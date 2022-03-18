using System;

/* This is the final project for project01 at Cegep de Shawinigan
 *
 *####### You need to enter 0 first for number below 10 , Like a real machine so 2 is 02 ####################
 * 
 * 
 * -This program will handle any invalid input. It seem over engineered but its nice and pretty for the user
 * All the message are displayed on the pad of the machine 
 * -All the input are handled without pressing enter .
 * -Will display a message for almost all action .
 * -The function get candy was a bit .... you know .
 * Johny Pineault  
 */


namespace CandyMachine
{
    public class Program
    {
        public static class GlobalData
        {
            public static bool StillRun = true;
            public static Data dataManager = new Data();
            public static Candy[] candies = dataManager.LoadCandies();
            public static decimal moneyIn = 0m;
            public static decimal moneyOut = 0m;
            public static decimal selectionCost = 0m;

            public static int userSelect;
            // public static decimal = 0m;
        }


        public static void Main()
        {
            // Varriable declaration 
            string? continueSelection = null;
            string? message = null; //Used for displayin different mesasge in the for loop


            do
            {
                // Reset all the variable
                VarReset();
                
                // === Get selection ===
                GlobalData.userSelect = GetSelection(25);
                
                // === Check for stock ===
                if (GlobalData.candies[GlobalData.userSelect - 1].Stock == 0)
                {
                    DisplayRefresh("Vide desoler.");    // Refresh the display
                    Thread.Sleep(2000);         // Wait to display the empty message
                    Main();                                   // return to main
                }
                else // Stock is available lets continue
                {
                    GlobalData.selectionCost = GlobalData.candies[GlobalData.userSelect - 1].Price; // put cost price
                    DisplayRefresh(GlobalData.candies[GlobalData.userSelect - 1].Name);     // Refresh display to show it
                }

                // === Get coin ===
                
                // will do the function until the money is equal or greater than the price
                do
                {
                    DisplayRefresh(GlobalData.candies[GlobalData.userSelect - 1].Name);
                    GlobalData.moneyIn = GlobalData.moneyIn + GetCoin(GlobalData.moneyIn);      
                } while (GlobalData.moneyIn < GlobalData.candies[GlobalData.userSelect - 1].Price);


                // === Give the good stuff ! ===

                // Lets charge the item based on received minus item price
                GlobalData.moneyIn = GlobalData.moneyIn - GlobalData.candies[GlobalData.userSelect - 1].Price;

                // Lets do the magic to give item
                Console.Clear();
                GlobalData.moneyOut = GlobalData.moneyIn; // swith money to out
                GlobalData.moneyIn = 0m; // empty money in

                if (GlobalData.moneyOut == 0) // Display different message if there is money left or not
                {
                    message = "Ramasser item";
                }
                else
                {
                    message = "Ramasser item et monaies";
                }

                // Print the result with the message from above
                DisplayRefresh(message, GlobalData.candies[GlobalData.userSelect - 1].Name);
                SoundLibrary("success");

                // Lets remove one of the item from the stock
                GlobalData.candies[GlobalData.userSelect - 1].Stock--;

                // Lets reset the variable
                VarReset();
                Console.WriteLine("Enter pour ramasser.");
                Console.ReadLine();

                // === Ending ! ===
                do
                {
                    Console.Clear();
                    Board.Print("Voulez faire autre choix ?(y/n)?", 0, 0m, 0, 0, "");

                    // Let user make choice yes or no if he want to continue
                    switch (UserCharInput(1, "", true))
                    {
                        case "y":
                            continueSelection = "y"; // Modify the running value
                            break;
                        case "n":
                            continueSelection = "n"; // Modify the running value
                            Console.Clear();
                            Board.Print("Merci et a la prochaine", 0, 0m, 0, 0, "");
                            GlobalData.StillRun = false; // Modify the global running value 
                            Environment.Exit(1);
                            
                            break;
                    }
                } while ((continueSelection != "y") &&
                         (continueSelection != "n")); // this make sure the input is y or o
            } while (GlobalData.StillRun == true); // All the big loop run on this
        }
        // ############################# Function for candy machine ####################################################


        // ----------- Get Selection -----------
        public static int GetSelection(int max)
        {
            int selection = 1;

            do
            {
                if (selection > 25 || selection < 1)
                {
                    DisplayRefresh("Entrer erroner essayer encore");
                    selection = NewUserNumberInput(2, "--->", true, false);
                    SoundLibrary("error");
                }
                else
                {
                    DisplayRefresh("Faite une selection");
                    selection = NewUserNumberInput(2, "--->", true, false);
                }
            } while ((selection <= 0) || (selection > (max))); //Make a selection until the number is bewten 0 and max

            return selection;
        }


        // ---------- Get Coin ---------------
        public static decimal GetCoin(decimal moneyIn) // We have money in parameter for displaying on the screen
        {
            decimal coin = 0m;
            ConsoleKeyInfo userInput;
            int input;


            Console.WriteLine("#################\n" +
                              "# [0] = Annuler #\n" +
                              "# [1] = 5c      #\n" +
                              "# [2] = 10c     #\n" +
                              "# [3] = 25c     #\n" +
                              "# [4] = 1$      #\n" +
                              "# [5] = 2$      #\n" +
                              "#################");

            input = NewUserNumberInput(1, "-->", false, true);


            switch (input)
            {
                case 0:
                    SoundLibrary("cancel");

                    if (moneyIn > 0)
                    {
                        GlobalData.moneyOut = GlobalData.moneyIn;
                        GlobalData.moneyIn = 0;
                        DisplayRefresh("Annuler", "Voici votre argent");
                        Console.WriteLine("Appuyer sur enter");
                        Console.ReadLine();
                    }

                    Main();

                    break;

                case 1:
                    coin = 0.05m;
                    SoundLibrary("coin"); // Coin sound
                    break;
                case 2:
                    coin = 0.10m;
                    SoundLibrary("coin"); // Coin sound
                    break;
                case 3:
                    coin = 0.25m;
                    SoundLibrary("coin"); // Coin sound
                    break;
                case 4:
                    coin = 1m;
                    SoundLibrary("coin"); // Coin sound
                    break;
                case 5:
                    coin = 2m;
                    SoundLibrary("coin"); // Coin sound
                    break;
                default:
                    break;
            }

            return coin;
        }

        // ----------- Get Candy ------------
        public static string GetCandy(int selection)
        {
            return GlobalData.candies[selection - 1].Name;
        }

        // ############################# Function used in other function aka common function ###########################

        public static void VarReset()
        {
            GlobalData.moneyIn = 0m;        // Will reset all the variable used in the program
            GlobalData.moneyOut = 0m;
            GlobalData.selectionCost = 0m;
            GlobalData.userSelect = 0;
        }

        public static void DisplayRefresh(string message = $"Faite votre choix", string endMessage = ":)")
        {
            // Will refresh the display .. not so much but save the big board print line and console clear .
            
            Console.Clear();
            Board.Print(message,
                GlobalData.userSelect,
                GlobalData.selectionCost,
                GlobalData.moneyIn,
                GlobalData.moneyOut,
                endMessage);
        }

        public static int NewUserNumberInput(int lenght = 1, string texte = "", bool sound = false,
            bool errorHandle = true)
        {
            
            // This is my new user input function :) 
            // Will handle the error input if the parameter is set to true
            
            ConsoleKeyInfo userInput;
            char? userInputString = null;
            string? output = null;
            int finalOutput;
            bool tryParse = false; // Used for try parse if we dont enter number 


                do
                {
                    Console.SetCursorPosition(0, Console.CursorTop); // Replace the cursor at the start of the line

                    Console.Write(texte); //Write text if there is any

                    finalOutput = 0; // Lets reset the value , used with while try parse
                    output = null; // Lets reset the value , used with while try parse

                    // For the leng of number total we will run it . ex : 05 or 14 is lenght 2 
                    for (int i = 0; i < lenght; i++)
                    {
                        userInput = Console.ReadKey(); // Read the key to special magic char
                        userInputString = userInput.KeyChar; // Convert the magic char to only single char
                        output = output +
                                 userInputString; // Add the char to a string if the number is more than 1 digit

                        // If sound is true will beep each time a key is pressed
                        if (sound == true)
                        {
                            SoundLibrary("beep");
                        }
                    }

                    tryParse = int.TryParse(output, out finalOutput); // Parse the number
                    
                    if (errorHandle == false)
                    {
                        tryParse = true; // Skip the error handling and ship the value as is 
                    }
                    
                } while (tryParse != true);
                
            return finalOutput;
        }


        public static string UserCharInput(int lenght = 1, string texte = "", bool sound = false)
        {
            ConsoleKeyInfo userInput;
            char? userInputString = null;
            string output = null;

            /*Will return a string of a said lenght with . can show a string of text with console.write
             * will output a beep if sound is true 
             */

            Console.Write(texte); //Write text if there is any


            // For the leng of number total we will run it . ex : 05 or 14 is lenght 2 
            for (int i = 0; i < lenght; i++)
            {
                userInput = Console.ReadKey(); // Read the key to special magic char
                userInputString = userInput.KeyChar; // Convert the magic char to only single char
                output = output +
                         userInputString; // Add the char to a string if the number is more than 1 digit

                // If sound is true will beep each time a key is pressed
                if (sound == true)
                {
                    Console.Beep(1500, 100);
                }
            }


            return output;
        }

        public static void SoundLibrary(string sound = "beep")
        {
            // This is my lill sound library for this program .
            
            switch (sound)
            {
                case "beep":
                    Console.Beep(1500, 100); //Pad press sound
                    break;
                case "coin":
                    Console.Beep(1300, 100); // Coin sound
                    Console.Beep(2500, 100);
                    break;
                case "cancel":
                    Console.Beep(2600, 100); // Cancel Sound
                    Console.Beep(1600, 100);
                    Console.Beep(800, 100);
                    break;
                case "success":
                    Console.Beep(1000, 100); // Success sound
                    Console.Beep(2000, 100);
                    Console.Beep(3000, 100);
                    break;
                case "error":
                    Console.Beep(2600, 100); // error Sound
                    Console.Beep(800, 100);
                    break;
                default:
                    Console.Beep(500, 100); // Default sound
                    break;
            }
        }
        // End 
    }
}