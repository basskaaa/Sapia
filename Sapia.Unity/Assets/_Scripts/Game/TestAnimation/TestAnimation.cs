using System.Collections;
using UnityEngine;

namespace Assets._Scripts.Game.TestAnimation
{
    public abstract class TestAnimation : MonoBehaviour, ITestAnimation
    {
        private float _delay;
        private float _duration;
        private Vector3 _axis;

        protected void Rotate(float delay, float duration, Vector3 axis)
        {
            IsPlaying = true;

            _delay = delay;
            _duration = duration;
            _axis = axis;

            StartCoroutine(nameof(RotateEnumerator));
        }

        IEnumerator RotateEnumerator()
        {
            IsPlaying = true;

            if (_delay > 0)
            {
                yield return new WaitForSeconds(_delay);
            }

            Quaternion startRot = transform.rotation;
            float t = 0.0f;
            while (t < _duration)
            {
                t += Time.deltaTime;
                transform.rotation = startRot * Quaternion.AngleAxis(t / _duration * 360f, _axis); //or transform.right if you want it to be locally based
                yield return null;
            }
            transform.rotation = startRot;
            IsPlaying = false;
        }

        public bool IsPlaying { get; private set; }
        public abstract void Play();
    }
}