using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Utils;

namespace ScenesTransitions
{
    [CreateAssetMenu(fileName = "ScenesTransition", menuName = "ScenesTransition", order = 0)]
    public class ScenesTransition : ScriptableObjectSingleton<ScenesTransition>
    {
        [SerializeField] private PlayableDirector _fadeInCinematicPrefab;
        [SerializeField] private PlayableDirector _fadeOutCinematicPrefab;

        public async void ChangeScene(string sceneName)
        {
            await AwaitCinematic(_fadeInCinematicPrefab);
            
            SceneManager.LoadScene(sceneName);
            
            await AwaitCinematic(_fadeOutCinematicPrefab);
        }

        private async Task AwaitCinematic(PlayableDirector directorPrefab)
        {
            var fadeCinematic = Instantiate(directorPrefab);
            fadeCinematic.Play();

            while (fadeCinematic.state == PlayState.Playing)
            {
                await Task.Yield();
            }
            Destroy(fadeCinematic);
        }
        
        private async Task AwaitAsync(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                await Task.Yield();
            }
        }
    }
}