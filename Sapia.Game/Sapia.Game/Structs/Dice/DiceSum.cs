namespace Sapia.Game.Structs.Dice;

public readonly struct DiceSum : IDiceValue
{
    public DiceSum(IReadOnlyCollection<IDiceValue> values)
    {
        Values = values;
    }

    public IReadOnlyCollection<IDiceValue> Values { get; }

    public override string ToString()
    {
        if (Values.Count == 0)
        {
            return "0";
        }
        if (Values.Count == 1)
        {
            return Values.First().ToString();
        }

        var vals = string.Join(" + ", Values.Select(x => x.ToString()));

        return $"({vals})";
    }
}