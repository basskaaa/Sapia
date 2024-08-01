using Assets._Scripts.Game;
using Assets._Scripts.Input;
using Assets._Scripts.Ui.Combat.Cards;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Structs;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat
{
    public class CombatUi : MonoBehaviour, ICombatListener
    {
        private AbilityCardHolder _cards;
        private CombatRunner _combatRunner;
        private UtilityActions _utilityActions;

        public InteractionMode InteractionMode { get; private set; } = InteractionMode.Ready;

        void Awake()
        {
            _cards = GetComponentInChildren<AbilityCardHolder>(true);
            _utilityActions = GetComponentInChildren<UtilityActions>(true);

            _cards.CardUsed.AddListener(CardUsed);

            _combatRunner = FindFirstObjectByType<CombatRunner>();
            _combatRunner.AddListener(this);
        }

        private void CardUsed(UsableAbility ability, CombatParticipantRef target)
        {
            if (target != null)
            {
                _combatRunner.UseAbility("Player", ability, target?.ParticipantId);
            }
        }

        public void StepChanged(Sapia.Game.Combat.Combat combat, CombatStep step)
        {
            if (step is TurnStep turn && turn.Participant.Character.IsPlayer)
            {
                if (InteractionMode != InteractionMode.Ready)
                {
                    ChangeInteractionMode(InteractionMode.Ready);
                }
                _cards.Show(turn.Abilities);

                _utilityActions.Show();
            }
            else
            {
                _utilityActions.Hide();
            }
        }
        
        public void ChangeInteractionMode(InteractionMode switchToInteractionMode)
        {
            InteractionMode = switchToInteractionMode;

            switch (InteractionMode)
            {
                case InteractionMode.Disabled:
                case InteractionMode.Move:
                    _cards.gameObject.SetActive(false);
                    break;

                default:
                case InteractionMode.Ready:
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
            if (GetMousePosition.TryGetCurrentRay(out var ray) && GetMousePosition.TryProjectGroundRay(ray, out var worldPos))
            {
                var coord = TransformWorldToCoord(worldPos);

                if (_combatRunner.Move("Player", coord))
                {
                    ChangeInteractionMode(InteractionMode.Disabled);
                }
            }
        }

        public Coord TransformWorldToCoord(Vector3 worldPos) => new Coord((int)worldPos.x, (int)worldPos.z);

        public Vector3 TransformWorldToCoordVector(Vector3 worldPos)
        {
            var coord = TransformWorldToCoord(worldPos);
            return new Vector3(coord.X, 0, coord.Y);
        }

        public void EndTurn()
        {
            if (_combatRunner.CurrentStep is TurnStep turn && turn.Participant.Character.IsPlayer)
            {
                turn.EndTurn();
                _combatRunner.Step();
            }
        }
    }

    public enum InteractionMode
    {
        Ready,
        Disabled,
        Move
    }
}