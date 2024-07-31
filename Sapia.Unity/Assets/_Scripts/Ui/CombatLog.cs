using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Game;
using Nova;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class CombatLog : MonoBehaviour, ICombatListener
    {
        public TextBlock Heading;
        public ListView Logs;

        private readonly List<string> _logs = new();

        void Start()
        {
            FindFirstObjectByType<CombatRunner>().AddListener(this);

            Logs.AddDataBinder<string, LogItemVisuals>(BindLogToList);
        }

        private void BindLogToList(Data.OnBind<string> evt, LogItemVisuals target, int index)
        {
            target.Label.Text = evt.UserData;
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            var log = CreateLog(step);

            if (!string.IsNullOrWhiteSpace(log))
            {
                _logs.Insert(0, log);

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

        private string CreateLog(CombatStep step)
        {
            if (step is ParticipantStep participantStep)
            {
                var participantChanged = participantStep.Participant != _lastParticipant;

                _lastParticipant = participantStep.Participant;

                if (step is MovedStep && _wasMoveStep == false)
                {
                    _wasMoveStep = true;
                    return $"{Name(participantStep.Participant)} is moving";
                }
                else
                {
                    _wasMoveStep = false;
                }

                if (participantChanged && participantStep is TurnStep)
                {
                    return $"{Possessive(participantStep.Participant)} turn";
                }

                if (participantStep is AbilityUsedStep abilityStep)
                {
                    return $"{Name(participantStep.Participant)} used {abilityStep.Result.Ability.Id}";
                }
            }

            return null;
        }

        private string Name(CombatParticipant participant) => participant.Character.IsPlayer ? "You" : participant.ParticipantId;
        private string Possessive(CombatParticipant participant) => participant.Character.IsPlayer ? "Your" : $"{participant.ParticipantId}'s";
    }

    public class LogItemVisuals : ItemVisuals
    {
        public TextBlock Label;
    }
}