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

        public int Turns { get; set; }
        public int GetPlayerTurn
        {
            get { return Turns % 2; }
        }

        /// <summary>
        /// Key being the ship, and value being the amount of ships the player can place in the game
        /// </summary>
        public Dictionary<Ship, int> ShipsAvailableInGame { get; } = new Dictionary<Ship, int>
        {
            {new Ship{Icon = "A", Name = "Aircraft Carrier", Size = 6 }, 1 },
            {new Ship{Icon = "B", Name = "Battleship", Size = 5 }, 1 },
            {new Ship{Icon = "C", Name = "Cruiser", Size = 4 }, 1 },
            {new Ship{Icon = "D", Name = "Destroyer", Size = 3 }, 2 },
            {new Ship{Icon = "S", Name = "Submarine", Size = 1 }, 1 }
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

        public string GetHitMapForPlayer(int player)
        {
            return string.Join('\n', CreateMap(Players[player].HitMap));
        }

        public string GetShipMapForPlayer(int player)
        {
            return string.Join('\n', CreateMap(Players[player].ShipMap));
        }

        private string[] CreateMap(string[,] map)
        {
            List<string> output = new List<string>();
            output.Add("------------------------------------------");

            for (int y = 0; y < map.GetLength(0); y++)
            {
                string line = "|";

                for (int x = 0; x < map.GetLength(1); x++)
                {
                    line += $" {map[y, x]} |";
                }
                output.Add(line);
                output.Add("------------------------------------------");
            }

            return output.ToArray();
        }

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

        public (bool isValid, string message) PlaceShip(string str, Ship ship, int player)
        { //input is in the format "c4 te" - hopefully
            string input = str.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return (false, "You are not allowed to enter an empty position");
            }

            if (input.Length != 5 || input[2] != ' ')
            {
                return (false, "The command is invalid");
            }

            int yStart = 0;
            int xStart = 0;

            var stringToCoordinateResult = StringToCoordinate(input.Substring(0, 2));

            if (!stringToCoordinateResult.isValid)
            {
                return (false, stringToCoordinateResult.message);
            }
            else
            {
                yStart = stringToCoordinateResult.y;
                xStart = stringToCoordinateResult.x;
            }

            string direction = input.Substring(3);

            switch (direction)
            {
                case "tn":
                    if (yStart - ship.Size < 0)
                    {
                        return (false, "You are trying to place a ship outside the playing field towards north");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart - i, xStart] = ship.Icon;
                    }
                    break;

                case "te":
                    if (xStart + ship.Size > Players[player].ShipMap.GetLength(1))
                    {
                        return (false, "You are trying to place a ship outside the playing field towards east");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart, xStart + i] = ship.Icon;
                    }
                    break;

                case "ts":
                    if (yStart + ship.Size > Players[player].ShipMap.GetLength(0))
                    {
                        return (false, "You are trying to place a ship outside the playing field towards south");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart + i, xStart] = ship.Icon;
                    }
                    break;

                case "tw":
                    if (xStart - ship.Size < 0)
                    {
                        return (false, "You are trying to place a ship outside the playing field towards west");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart, xStart - i] = ship.Icon;
                    }
                    break;

                default:
                    return (false, "You entered an invalid direction");
            }

            return (true, "How did you even reach this case?!");
        }

        /// <summary>
        /// Returning true if the game is still running
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            string[,] enemyShipMap = Players[(Turns + 1) % 2].ShipMap;

            bool isStillAlive = false;

            for (int yPos = 0; yPos < enemyShipMap.GetLength(0); yPos++)
            {
                for (int xPos = 0; xPos < enemyShipMap.GetLength(1); xPos++)
                {
                    if (ShipsAvailableInGame.Keys.Where(ship => ship.Icon == enemyShipMap[yPos, xPos]).Count() == 1)
                    {
                        return true;
                    }
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
            var stringToCoordinateResult = StringToCoordinate(input);
            if (!stringToCoordinateResult.isValid)
            {
                return (false, stringToCoordinateResult.message);
            }

            int x = stringToCoordinateResult.x;
            int y = stringToCoordinateResult.y;

            //burde opdatere dette til next player eller enemyplayer properties/metoder
            Player enemyPlayer = Players[(Turns + 1) % 2];
            string[,] enemyShipMap = enemyPlayer.ShipMap;

            Player player = Players[GetPlayerTurn];

            //this is pretty ugly :/
            if (ShipsAvailableInGame.Keys.Where(ship => ship.Icon == enemyPlayer.ShipMap[y, x]).Count() == 1)
            {
                enemyShipMap[y, x] = "x";
                player.HitMap[y, x] = "x";

                return (true, "You hit a ship");
            }
            else
            {
                enemyShipMap[y, x] = "o";
                player.HitMap[y, x] = "o";
                return (true, "You missed");
            }
        }

        /// <summary>
        /// Transforms and validates a coordinate string
        /// </summary>
        /// <param name="str">the string to be transformed into coordinates</param>
        /// <returns>Whether its valid or not, the validation message, the x and y positions</returns>
        private (bool isValid, string message, int x, int y) StringToCoordinate(string str)
        {
            string input = str.Trim();

            Dictionary<string, int> letterToInt = new Dictionary<string, int>
            {
                {"a",0 },
                {"b",1 },
                {"c",2 },
                {"d",3 },
                {"e",4 },
                {"f",5 },
                {"g",6 },
                {"h",7 },
                {"i",8 },
                {"j",9 },
            };

            if (string.IsNullOrEmpty(input))
            {
                return (false, "You are not allowed to enter an empty position", 0, 0);
            }

            if (input.Length != 2)
            {
                return (false, "The command is invalid", 0, 0);
            }

            string stringXStart = input[0].ToString();
            int yValue = 0;
            int xValue = 0;

            if (!int.TryParse(input[1].ToString(), out yValue))
            {
                return (false, "The row value is invalid", 0, 0);
            }

            if (!letterToInt.ContainsKey(stringXStart))
            {
                return (false, "The column value is invalid", 0, 0);
            }
            else
            {
                xValue = letterToInt[stringXStart];
            }

            //test if x and y is within map, and if you have targeted the same location before
            int yMin = 0;
            int xMin = 0;
            int yMax = Players[GetPlayerTurn].HitMap.GetLength(0);
            int xMax = Players[GetPlayerTurn].HitMap.GetLength(1);

            Player player = Players[GetPlayerTurn];

            if (xValue >= xMin &&
                xValue < xMax &&
                yValue >= yMin &&
                yValue < yMax)
            {
                if (
                    player.HitMap[yValue, xValue] == "x" ||
                    player.HitMap[yValue, xValue] == "o")
                {
                    return (false, "This coordinate has already been hit once", 0, 0);
                }
            }
            else
            {
                return (false, "You're aiming outside the map",0, 0);
            }

            return (true, "Success", xValue, yValue);
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

            string[] hitMap = CreateMap(player.HitMap);
            string[] shipMap = CreateMap(player.ShipMap);

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
