using UnityEngine;

namespace Assets._Scripts.Anim
{
    public class AnimManager : MonoBehaviour
    {
        private Animator _animator;

        [SerializeField] private AnimationClip _idle;
        [SerializeField] private AnimationClip _move;
        [SerializeField] private AnimationClip _death;
        [SerializeField] private AnimationClip _hit;
        [SerializeField] private AnimationClip[] _ability;

        public enum animName
        {
            idle,
            move,
            death,
            hit,
            ability
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnim(animName anim, int index)
        {
            switch (anim)
            {
                case animName.idle:
                    _animator.Play(_idle.name);
                    break;
                case animName.move:
                    _animator.Play(_move.name);
                    PlayAnim(animName.idle, 0);
                    break;
                case animName.death:
                    _animator.Play(_death.name);
                    PlayAnim(animName.idle, 0);
                    break;
                case animName.hit:
                    _animator.Play(_hit.name);
                    PlayAnim(animName.idle, 0);
                    break;
                case animName.ability:
                    _animator.Play(_ability[index].name);
                    PlayAnim(animName.idle, 0);
                    break;
                default:
                    return;
            }
        }

        public bool IsPlaying()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return false;
            }
            else
            {
                return _animator.GetCurrentAnimatorStateInfo(0).length >
                       _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }

        public void StopAnim()
        {
            _animator.StopPlayback();
        }
    }
}
