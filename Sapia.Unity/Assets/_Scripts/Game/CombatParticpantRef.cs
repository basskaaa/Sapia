using System.Linq;
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
                MoveStep(move);
            }

            if (step is AbilityUsedStep abilityUsedStep)
            {
                AbilityStep(abilityUsedStep);
            }
        }

        private void AbilityStep(AbilityUsedStep abilityUsedStep)
        {
            if (abilityUsedStep.Participant.ParticipantId == ParticipantId)
            {
                // An ability was used by this actor. Play the attack animation 
                // Then wait for all others to finish animating
                _anim.PlayAnimation(AnimManager.animName.ability, 0); // needs to reference animation index in ability data
                AudioManager.Instance.PlaySound(AudioManager.Instance.sfxClips[4].AudioClip, 0.5f, AudioManager.Instance.sfxClips[4].LowPitchRange, AudioManager.Instance.sfxClips[4].HighPitchRange);
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
                            _anim.PlayRandomAnimation(AnimManager.animName.hit);

                            AudioManager.Instance.PlaySound(AudioManager.Instance.sfxClips[5].AudioClip, 0.5f, AudioManager.Instance.sfxClips[5].LowPitchRange, AudioManager.Instance.sfxClips[5].HighPitchRange);
                        }
                        else
                        {
                            _anim.PlayRandomAnimation(AnimManager.animName.death);
                        }
                        break;
                    }
                }
            }
        }

        private void MoveStep(MovedStep move)
        {
            // This is a move step for this actor, move to the new position then invoke step
            var initialPosition = transform.position;

            transform.position = new Vector3(move.Participant.Position.X, transform.position.y, move.Participant.Position.Y);

            if (!gameObject.CompareTag("Player"))
            {
                transform.LookAt(FindObjectOfType<Player>().transform);
            }
            else
            {
                var dir = transform.position - initialPosition;

                // Look in front current position in direction of movement
                transform.LookAt(transform.position + dir);
            }
            _anim.PlayRandomAnimation(AnimManager.animName.move);
            Invoke(nameof(Step), .5f);
            //_anim.PlayRandomAnimation(AnimManager.animName.idle);
        }

        private void Step() => _combatRunner.Step();
    }
}