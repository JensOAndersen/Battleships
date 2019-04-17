using System.Collections;

namespace BattleshipsGame
{
    public interface IMap
    {
        IEnumerable Map { get; }
    }
}