using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Game;
using Nova;
using Sapia.Game.Combat;
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

        private string CreateLog(CombatStep step)
        {
            return step.ToString();
        }
    }

    public class LogItemVisuals : ItemVisuals
    {
        public TextBlock Label;
    }
}