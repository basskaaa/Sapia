using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;

namespace Assets._Scripts.Ui
{
    public static class TextHelper
    {
        public static string Name(CombatParticipant participant) => participant.Character.IsPlayer ? "You" : participant.ParticipantId;
        public static string Possessive(CombatParticipant participant) => participant.Character.IsPlayer ? "Your" : $"{participant.ParticipantId}'s";

        public static string TargetOfAbilityResult(CombatStep step, AffectedParticipant p)
        {
            var participant = step.Combat.Participants[p.ParticipantId];

            var name = Name(participant);

            if (participant.Character.IsPlayer)
            {
                name = name.ToLower();
            }

            var dmg = p.HealthChange.HasValue ? $" ({p.HealthChange.Value})" : string.Empty;

            return $"{name}{dmg}";
        }
    }
}