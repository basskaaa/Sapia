using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Game;
using Nova;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class CombatLog : MonoBehaviour, ICombatListener
    {
        public TextBlock Heading;
        public ListView Logs;

        private CombatRunner _combatRunner;

        private readonly List<string> _logs = new();

        void Awake()
        {
            _combatRunner = FindFirstObjectByType<CombatRunner>();
            _combatRunner.AddListener(this);

            Logs.AddDataBinder<string, LogItemVisuals>(BindLogToList);
        }

        private void BindLogToList(Data.OnBind<string> evt, LogItemVisuals target, int index)
        {
            target.Label.Text = evt.UserData;
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            var logs = CreateLog(step).ToArray();

            if (logs.Length > 0)
            {
                foreach (var log in logs)
                {
                    _logs.Insert(0, log);
                }

                Refresh();
            }
        }

        private void Refresh()
        {
            if (_logs.Count > 0)
            {
                Heading.Text = _logs[0];
                Logs.SetDataSource(_logs.Skip(1).ToArray());
            }
        }

        private CombatParticipant _lastParticipant;
        private bool _wasMoveStep = false;

        private IEnumerable<string> CreateLog(CombatStep step)
        {
            if (step is ParticipantStep participantStep)
            {
                var participantChanged = participantStep.Participant != _lastParticipant;

                _lastParticipant = participantStep.Participant;

                if (step is MovedStep && _wasMoveStep == false)
                {
                    _wasMoveStep = true;
                    yield return $"{Name(participantStep.Participant)} {(participantStep.Participant.Character.IsPlayer ? "are" : "is")} moving";
                    yield break;
                }
                else if (step is not MovedStep)
                {
                    _wasMoveStep = false;
                }

                if (participantChanged && participantStep is TurnStep)
                {
                    yield return $"{Possessive(participantStep.Participant)} turn";
                    yield break;
                }

                if (participantStep is AbilityUsedStep abilityStep)
                {
                    var targets = abilityStep.Result.AffectedParticipants
                        .Select(x => TargetOfAbilityResult(step, x))
                        .ToArray();

                    var targetText = string.Join(", ", targets);
                    var fullTargetText = string.IsNullOrWhiteSpace(targetText) ? string.Empty : $" against {targetText}";

                    yield return $"{Name(participantStep.Participant)} used {abilityStep.Result.Ability.Id}{fullTargetText}";

                    foreach (var affectedParticipant in abilityStep.Result.AffectedParticipants)
                    {
                        if (affectedParticipant.HealthChange.HasValue && affectedParticipant.HealthChange.Value < 0)
                        {
                            var participant = step.Combat.Participants[affectedParticipant.ParticipantId];

                            if (!participant.Character.IsAlive)
                            {
                                yield return $"{Name(participant)} died";
                            }
                        }
                    }
                }

                if (participantStep is AbilityFailedStep failed)
                {
                    yield return $"{Name(participantStep.Participant)} failed to use {failed.Use.AbilityId} - {failed.Reason.HumanName()}";
                    yield break;
                }
            }
        }

        private string Name(CombatParticipant participant) => participant.Character.IsPlayer ? "You" : participant.ParticipantId;
        private string Possessive(CombatParticipant participant) => participant.Character.IsPlayer ? "Your" : $"{participant.ParticipantId}'s";

        private string TargetOfAbilityResult(CombatStep step, AffectedParticipant p)
        {
            var participant = step.Combat.Participants[p.ParticipantId];

            var name = Name(participant);

            if (participant.Character.IsPlayer)
            {
                name = name.ToLower();
            }

            if (!participant.Character.IsAlive)
            {

            }

            var dmg = p.HealthChange.HasValue ? $" ({p.HealthChange.Value})" : string.Empty;

            return $"{name}{dmg}";
        }
    }

    public class LogItemVisuals : ItemVisuals
    {
        public TextBlock Label;
    }
}