using Assets._Scripts.Game;
using Assets._Scripts.Input;
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
        private UtilityActions _utilityActions;

        public InteractionMode InteractionMode { get; private set; }= InteractionMode.None;

        void Awake()
        {
            _cards = GetComponentInChildren<AbilityCardHolder>(true);
            _topCardSetter = GetComponentInChildren<TopCardSetter>(true);
            _utilityActions = GetComponentInChildren<UtilityActions>(true);

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
                GetCardsPos();

                _utilityActions.Show();
            }
            else
            {
                _utilityActions.Hide();
            }
        }

        private void GetCardsPos()
        {
            CardSelect[] _cardSelects = _cards.GetComponentsInChildren<CardSelect>();

            foreach (CardSelect cardSelect in _cardSelects)
            {
                cardSelect.GetInitPosData();
            }
        }

        public void ChangeInteractionMode(InteractionMode switchToInteractionMode)
        {
            InteractionMode = switchToInteractionMode;

            switch (InteractionMode)
            {
                case InteractionMode.Move:
                    _cards.gameObject.SetActive(false);
                    break;

                default:
                case InteractionMode.None:
                    _cards.gameObject.SetActive(true);
                    break;
            }
        }

        void Update()
        {
            if (InteractionMode == InteractionMode.Move && UnityEngine.Input.GetMouseButtonUp(0))
            {
                TryMove();
            }
        }

        private void TryMove()
        {
            if (GetMousePosition.Instance.TryGetCurrentRay(out var ray) && GetMousePosition.Instance.TryProjectGroundRay(ray, out var worldPos))
            {
                UnityEngine.Debug.Log($"Clicked at {worldPos}");

                _combatRunner.Move("Player", worldPos);
            }
        }
    }

    public enum InteractionMode
    {
        None,
        Move
    }
}