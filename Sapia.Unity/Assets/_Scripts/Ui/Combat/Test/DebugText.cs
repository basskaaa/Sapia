using System;
using Nova;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Test
{
    public class DebugText : MonoBehaviour
    {
        private TextBlock _text;

        void Awake()
        {
            _text = GetComponent<TextBlock>();
        }

        public void Show(params string[] text)
        {
            _text.Text = string.Join(Environment.NewLine, text);
        }
    }
}
