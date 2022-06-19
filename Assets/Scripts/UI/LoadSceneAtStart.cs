using UnityEngine;

namespace UI
{
    public class LoadSceneAtStart : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private void Start()
        {
            ScenesTransitions.ScenesTransition.Instance.ChangeScene(_sceneName);
        }
    }
}