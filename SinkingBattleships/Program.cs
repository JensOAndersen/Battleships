using System;
using BattleshipsGame;

namespace SinkingBattleships
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Explanation of development (in danish)
            //4 2d arrays, hver spiller skal have 2 2d arrays der holder styr på hvor 
            //han /hun har ramt samt hvilke af sine egne brikker der er blevet ramt.

            /*
             * 1. Velkomst skærm, der viser hvem der har spillet de seneste spil, og hvem der har vundet i hvor mange ture
             * 2. Menupunktet 'start nyt spil'
             * 3. Indtastning af spillere
             * 4. Skiftes til at indtaste koordinater, og retning for de skibe man gerne vil placere'
             * 4a. Man skal have vist sit bræt så man allerede kan se hvor man har placeret sine skibe
             * 5. Spillet starter, brugerne skiftes til at give inputs til spillet i format "4c" eller "3d"
             * 5a. Man får feedback om man har ramt en modstanders skib, og man skal kunne se hvor man tidligere har skudt
             * 6. Når en spiller ikke har flere skibe der er hele slutter spillet, og en vinder bliver vist.
             * 7. Vinderen bliver skrevet ind i tekstfil over hvor mange ture det har taget ham at vinde
             * 8. Gå tilbage til punkt 1
             * 
             * Hvordan skal det her spil fungere, rent arkitektonisk?
             * - Jeg vil gerne have et spil, som jeg kan bruge i andre projekter.
             * - Eks, jeg kan tage spillet ind i et razor pages projekt, eller wpf projekt, og bruge det der
             * - Der skal være en eller anden form for logik i program.cs, for at skrive highscores og gui ud
             * - al spil-logik skal foregå i et class library
             * 
             * Min udviklingsprocess:
             * - Starte med at lave indskrivning af spillernavne
             * - Lave en metode som kan lave et 2d array om til en streng der kan skrives ud i konsollen, eller returneres til brugeren
             * - Der skal laves noget logik der håndterer spillernes ture
             * - Der skal laves en metode som modtaget brugerinput, validerer, og laver det om til en handling på brættet
             * - Logik til at håndtere selve spillet, give brugerne besked om deres skibe er ramt og så videre
             * - Postgame skal laves, give brugeren besked om hvem der vandt, samt at skrive highscores ind i highscores.txt
             */
            #endregion

            Game game = new Game("c:/output/battleships.txt");

            while (true)
            {
                Console.WriteLine(game.GreetMessage());

                //setting up game, and menu logic
                bool isSettingUp = true;
                while (isSettingUp)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.D1: //sets up player names

                            for (int i = 0; i < 2; i++)
                            {
                                Console.WriteLine("Please enter name for player " + (i + 1));

                                (bool isValid, string message) proposedName;

                                string name;

                                while (!(proposedName = game.IsValidName(name = Console.ReadLine())).isValid)
                                {
                                    Console.WriteLine(proposedName.message);
                                }
                                game.Players[i].Name = name;
                                Console.WriteLine(proposedName.message);
                            }
                            isSettingUp = false;
                            break;

                        case ConsoleKey.D2: //writes out highscore into a textfile
                            Console.WriteLine("Not Implemented yet");
                            break;

                        case ConsoleKey.D3: //exits the game
                            Environment.Exit(0);
                            break;

                        case ConsoleKey.D4: //replaces game with a new instance
                            game = new Game("C:/output/battleships.txt");
                            Console.Clear();
                            Console.WriteLine("Starting new game...");
                            Console.WriteLine(game.GreetMessage());
                            break;

                        default:
                            Console.WriteLine(game.WrongMenuChoice());
                            break;
                    }

                }



                //setting up player maps
                for (int playerNumber = 0; playerNumber < game.Players.Length; playerNumber++)
                {
                    Console.Clear();
                    foreach (var shipKvP in game.ShipsAvailableInGame)
                    {
                        for (int j = 0; j < shipKvP.Value; j++)
                        {
                            Console.WriteLine(game.ShipPlacementMessage(shipKvP.Key));

                            (bool isValid, string message) shipPlacementResult;

                            //ad ad ad, fiks det her
                            while (!(shipPlacementResult = game.PlaceShip(
                                         Console.ReadLine(),
                                         shipKvP.Key,
                                         playerNumber)
                                ).isValid)
                            {
                                Console.WriteLine(shipPlacementResult.message);
                            }
                            //Console.WriteLine(game.GetShipMapForPlayer(playerNumber));
                        }
                    }
                }

                Console.WriteLine("The game boards are ready, press enter to begin the game");
                Console.ReadKey(true);
                Console.Clear();

                //Main game loop
                do
                {
                    Console.Clear();

                    Console.WriteLine(game.StartRoundMsg());

                    (bool isValid, string message) shootAtEnemyResult;
                    while (!(shootAtEnemyResult = game.ShootAtEnemy(Console.ReadLine())).isValid)
                    {
                        Console.WriteLine(shootAtEnemyResult.message);
                    }

                    Console.WriteLine(shootAtEnemyResult.message);

                    Console.ReadKey(true);

                    game.Turns++;
                } while (game.IsRunning());
            }
        }
    }
}
