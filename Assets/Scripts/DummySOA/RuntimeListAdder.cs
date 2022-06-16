using UnityEngine;

namespace DummySOA
{
    public class RuntimeListAdder : MonoBehaviour
    {
        [SerializeField] private RuntimeGameObjectsList _list;

        private void Start()
        {
            Add();
        }

        private void OnEnable()
        {
            Add();
        }

        private void OnDisable()
        {
            Remove();
        }

        private void Add()
        {
            if (!_list.Contains(gameObject))
            {
                _list.Add(gameObject);
            }
        }

        private void Remove()
        {
            if (_list.Contains(gameObject))
            {
                _list.Remove(gameObject);
            }
        }
    }
}