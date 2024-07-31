using UnityEngine;

namespace Assets._Scripts.Game.TestAnimation
{
    public class HitAnimation : TestAnimation
    {
        public override void Play()
        {
            Rotate(.25f, .5f, Vector3.up);
        }
    }
}