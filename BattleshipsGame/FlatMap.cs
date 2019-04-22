using System;
using System.Collections.Generic;
using System.Text;
using BattleshipsGame.Utils;

namespace BattleshipsGame
{

    /*
     * You are working on the validation methods, splitting them up sot he methods only return a single coherent value
     */
    public class FlatMap : MapAbstract
    {
        public FlatMap()
        {
            map = new Dictionary<(int x, int y), string>();
        }

        /// <summary>
        /// Sends a shot towards a coordinate
        /// </summary>
        /// <param name="str">The string coordinates in the format[a-j[0-9]</param>
        /// <returns>Whether its valid or not, the validation message</returns>
        public override (bool isValid, string message) ShootAtCoordinate((int x, int y) coordinate)
        {
            if (map.ContainsKey(coordinate))
            {
                if (map[coordinate] == "x" ||
                    map[coordinate] == "o")
                {
                    return (false, "This coordinate has already been shot at");
                } else
                {
                    map[coordinate] = "x";
                    return (true, "You hit a ship!!");
                }
            }
            else
            {
                map.Add(coordinate, "o");
                return (true, "You missed the shot unfortunately");
            }

        }

        /// <summary>
        /// Places a ship on the map
        /// </summary>
        /// <param name="str">the coordinate positions of the ship, as well as directions</param>
        /// <param name="ship">The ship to be placed</param>
        /// <returns>whether it succeeded or not, and a message</returns>
        public override (bool isValid, string message) PlaceShip(string str, Ship ship)
        {
            //input is already validated from the method calling this

            int yStart = 0;
            int xStart = 0;

            var validationResult = Validators.IsValidCoordinate(str.Substring(0, 2));

            if (validationResult.isValid)
            {
                var conversionResult = Converters.StringToCoordinate(str.Substring(0, 2));
                yStart = conversionResult.y;
                xStart = conversionResult.x;
            }
            else
            {
                return validationResult;
            }

            string direction = str.Substring(3);

            switch (direction)
            {
                case "tn":
                    if (yStart - ship.Size < 0)
                    {
                        return (false, "You are trying to place a ship outside the playing field towards north");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map.Add((xStart, yStart - i), ship.Icon);
                    }
                    break;

                case "te":
                    if (xStart + ship.Size > mapXSize)
                    /*TODO:
                    * There might be an edge case error here, as mapXSize is the max size with the last value excluded, as arrays with a length of 10 has the max index of 9
                    * create unit test for this?
                    */
                    {
                        return (false, "You are trying to place a ship outside the playing field towards east");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map.Add((xStart + i, yStart), ship.Icon);
                    }
                    break;

                case "ts":
                    if (yStart + ship.Size > mapYSize)
                    {
                        return (false, "You are trying to place a ship outside the playing field towards south");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map.Add((xStart, yStart + i), ship.Icon);
                    }
                    break;

                case "tw":
                    if (xStart - ship.Size < 0)
                    {
                        return (false, "You are trying to place a ship outside the playing field towards west");
                    }

                    for (int i = 0; i < ship.Size; i++)
                    {
                        map.Add((xStart - i, yStart), ship.Icon);
                    }
                    break;

                default:
                    return (false, "You entered an invalid direction");
            }

            return (true, "How did you even reach this case?!");
        }

    }
}
