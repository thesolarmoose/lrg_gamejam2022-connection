using UnityEngine;

namespace Hook
{
    public class RopeTip : MonoBehaviour
    {
        [field:SerializeField] public float Weight { get; set; }

        public Vector2 position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
    }
}