using System.Collections;
using Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterHealthUi : MonoBehaviour
    {
        [SerializeField] private Health characterHealth;
        
        [Space]
        
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private GameObject heartlessPrefab;
        [SerializeField] private RectTransform heartsContainer;
        
        private IEnumerator Start()
        {
            characterHealth.eventDamaged.AddListener(_ => UpdateUi());
            
            yield return null;
            
            UpdateUi();
        }

        private void UpdateUi()
        {
            Clear();
            int health = characterHealth.CurrentHealth;

            for (int i = 0; i < health; i++)
            {
                AddHeart();
            }

            int toMax = characterHealth.MaxHealth - health;
            for (int i = 0; i < toMax; i++)
            {
                AddEmptyHeart();
            }
        }

        private void AddHeart()
        {
            Instantiate(heartPrefab, heartsContainer);
        }
        
        private void AddEmptyHeart()
        {
            Instantiate(heartlessPrefab, heartsContainer);
        }

        private void Clear()
        {
            var children = heartsContainer.GetComponentsInChildren<Image>();
            foreach (var item in children)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
