using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;

        [SerializeField] private InputActionReference _pauseAction;

        private bool _paused;
        
        private void Start()
        {
            _resumeButton.onClick.AddListener(Resume);
            _quitButton.onClick.AddListener(Quit);
            _pauseAction.action.Enable();
            _pauseAction.action.performed += ctx => Toggle();
            
            Resume();
        }

        private void OnEnable()
        {
            _pauseAction.action?.Enable();
        }

        private void OnDisable()
        {
            _pauseAction.action?.Disable();
        }

        private void Pause()
        {
            _paused = true;
            Time.timeScale = 0;
            _container.SetActive(true);
        }

        private void Resume()
        {
            _paused = false;
            Time.timeScale = 1;
            _container.SetActive(false);
        }

        private void Toggle()
        {
            if (_paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}