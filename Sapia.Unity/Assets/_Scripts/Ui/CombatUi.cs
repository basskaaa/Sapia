using Assets._Scripts.Game;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class CombatUi : MonoBehaviour, ICombatListener
    {
        private AbilityCardHolder _cards;
        private TopCardSetter _topCardSetter;
        private CombatRunner _combatRunner;

        void Awake()
        {
            _cards = GetComponentInChildren<AbilityCardHolder>();
            _topCardSetter = GetComponentInChildren<TopCardSetter>();

            _cards.CardUsed.AddListener(CardUsed);

            _combatRunner = FindFirstObjectByType<CombatRunner>();
            _combatRunner.AddListener(this);
        }

        private void CardUsed(UsableAbility ability, CombatParticipantRef target)
        {
            UnityEngine.Debug.Log($"Trying {ability.AbilityType.Id}");

            _combatRunner.UseAbility("Player", ability, target?.ParticipantId);
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            if (step is TurnStep turn && turn.Participant.Character.IsPlayer)
            {
                _cards.Show(turn.Abilities);
                _topCardSetter.FindTopCard();
            }
        }
    }
}