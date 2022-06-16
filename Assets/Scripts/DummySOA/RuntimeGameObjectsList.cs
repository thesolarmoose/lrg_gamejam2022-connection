using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DummySOA
{
    [CreateAssetMenu(fileName = "RuntimeGameObjectsList", menuName = "RuntimeGameObjectsList", order = 0)]
    public class RuntimeGameObjectsList : ScriptableObject, IList<GameObject>
    {
        [SerializeField] private List<GameObject> _objects;
        
        public IEnumerator<GameObject> GetEnumerator()
        {
            return _objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }

        public void Add(GameObject item)
        {
            _objects.Add(item);
        }

        public void Clear()
        {
            _objects.Clear();
        }

        public bool Contains(GameObject item)
        {
            return _objects.Contains(item);
        }

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            _objects.CopyTo(array, arrayIndex);
        }

        public bool Remove(GameObject item)
        {
            return _objects.Remove(item);
        }

        public int Count => _objects.Count;

        public bool IsReadOnly => ((ICollection<GameObject>) this).IsReadOnly;

        public int IndexOf(GameObject item)
        {
            return _objects.IndexOf(item);
        }

        public void Insert(int index, GameObject item)
        {
            _objects.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _objects.RemoveAt(index);
        }

        public GameObject this[int index]
        {
            get => _objects[index];
            set => _objects[index] = value;
        }
    }
}