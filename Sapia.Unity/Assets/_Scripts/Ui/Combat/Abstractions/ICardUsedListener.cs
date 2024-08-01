using Assets._Scripts.Game;
using Sapia.Game.Combat.Entities;

namespace Assets._Scripts.Ui.Combat.Abstractions
{
    public interface ICardUsedListener
    {
        void OnCardUsed(UsableAbility ability, CombatParticipantRef target);
    }
}