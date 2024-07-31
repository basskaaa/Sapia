using Sapia.Game.Combat.Entities;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.Steps;

public class MovedStep : ParticipantStep
{
    public Coord PreviousPosition { get; }

    public MovedStep(Combat combat, CombatParticipant participant, Coord previousPosition) : base(combat, participant)
    {
        PreviousPosition = previousPosition;
    }
}