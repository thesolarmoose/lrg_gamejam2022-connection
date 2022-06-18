using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using Character;
using Controllers;
using UnityEngine;
using UnityEngine.Playables;
using Utils;

namespace EnergySourcesConnections
{
    public class WinGameCinematicController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _targetEnergyConsumer;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private PlayableDirector _optionalCinematic;
        [SerializeField] private string _nextSceneName;

        [SerializeField] private float _waitTimeAfterCameraMove;

        public void NextScene()
        {
            Debug.Log("NextScene");

            var coroutine = CoroutineUtils.CoroutineSequence(new List<IEnumerator>
            {
                CoroutineUtils.ActionCoroutine(DisableAllControllers),
                MoveCameraToTarget(),
                CoroutineUtils.WaitTimeCoroutine(_waitTimeAfterCameraMove),
                PlayCinematic(),
                CoroutineUtils.ActionCoroutine(() =>
                    ScenesTransitions.ScenesTransition.Instance.ChangeScene(_nextSceneName))
            });

            StartCoroutine(coroutine);
        }

        private void DisableAllControllers()
        {
            var components = new List<MonoBehaviour>();
            components.AddRange(FindObjectsOfType<AiBehaviourManager>());
            components.AddRange(FindObjectsOfType<AiBehaviour>());
            components.AddRange(FindObjectsOfType<CharacterInputController>());
            components.AddRange(FindObjectsOfType<CameraFollow>());
            components.ForEach(component => component.enabled = false);
            
            var movements = FindObjectsOfType<CharacterMovement>();
            foreach (var characterMovement in movements)
            {
                characterMovement.Move(Vector2.zero);
            }
        }

        private IEnumerator MoveCameraToTarget()
        {
            var cameraPos = _camera.transform.position;
            var z = cameraPos.z;
            var targetPos = _targetEnergyConsumer.position;
            var dist = Vector2.Distance(cameraPos, targetPos);

            while (dist > 0.1)
            {
                cameraPos = Vector2.Lerp(cameraPos, targetPos, _moveSpeed);
                dist = Vector2.Distance(cameraPos, targetPos);
                cameraPos.z = z;
                _camera.transform.position = cameraPos;

                yield return null;
            }
        }

        private IEnumerator PlayCinematic()
        {
            if (_optionalCinematic != null)
            {
                _optionalCinematic.Play();
                while (_optionalCinematic.state == PlayState.Playing)
                {
                    yield return null;
                }
            }
        }
    }
}