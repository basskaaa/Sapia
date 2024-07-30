namespace CoA.Pathing;

public interface IPathManager<T>
{
    // Get the list of outbound paths from this node
    IReadOnlyCollection<T> OutboundPaths(T node);

    // Get the heuristic cost to the goal
    (float actualToNext, float heuristicToGoal) GetCosts(T currentNode, T nextNode, T goal);

    // Check whether this node is the same as another node
    bool EqualsOtherNode(T a, T b);
}