using UnityEngine;

namespace Assets._Scripts.ObjectTags
{
    public class FollowPlayer : MonoBehaviour
    {
        private Player _target;
        private Vector3 _offset;

        // Start is called before the first frame update
        void Start()
        {
            _target = FindFirstObjectByType<Player>();
            _offset = transform.position - _target.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _target.transform.position + _offset;
        }
    }
}
