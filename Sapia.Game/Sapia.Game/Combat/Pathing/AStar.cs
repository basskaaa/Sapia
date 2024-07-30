using System.Collections;

namespace Sapia.Game.Combat.Pathing;

public readonly struct AStarSettings
{
    public AStarSettings(int? maximumPathLength = null, float? maximumHeuristicCost = null)
    {
        MaximumHeuristicCost = maximumHeuristicCost;
        MaximumPathLength = maximumPathLength;
    }

    public float? MaximumHeuristicCost { get; }
    public int? MaximumPathLength { get; }
}

public readonly struct AStarResult<T> : IReadOnlyCollection<PathItem<T>>
{

    public AStarResult(IReadOnlyCollection<PathItem<T>> path)
    {
        Path = path;
        _hashSet = Path.Select(x => x.Node).ToHashSet();
        TotalCost = Path.Sum(x => x.Cost);
    }

    private readonly HashSet<T> _hashSet;
    public IReadOnlyCollection<PathItem<T>> Path { get; }
    public float TotalCost { get; }

    public int Count => Path.Count;
    public IEnumerator<PathItem<T>> GetEnumerator() => Path.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Path.GetEnumerator();

    public static AStarResult<T> Empty() => new(Array.Empty<PathItem<T>>());

    public AStarResult<T> PopOne() => new(Path.Skip(1).ToArray());

    public bool Contains(T entry) => _hashSet.Contains(entry);

    public T[] ToNodesArray() => Path.Select(x => x.Node).ToArray();
}

public class AStar<T>
{

    private readonly IPathManager<T> _pathManager;
    private readonly PriorityQueue<Path<T>> _queue = new();
    private readonly List<T> _visited = new();

    public AStar(IPathManager<T> pathManager)
    {
        _pathManager = pathManager;
    }

    public AStarResult<T>? GetPath(T start, T goal, AStarSettings settings = default)
    {
        if (_pathManager.EqualsOtherNode(start, goal))
        {
            return AStarResult<T>.Empty();
        }

        var outboundFromStart = _pathManager.OutboundPaths(start);

        // If the goal node is directly accessible from the start then just return a path between them
        if (outboundFromStart.Any(n => _pathManager.EqualsOtherNode(n, goal)))
        {
            var (actual, _) = _pathManager.GetCosts(start, goal, goal);

            return new AStarResult<T>(new[] { new PathItem<T>(goal, actual) });
        }

        _queue.Clear();
        _visited.Clear();

        // start a new search
        _queue.Enqueue(new Path<T>(null, start, 0, 0, 0));
        _visited.Add(start);

        while (!_queue.IsEmpty)
        {
            // Get the next path under consideration
            var path = _queue.Dequeue();

            if (settings.MaximumPathLength.HasValue && path.CurrentPath.Length > settings.MaximumPathLength.Value)
            {
                continue;
            }

            // Get next node
            var node = path.NextNode;

            var outboundPaths = _pathManager.OutboundPaths(node);

            // Add each non-visited outbound to IPQ
            foreach (var nextNode in outboundPaths)
            {
                if (_visited.Contains(nextNode))
                {
                    continue;
                }

                _visited.Add(nextNode);

                var (actualToNext, heuristicToGoal) = _pathManager.GetCosts(node, nextNode, goal);

                var newPath = new Path<T>(path.CurrentPath, nextNode, heuristicToGoal, actualToNext, path.ActualFromStartToNext);

                if (_pathManager.EqualsOtherNode(nextNode, goal))
                {
                    return new AStarResult<T>(newPath.CurrentPath);
                }

                if (!settings.MaximumHeuristicCost.HasValue || newPath.HeuristicFromNextToGoal < settings.MaximumHeuristicCost.Value)
                {
                    _queue.Enqueue(newPath);
                }
            }
        }

        // Nothing was found so return null
        return null;
    }
}