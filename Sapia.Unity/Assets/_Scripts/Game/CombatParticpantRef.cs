using System.Linq;
using Assets._Scripts.Anim;
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

        private AnimManager _anim;
        private CombatParticipantRef[] _others;
        private bool _isResponsibleForNextStep = false;
        private CombatRunner _combatRunner;

        void Awake()
        {
            _anim = gameObject.GetOrAddComponent<AnimManager>();

            _others = FindObjectsByType<CombatParticipantRef>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
                .Where(x => x != this)
                .ToArray();
        }

        void Update()
        {
            // This actor is responsible for advancing the step
            // This is usually because an ability has been used
            // Wait until all the other actors have stopped animating
            if (_isResponsibleForNextStep)
            {
                foreach (var other in _others)
                {
                    if (other._anim.IsPlaying())
                    {
                        return;
                    }
                }

                // No one is animating - step
                _isResponsibleForNextStep = false;
                Step();
            }
        }

        public void JoinCombat(CombatRunner combatRunner, Combat combat)
        {
            _combatRunner = combatRunner;

            _combatRunner.AddListener(this);

            Participant = combat.Participants[ParticipantId];
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            if (step is MovedStep move && move.Participant.ParticipantId == ParticipantId)
            {
                // This is a move step for this actor, move to the new position then invoke step
                transform.position = new Vector3(move.Participant.Position.X, transform.position.y, move.Participant.Position.Y);
                Invoke(nameof(Step), .5f);
            }

            if (step is AbilityUsedStep abilityUsedStep)
            {
                if (abilityUsedStep.Participant.ParticipantId == ParticipantId)
                {
                    // An ability was used by this actor. Play the attack animation t
                    // Then wait for all others to finish animating
                    _anim.PlayAnim(AnimManager.animName.ability, 0);
                    _isResponsibleForNextStep = true;
                }
                else
                {
                    // Check if anyone affected by the ability was this actor
                    foreach (var affectedParticipant in abilityUsedStep.Result.AffectedParticipants)
                    {
                        // Play appropriate animation if this actor was effected
                        if (affectedParticipant.ParticipantId == ParticipantId)
                        {
                            if (Participant.Character.IsAlive)
                            {
                                _anim.PlayAnim(AnimManager.animName.hit, 0);
                            }
                            else
                            {
                                _anim.PlayAnim(AnimManager.animName.death, 0);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void Step() => _combatRunner.Step();
    }
}