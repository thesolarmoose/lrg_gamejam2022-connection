using Character;
using Skills;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Controllers
{
    public class CharacterInputController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _character;
        [SerializeField] private BaseDirectionalSkill _shootSkill;
        [SerializeField] private GunLooker _gun;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private float _gamepadCursorDist;
        
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _lookAction;
        [SerializeField] private InputActionReference _pointAction;
        [SerializeField] private InputActionReference _shootAction;

        private InputDevice _lastDevice;
        
        private void Start()
        {
            _moveAction.action.Enable();
            _pointAction.action.Enable();
            _lookAction.action.Enable();
            _shootAction.action.Enable();
            InputUser.onChange += OnInputDeviceChanged;
            _shootAction.action.performed += OnShoot;
        }

        private void OnEnable()
        {
            _moveAction.action?.Enable();
            _pointAction.action?.Enable();
            _lookAction.action?.Enable();
            _shootAction.action?.Enable();
        }

        private void OnDisable()
        {
            _moveAction.action?.Disable();
            _pointAction.action?.Disable();
            _lookAction.action?.Disable();
            _shootAction.action.performed -= OnShoot;
            _shootAction.action?.Disable();
        }

        private void OnInputDeviceChanged(InputUser user, InputUserChange userChange, InputDevice device)
        {
            if (userChange == InputUserChange.DevicePaired)
            {
                _lastDevice = device;
            }
        }

        private void OnShoot(InputAction.CallbackContext ctx)
        {
            _shootSkill.Use(_gun.CurrentLookDirection);
        }

        private void Update()
        {
            var charPos = _character.transform.position;
            
            // move dir
            var moveDir = _moveAction.action.ReadValue<Vector2>();

            // look dir
            Vector2 lookDir;
            bool hideCursor = false;
            if (CurrentDeviceIsGamepad())
            {
                lookDir = _lookAction.action.ReadValue<Vector2>();
                var cursorOffset = lookDir * _gamepadCursorDist;
                var cursorPosition = charPos + (Vector3) cursorOffset;
                var cursorScreenPosition = _camera.WorldToScreenPoint(cursorPosition);
                if (lookDir.sqrMagnitude < 0.01f)
                    hideCursor = true;
                Mouse.current.WarpCursorPosition(cursorScreenPosition);
            }
            else
            {
                var pointerScreenPos = _pointAction.action.ReadValue<Vector2>();
                var pointerWorldPos = _camera.ScreenToWorldPoint(pointerScreenPos);
                lookDir = pointerWorldPos - charPos;
            }
            
            _character.Move(moveDir);
            _character.Look(lookDir);
            Cursor.visible = !hideCursor;
        }

        private bool CurrentDeviceIsGamepad()
        {
            return Gamepad.current != null && Gamepad.current == _lastDevice;
        }
    }
}