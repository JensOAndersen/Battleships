using System;
using System.Collections.Generic;
using System.Text;
using BattleshipsGame.Utils;

namespace BattleshipsGame
{

    /*
     * You are working on the validation methods, splitting them up sot he methods only return a single coherent value
     */
    public class FlatMap : Map
    {
        private string[,] map;

        //this needs to be refactored so the map itself cant be accessed outside the flatmap class
        public string[,] Map
        {
            get
            {
                return map;
            }
        }

        public FlatMap()
        {

            map = new string[mapYSize, mapXSize];

            PopulateMap(map);
        }

        /// <summary>
        /// Sends a shot towards a coordinate
        /// </summary>
        /// <param name="str">The string coordinates in the format[a-j[0-9]</param>
        /// <returns>Whether its valid or not, the validation message</returns>
        public override(bool isValid, string message) MarkCoordinate(string str, char icon)
        {
            str = str.Trim();

            int x = 0;
            int y = 0;

            try
            {
                var validationResult = Converters.StringToCoordinate(str);

                x = validationResult.x;
                y = validationResult.y;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            if (
                map[y, x] == "x" ||
                map[y, x] == "o" 
                )
            {
                return (false, "This coordinate has already been marked");
            }
            else
            {
                map[y, x] = icon.ToString();
                return (true, "Success!");
            }
        }

        /// <summary>
        /// Places a ship on the map
        /// </summary>
        /// <param name="str">the coordinate positions of the ship, as well as directions</param>
        /// <param name="ship">The ship to be placed</param>
        /// <returns>whether it succeeded or not, and a message</returns>
        public override(bool isValid, string message) PlaceShip(string str, Ship ship)
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

            var validationResult = Validators.IsValidCoordinate(input.Substring(0, 2));

            if (validationResult.isValid)
            {
                var conversionResult = Converters.StringToCoordinate(input.Substring(0, 2));
                yStart = conversionResult.y;
                xStart = conversionResult.x;
            }
            else
            {
                return (false, validationResult.message);
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
                        map[yStart - i, xStart] = ship.Icon;
                    }
                    break;

                case "te":
                    if (xStart + ship.Size > map.GetLength(1))
                    {
                        return (false, "You are trying to place a ship outside the playing field towards east");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map[yStart, xStart + i] = ship.Icon;
                    }
                    break;

                case "ts":
                    if (yStart + ship.Size > map.GetLength(0))
                    {
                        return (false, "You are trying to place a ship outside the playing field towards south");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map[yStart + i, xStart] = ship.Icon;
                    }
                    break;

                case "tw":
                    if (xStart - ship.Size < 0)
                    {
                        return (false, "You are trying to place a ship outside the playing field towards west");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map[yStart, xStart - i] = ship.Icon;
                    }
                    break;

                default:
                    return (false, "You entered an invalid direction");
            }

            return (true, "How did you even reach this case?!");
        }

    }
}
