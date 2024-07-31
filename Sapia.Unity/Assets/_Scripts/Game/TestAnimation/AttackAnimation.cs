using UnityEngine;

namespace Assets._Scripts.Game.TestAnimation
{
    public class AttackAnimation : TestAnimation
    {
        public override void Play()
        {
            Rotate(0, 1f, Vector3.right);
        }
    }
}