using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class CombatParticipantRef : MonoBehaviour, ICombatListener
    {
        public string ParticipantId;
        public CombatParticipant Participant { get; private set; }

        public void JoinCombat(CombatRunner combatRunner, Combat combat)
        {
            combatRunner.AddListener(this);

            Participant = combat.Participants[ParticipantId];
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            if (step is MovedStep move && move.Participant.ParticipantId == ParticipantId)
            {
                transform.position = new Vector3(move.Participant.Position.X, transform.position.y, move.Participant.Position.Y);
            }
        }
    }
}