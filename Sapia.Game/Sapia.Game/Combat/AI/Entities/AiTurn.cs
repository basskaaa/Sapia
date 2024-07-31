namespace Sapia.Game.Combat.AI.Entities;

public struct AiTurn
{
    public HashSet<DecisionAttempts> Decisions = new();

    public AiTurn()
    {
    }
}