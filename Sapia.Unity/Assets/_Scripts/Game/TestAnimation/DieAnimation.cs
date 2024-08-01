using System.Collections;
using UnityEngine;

namespace Assets._Scripts.Game.TestAnimation
{
    public class DieAnimation : MonoBehaviour, ITestAnimation
    {
        public bool IsPlaying { get; private set; }

        private float _duration = 1.5f;

        public void Play()
        {
            IsPlaying = true;
            StartCoroutine(nameof(Fall));
        }

        private IEnumerator Fall()
        {
            IsPlaying = true;
            yield return new WaitForSeconds(0.25f);

            Quaternion startRot = transform.rotation;
            float t = 0.0f;
            while (t < _duration)
            {
                t += Time.deltaTime;
                transform.rotation = startRot * Quaternion.AngleAxis(t / _duration * 90f, transform.right); //or transform.right if you want it to be locally based
                yield return null;
            }

            IsPlaying = false;
        }
    }
}