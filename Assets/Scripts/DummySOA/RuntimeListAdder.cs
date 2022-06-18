using System.Collections.Generic;
using UnityEngine;

namespace DummySOA
{
    public class RuntimeListAdder : MonoBehaviour
    {
        [SerializeField] private List<RuntimeGameObjectsList> _list;

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
            foreach (var list in _list)
            {
                if (!list.Contains(gameObject))
                {
                    list.Add(gameObject);
                }
            }
        }

        private void Remove()
        {
            foreach (var list in _list)
            {
                if (list.Contains(gameObject))
                {
                    list.Remove(gameObject);
                }
            }
        }
    }
}