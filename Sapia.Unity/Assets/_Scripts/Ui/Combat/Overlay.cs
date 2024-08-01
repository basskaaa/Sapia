using Assets._Scripts.Game;
using Nova;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat
{
    public class Overlay : MonoBehaviour, ICombatListener
    {
        private TextBlock _label;
        private CombatParticipant _lastParticipant;

        void Start()
        {
            _label = GetComponentInChildren<TextBlock>();
            FindFirstObjectByType<CombatRunner>().AddListener(this);

            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show(string text)
        {
            _label.Text = text;
            gameObject.SetActive(true);

            Invoke(nameof(Hide), 1.5f);
        }

        public void StepChanged(Sapia.Game.Combat.Combat combat, CombatStep step)
        {
            if (step is ParticipantStep participantStep)
            {
                var participantChanged = participantStep.Participant != _lastParticipant;

                _lastParticipant = participantStep.Participant;

                if (participantChanged && participantStep is TurnStep)
                {
                    Show($"{TextHelper.Possessive(participantStep.Participant)} turn");
                }
            }
        }
    }
}
