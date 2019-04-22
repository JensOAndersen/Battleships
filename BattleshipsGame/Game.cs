using BattleshipsGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipsGame
{
    public class Game
    {
        public Player[] Players { get; set; } = new Player[2] {
            new Player(),
            new Player()
        };

        public int Turns
        {
            get;
            set;
        }
        public int GetPlayerTurn
        {
            get { return Turns % 2; }
        }

        /// <summary>
        /// Key being the ship, and value being the amount of ships the player can place in the game
        /// </summary>
        public Dictionary<Ship, int> ShipsAvailableInGame { get; } = new Dictionary<Ship, int>
        {
            //{new Ship{Icon = "A", Name = "Aircraft Carrier", Size = 6 }, 1 },
            //{new Ship{Icon = "B", Name = "Battleship", Size = 5 }, 1 },
            {new Ship{Icon = "C", Name = "Cruiser", Size = 4 }, 1 }//,
            //{new Ship{Icon = "D", Name = "Destroyer", Size = 3 }, 2 },
            //{new Ship{Icon = "S", Name = "Submarine", Size = 1 }, 1 }
        };

        private string saveDir;

        public Game(string saveDir)
        {
            this.saveDir = saveDir;
        }

        public string GreetMessage()
        {
            string output =
                "Welcome to Battleships \n" +
                "You have the following menu options: \n" +
                "1. Enter player names and begin\n" +
                "2. Highscores\n" +
                "3. Leave the game\n" +
                "4. Create a new game with new players";

            return output;
        }

        public string WrongMenuChoice()
        {
            return "Please press a button corresponding to a menu item";
        }

        /// <summary>
        /// Validates a name with IsNullOrEmpty, and whether it currently exists
        /// </summary>
        /// <param name="name">Name to be validated</param>
        /// <returns>A boolean indicating whether it was a success or not, and a message</returns>
        public (bool isValid, string message) IsValidName(string name)
        {
            string input = name.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return (false, "The player must have a name");
            }
            else if (input == Players[0].Name || input == Players[1].Name)
            {
                return (false, "The player already exists");
            }
            else
            {
                return (true, "The player has been created");
            }
        }

        /// <summary>
        /// Call this method whenever a user is placing a ship, it returns a message about what ship is about to be placed
        /// </summary>
        /// <param name="ship">The ship to be placed</param>
        /// <returns>a string describing the ship to be placed</returns>
        public string ShipPlacementMessage(Ship ship)
        {
            string output =
                $"You are placing a {ship.Name}, where do you want it to be? \n" +
                $"Koordinates are entered in the following format 'c4' for column c, row 4\n" +
                $"followed by the direction you want the ship to point, ex: 'c4 tw'\n" +
                "directions:\n" +
                "'tw' - to west\n" +
                "'te' - to east\n" +
                "'ts' - to south\n" +
                "'tn' - to north\n" +
                $"your takes up {ship.Size} fields"
                ;

            return output;
        }

        /// <summary>
        /// Returning true if the game is still running
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            string[,] enemyShipMap = Players[(Turns + 1) % 2].ShipMap.Map;

            bool isStillAlive = false;


            foreach (var item in enemyShipMap)
            {
                if (ShipsAvailableInGame.Keys.Where(ship => ship.Icon == item).Count() == 1)
                {
                    return true;
                }
            }

            return isStillAlive;
        }

        /// <summary>
        /// Tries to shoot at a coordinate
        /// </summary>
        /// <param name="input">string coordinate in the format [a-j][0-10]</param>
        /// <returns>IsValid is a bool indicating whether or not it is possible to shoot at the location, message is the returning message</returns>
        public (bool isValid, string message) ShootAtEnemy(string input)
        {
            input = input.ToLower();

            var validationResult = Validators.IsValidCoordinate(input);
            if (!validationResult.isValid)
            {
                return (false, validationResult.message);
            }

            //burde opdatere dette til next player eller enemyplayer properties/metoder
            Player enemyPlayer = Players[(Turns + 1) % 2];
            FlatMap enemyShipMap = enemyPlayer.ShipMap;

            Player player = Players[GetPlayerTurn];


            var coordinate = Converters.StringToCoordinate(input);

            //this is pretty ugly :/
            if (ShipsAvailableInGame.Keys.Where(ship => ship.Icon == enemyPlayer.ShipMap.Map[coordinate.y, coordinate.x]).Count() == 1)
            {
                enemyShipMap.MarkCoordinate(input, 'x');
                player.HitMap.MarkCoordinate(input, 'x');

                return (true, "You hit a ship");
            }
            else
            {
                enemyShipMap.MarkCoordinate(input, 'o');
                player.HitMap.MarkCoordinate(input, 'o');
                return (true, "You missed");
            }
        }

        public (bool isValid, string message) PlaceShip(string input, Ship ship, int playerNumber)
        {
            input = input.ToLower();
            return Players[playerNumber].PlaceShip(input, ship);
        }

        /// <summary>
        /// Greeting message at the start of each round
        /// </summary>
        /// <returns>A greeting message</returns>
        public string StartRoundMsg()
        {
            Player player = Players[GetPlayerTurn];

            string output =
                $"{player.Name} It is your turn, here is your maps: \n" +
                $"Placement of your ships:                             Your shots at the opponents board: \n";

            string[] hitMap = FlatMap.CreateMap(player.HitMap);
            string[] shipMap = FlatMap.CreateMap(player.ShipMap);

            for (int i = 0; i < hitMap.Length; i++)
            {
                output += shipMap[i] + "          " + hitMap[i];
                output += "\n";
            }

            output += "Enter a coordinate on the opponents map, by entering coordinates in the format 'c3' or 'd8'";

            return output;
        }
    }
}
