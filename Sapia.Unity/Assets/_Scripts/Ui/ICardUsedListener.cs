using Assets._Scripts.Game;
using Sapia.Game.Combat.Entities;

namespace Assets._Scripts.Ui
{
    public interface ICardUsedListener
    {
        void OnCardUsed(UsableAbility ability, CombatParticipantRef target);
    }
}