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
                "Velkommen til Sænke Slagskibe! \n" +
                "Du har nu følgende muligheder: \n" +
                "1. Start nyt spil \n" +
                "2. Se highscores \n" +
                "3. Forlad spillet \n" +
                "4. Start et nyt spil";

            return output;
        }

        public string WrongMenuChoice()
        {
            return "Tryk venligst på en knap svarende til punkterne i menueen";
        }

        public (bool isValid, string message) IsValidName(string v)
        {
            string input = v.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return (false, "Spilleren skal have et navn");
            }
            else if (input == Players[0].Name || input == Players[1].Name)
            {
                return (false, "Spilleren eksisterer allerede");
            }
            else
            {
                return (true, "Spilleren er oprettet");
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
                $"Du er ved at placere et {ship.Name}, hvor ønsker du det skal være? \n" +
                $"Koordinater skrives ind som 'c4', for at placere på kolonne c, række 4 \n" +
                $"derefter fulgt at hvilken retning dit skib skal pege, eks: c4 tw \n" +
                "Retninger:\n" +
                "'tw' - to west\n" +
                "'te' - to east\n" +
                "'ts' - to south\n" +
                "'tn' - to north\n" +
                $"Dit skib har størrelsen: {ship.Size} felter"
                ;

            return output;
        }

        public (bool isValid, string message) PlaceShip(string str, Ship ship, int player)
        { //input is in the format "c4 te" - hopefully
            string input = str.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return (false, "Du må ikke angive en tom position");
            }

            if (input.Length != 5 || input[2] != ' ')
            {
                return (false, "kommandoen er ugyldig");
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
                        return (false, "Du prøver at placere et skib udenfor spillebanen mod nord");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart - i, xStart] = ship.Icon;
                    }
                    break;

                case "te":
                    if (xStart + ship.Size > Players[player].ShipMap.GetLength(1))
                    {
                        return (false, "Du prøver at placere et skib udenfor spillebanen mod øst");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart, xStart + i] = ship.Icon;
                    }
                    break;

                case "ts":
                    if (yStart + ship.Size > Players[player].ShipMap.GetLength(0))
                    {
                        return (false, "Du prøver at placere et skib udenfor spillebanen mod syd");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart + i, xStart] = ship.Icon;
                    }
                    break;

                case "tw":
                    if (xStart - ship.Size < 0)
                    {
                        return (false, "Du prøver at placere et skib udenfor spillebanen mod vest");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        Players[player].ShipMap[yStart, xStart - i] = ship.Icon;
                    }
                    break;

                default:
                    return (false, "du angav ikke en gyldig retning");
            }

            return (true, "Hvordan er du nået hertil!?");
        }

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
                        isStillAlive = true;
                    }
                }
            }

            if (isStillAlive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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

                return (true, "Du ramte et skib!");
            }
            else
            {
                enemyShipMap[y, x] = "o";
                player.HitMap[y, x] = "o";
                return (true, "Du ramte ved siden af");
            }
        }

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
                return (false, "Du må ikke angive en tom position", 0, 0);
            }

            if (input.Length != 2)
            {
                return (false, "kommandoen er ugyldig", 0, 0);
            }

            string stringXStart = input[0].ToString();
            int yValue = 0;
            int xValue = 0;

            if (!int.TryParse(input[1].ToString(), out yValue))
            {
                return (false, "Rækkeværdien er ugyldig", 0, 0);
            }

            if (!letterToInt.ContainsKey(stringXStart))
            {
                return (false, "kolonne værdien findes ikke", 0, 0);
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
                    return (false, "Du har allerede skudt på dette koordinat tidligere", 0, 0);
                }
            }
            else
            {
                return (false, "Du rammer udenfor kortet :(", 0, 0);
            }

            return (true, "Success", xValue, yValue);
        }

        public string StartRoundMsg()
        {
            Player player = Players[GetPlayerTurn];

            string output =
                $"{player.Name} Det er din tur, dine bræt ser ud som følgende: \n" +
                $"Dine skibsplaceringer:                             Dine skud på modstanderens bræt: \n";

            string[] hitMap = CreateMap(player.HitMap);
            string[] shipMap = CreateMap(player.ShipMap);

            for (int i = 0; i < hitMap.Length; i++)
            {
                output += shipMap[i] + "          " + hitMap[i];
                output += "\n";
            }

            output += "Skyd på modstanderen ved at skrive koordinater ind på hans skibe i formatet 'c3' eller 'd8'\n";

            return output;
        }
    }
}
