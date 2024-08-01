using System;
using Nova;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Test
{
    public class DebugText : MonoBehaviour
    {
        public TextBlock text, buttonLabel;

        void Awake()
        {
            Hide();
        }

        private void Hide()
        {
            text.gameObject.SetActive(false);
            buttonLabel.Text = "+";
        }

        private void Show()
        {
            text.gameObject.SetActive(true);
            buttonLabel.Text = "-";
        }

        public void Toggle()
        {
            if (text.gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void ShowText(params string[] text)
        {
            this.text.Text = string.Join(Environment.NewLine, text);
        }
    }
}
