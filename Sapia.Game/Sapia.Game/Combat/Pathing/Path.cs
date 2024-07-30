namespace Sapia.Game.Combat.Pathing;

public readonly struct Path<T> : IComparable<Path<T>>
{
    /// <summary>
    /// The heuristic cost to reach the goal from the next node
    /// </summary>
    public float HeuristicFromNextToGoal { get; }

    /// <summary>
    /// The actual cost to move to the next node
    /// </summary>
    public float ActualFromPreviousToNext { get; }

    /// <summary>
    /// The total actual cost of all nodes visited so far, including the next actual cost
    /// </summary>
    public float ActualFromStartToNext { get; }

    public PathItem<T>[] CurrentPath { get; }

    public T NextNode => CurrentPath[^1].Node;

    public Path(IReadOnlyCollection<PathItem<T>>? pathToHere, T newNode, float heuristicCost, float actualFromPreviousToNext, float actualPreviousTotalCost)
    {
        ActualFromPreviousToNext = actualFromPreviousToNext;
        ActualFromStartToNext = actualPreviousTotalCost + actualFromPreviousToNext;
        HeuristicFromNextToGoal = heuristicCost + ActualFromStartToNext;

        CurrentPath = (pathToHere ?? Array.Empty<PathItem<T>>())
            .Append(new PathItem<T>(newNode, actualFromPreviousToNext))
            .ToArray();
    }

    public int CompareTo(Path<T> other)
    {
        return HeuristicFromNextToGoal.CompareTo(other.HeuristicFromNextToGoal);
    }
}

public readonly struct PathItem<T>
{
    public PathItem(T node, float cost)
    {
        Node = node;
        Cost = cost;
    }

    public T Node { get; }
    public float Cost { get; }
}