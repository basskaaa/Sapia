namespace Sapia.Game.Combat.AI.Entities;

public struct AiTurn
{
    public HashSet<DecisionAttempts> Decisions = new();

    public AiTurn()
    {
    }


    private static readonly IReadOnlyCollection<DecisionAttempts> _allDecisions = (DecisionAttempts[])Enum.GetValues(typeof(DecisionAttempts));

    public bool HasMadeAllDecisions()
    {
        foreach (var decision in _allDecisions)
        {
            if (!Decisions.Contains(decision))
            {
                return false;
            }
        }

        return true;
    }
}