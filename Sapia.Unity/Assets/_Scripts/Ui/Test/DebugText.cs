using System;
using UnityEngine;
using Nova;

namespace Assets._Scripts.Ui.Test
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
