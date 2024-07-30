using Sapia.Game.Structs;

namespace Sapia.Game.Extensions;

public static class CoordExtensions
{
    public static IEnumerable<Coord> GetAdjacent(this Coord vector, bool includeDiagonal = true)
    {
        // R
        yield return vector + new Coord(1, 0);

        // DR
        if (includeDiagonal)
        {
            yield return vector + new Coord(1, 1);
        }

        // D
        yield return vector + new Coord(0, 1);

        // DL
        if (includeDiagonal)
        {
            yield return vector + new Coord(-1, 1);
        }

        // L
        yield return vector + new Coord(-1, 0);

        // UL
        if (includeDiagonal)
        {
            yield return vector + new Coord(-1, -1);
        }

        // U
        yield return vector + new Coord(0, -1);

        // UR
        if (includeDiagonal)
        {
            yield return vector + new Coord(1, -1);
        }
    }
}