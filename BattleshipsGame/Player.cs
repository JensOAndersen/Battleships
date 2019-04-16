using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipsGame
{
    public class Player
    {
        public string Name { get; set; }
        public FlatMap ShipMap { get; set; } = new FlatMap();
        public FlatMap HitMap { get; set; } = new FlatMap();
        public Player()
        {}
    }
}
