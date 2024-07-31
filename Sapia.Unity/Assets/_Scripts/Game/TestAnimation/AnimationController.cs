using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Scripts.Game.TestAnimation
{
    public class AnimationController : MonoBehaviour
    {
        private AttackAnimation _attack;
        private HitAnimation _hit;
        private DieAnimation _die;

        public bool IsPlaying => (_attack?.IsPlaying ?? false) || (_hit?.IsPlaying ?? false);

        void Start()
        {
            _hit = gameObject.GetOrAddComponent<HitAnimation>();
            _attack = gameObject.GetOrAddComponent<AttackAnimation>();
            _die = gameObject.GetOrAddComponent<DieAnimation>();
        }

        public void Attack() => _attack.Play();
        public void Hit() => _hit.Play();
        public void Die() => _die.Play();
    }
}