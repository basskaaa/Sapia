using System.Linq;
using Assets._Scripts.Game.TestAnimation;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class CombatParticipantRef : MonoBehaviour, ICombatListener
    {
        public string ParticipantId;
        public CombatParticipant Participant { get; private set; }

        private AnimationController _animationController;
        private Combat _combat;
        private CombatParticipantRef[] _others;
        private bool _isResponsibleForNextStep = false;

        void Awake()
        {
            _animationController = gameObject.GetOrAddComponent<AnimationController>();

            _others = FindObjectsByType<CombatParticipantRef>(FindObjectsInactive.Include FindObjectsSortMode.InstanceID)
                .Where(x => x != this)
                .ToArray();
        }

        void Update()
        {
            if (_isResponsibleForNextStep)
            {
                foreach (var other in _others)
                {
                    if (other._animationController.IsPlaying)
                    {
                        return;
                    }
                }

                _isResponsibleForNextStep = false;
                Step();
            }
        }

        public void JoinCombat(CombatRunner combatRunner, Combat combat)
        {
            _combat = combat;
            combatRunner.AddListener(this);

            Participant = combat.Participants[ParticipantId];
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            if (step is MovedStep move && move.Participant.ParticipantId == ParticipantId)
            {
                transform.position = new Vector3(move.Participant.Position.X, transform.position.y, move.Participant.Position.Y);
                Invoke(nameof(Step), .35f);
            }

            if (step is AbilityUsedStep abilityUsedStep)
            {
                if (abilityUsedStep.Participant.ParticipantId == ParticipantId)
                {
                    _animationController.Attack();
                    _isResponsibleForNextStep = true;
                }
                else
                {
                    foreach (var affectedParticipant in abilityUsedStep.Result.AffectedParticipants)
                    {
                        if (affectedParticipant.ParticipantId == ParticipantId)
                        {
                            if (Participant.Character.IsAlive)
                            {
                                _animationController.Hit();
                            }
                            else
                            {
                                _animationController.Die();
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void Step()
        {
            _combat.Step();
        }
    }
}