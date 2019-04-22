using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipsGame
{
    public class Player
    {
        public string Name { get; set; }
        public FlatMap ShipMap { get; } = new FlatMap();
        public FlatMap HitMap { get; } = new FlatMap();
        public Player()
        {}

        public (bool isValid, string Message) PlaceShip(string str, Ship ship)
        {
            return ShipMap.PlaceShip(str, ship);
        }
    }
}
