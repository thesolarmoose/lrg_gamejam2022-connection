using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreditsMenu : MonoBehaviour
    {
        [SerializeField] private Button _quitButton;

        private void Start()
        {
            _quitButton.onClick.AddListener(Quit);
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}