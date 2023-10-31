using UnityEngine;
using UnityEngine.InputSystem;

namespace SolarAscension {
    [RequireComponent(typeof(Camera))]
    public class OrbitalCamera : MonoBehaviour {
        [Header("Follow Settings")]
        [SerializeField] private Transform focusPoint;
        [SerializeField, Range(0.1f, 1f)] private float moveSmoothing = 0.4f;

        [Header("Default Settings")]
        [SerializeField] private float defaultDistance = 200f;
        [SerializeField] private Vector2 defaultAngles = Vector2.zero;

        [Header("Rotation Settings")]
        [SerializeField, Range(1f, 360f)] private float rotationSpeed = 90f;
        [SerializeField, Range(0.1f, 1f)] private float smoothingSpeed = 0.1f;
        [SerializeField, Range(0.1f, 2f)] private float horizontalSensitivity = 1f, verticalSensitivity = 1f;
        [SerializeField, Range(0.1f, 2f)] private float rotationSensitivity = 1f;
        [SerializeField, Range(0.1f, 2f)] private float mouseHorizontalSensitivity = 1f, mouseVerticalSensitivity = 1f;
        [SerializeField] private bool invertX, invertY, invertZ;

        [Header("Zoom Settings")]
        [SerializeField, Range(0.1f, 20f)] private float zoomSpeed = 1f;
        [SerializeField, Range(0.0f, 0.2f)] private float zoomSmoothing = 0.1f;
        [SerializeField, Range(40f, 300f)] private float minimumZoomDistance = 40f;
        [SerializeField, Range(300f, 1000f)] private float maximumZoomDistance = 1000f;

        private bool _isSetNextFocusPoint;
        private bool _useMouseRotation;
        private Vector2 _moveRotation;
        
        private Input.CameraActions _input;
        private PlayerStateData _playerStateData;

        private Vector2 Inversion => new(invertY ? -1 : 1, invertX ? -1 : 1);
        private Vector2 Sensitivity => new(verticalSensitivity, horizontalSensitivity);
        private Vector2 MouseSensitivity => new(mouseVerticalSensitivity, mouseHorizontalSensitivity);

        public GridCoordinate focusPointCoordinate;
        private Quaternion _defaultRotation, _horizontalRotation, _verticalRotation, _cameraRotation;
        private Vector3 _focusPoint, _moveVelocity, _desiredFocusPoint;
        private Vector2 _orbitAngles, _orbitInput;
        private float _localRotation, _currentDistance, _desiredDistance, _zoomVelocity, _rotationInput;
        private InputAxisConverter _inputToAxis;

        public void SetInitialFocusTransform(Transform focus) => focusPoint = focus;
        
        public void SetUp(Player player) {
            _playerStateData = player.State.StateData;
            _input = new Input().Camera;
            _input.Enable();

            GameplaySettings.OnCameraSettingsUpdate += UpdateCameraSettings;
            UpdateCameraSettings();

            _inputToAxis = new InputAxisConverter(smoothingSpeed);
            _orbitAngles = defaultAngles;
            _currentDistance = _desiredDistance = defaultDistance;
            _focusPoint = _desiredFocusPoint = focusPoint.position;
            _horizontalRotation = Quaternion.Euler(new Vector3(0f, _orbitAngles.y, 0f));
            _verticalRotation = Quaternion.Euler(new Vector3(_orbitAngles.x, 0f, 0f));
            transform.localRotation = _defaultRotation = _horizontalRotation * _verticalRotation;
            _cameraRotation = Quaternion.identity;
        }

        private void OnValidate() {
            if (focusPoint == null) {
                return;
            }
            Quaternion lookRotation = Quaternion.Euler(defaultAngles);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = focusPoint.position - lookDirection * defaultDistance;
            
            Transform cameraTransform = transform;
            cameraTransform.localRotation = lookRotation;
            cameraTransform.localPosition = lookPosition;
        }

        private void Update() {
            if (MenuManager.Instance.paused) {
                return;
            }

            if ( _input.MiddleMouseButton.WasPerformedThisFrame() ) {
                _useMouseRotation = true;
            }

            if ( _input.MiddleMouseButton.WasReleasedThisFrame() ) {
                _useMouseRotation = false;
            }
            
            if (_useMouseRotation) {
                _moveRotation = _input.MouseMove.ReadValue<Vector2>();
            } else {
                Vector2 input = Keyboard.current.shiftKey.isPressed ? Vector2.zero : _input.Move.ReadValue<Vector2>();
                Vector2 orbitInput = _inputToAxis.InputToAxis(input);
                _orbitInput = Vector2.ClampMagnitude(orbitInput, 1f);
                _rotationInput = _input.Rotate.ReadValue<float>() * (invertZ ? -1 : 1);
            }
            
            float zoom = _input.Zoom.ReadValue<float>() / 120f * -1f;
            _desiredDistance += zoom * zoomSpeed;
            _desiredDistance = Mathf.Clamp(_desiredDistance, minimumZoomDistance, maximumZoomDistance);

            if ( _input.SetFocus.WasPerformedThisFrame() ) {
                SetFocusPointHover();
            }

            if ( _input.ResetCamera.WasPerformedThisFrame() ) {
                transform.localRotation = _defaultRotation;
            }
        }

        public void ResetCamera() {
            Vector3 centerPoint = _playerStateData.startingGridBehaviour.GetLinkCenter();
            transform.localPosition = _desiredFocusPoint = _focusPoint =  centerPoint;
            transform.localRotation = _defaultRotation;
            AudioManager.Instance.Play("select");
        }

        public void SetFocusPointSelected() {
            if ( _playerStateData.selectedGridBehaviour != null ) {
                focusPointCoordinate = _playerStateData.selectedGridBehaviour.GetLinkCenter();
                _desiredFocusPoint = focusPointCoordinate.ToWorldPositionCentered();
                _playerStateData.SetPlaneCoordinate(_playerStateData.selectedGridBehaviour.GetLinkCenter());
                AudioManager.Instance.Play("select");
            } else {
                AudioManager.Instance.Play("fail");
            }
        }

        private void SetFocusPointHover() {
            if ( _playerStateData.hoveredGridBehaviour != null ) {
                focusPointCoordinate = _playerStateData.hoveredGridBehaviour.GetLinkCenter();
                _desiredFocusPoint = focusPointCoordinate.ToWorldPositionCentered();
                _playerStateData.SetPlaneCoordinate(_playerStateData.hoveredGridBehaviour.GetLinkCenter());
                AudioManager.Instance.Play("select");
            } else {
                AudioManager.Instance.Play("fail");
            }
        }

        public void SetFocusPointFromCoordinate(GridCoordinate c) {
            _desiredFocusPoint = c.ToWorldPositionCentered();
            focusPointCoordinate = c;
        }

        private void LateUpdate() {
            if (MenuManager.Instance.paused) {
                return;
            }

            if (_useMouseRotation) {
                _orbitAngles = rotationSpeed * Time.unscaledDeltaTime * _moveRotation * MouseSensitivity * Inversion;
            }
            else {
                _orbitAngles = rotationSpeed * Time.unscaledDeltaTime * _orbitInput * Sensitivity * Inversion;
                _localRotation = rotationSpeed * Time.unscaledDeltaTime * _rotationInput * rotationSensitivity;
            }
            
            AdjustFocusPoint();
            AdjustRotation();
            AdjustZoom();

            Transform cameraTransform = transform;

            Quaternion lookRotation = cameraTransform.localRotation * _horizontalRotation * _verticalRotation * _cameraRotation;
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _currentDistance;

            cameraTransform.localRotation = lookRotation;
            cameraTransform.localPosition = lookPosition;
        }

        private void AdjustFocusPoint() {
            Vector3 targetPoint = _desiredFocusPoint;
            float distance = Vector3.Distance(targetPoint, _focusPoint);
            if (distance > 0.01f) {
                _focusPoint = Vector3.SmoothDamp(_focusPoint, targetPoint, ref _moveVelocity, moveSmoothing);
            }
            else {
                _focusPoint = targetPoint;
            }
        }

        private void AdjustRotation() {
            _horizontalRotation = Quaternion.AngleAxis(_orbitAngles.y, Vector3.up);
            _verticalRotation = Quaternion.AngleAxis(_orbitAngles.x, Vector3.right);
            _cameraRotation = Quaternion.AngleAxis(_localRotation, Vector3.forward);
        }

        private void AdjustZoom() {
            if (Mathf.Abs(_currentDistance - _desiredDistance) > 0.001f) {
                _currentDistance = Mathf.SmoothDamp(_currentDistance, _desiredDistance, ref _zoomVelocity, zoomSmoothing);
            }
            else {
                _currentDistance = _desiredDistance;
            }
        }

        private void UpdateCameraSettings() {
            horizontalSensitivity = GameplaySettings.HorizontalSensitivity;
            verticalSensitivity = GameplaySettings.VerticalSensitivity;
            mouseHorizontalSensitivity = GameplaySettings.MouseHorizontalSensitivity;
            mouseVerticalSensitivity = GameplaySettings.MouseVerticalSensitivity;
            rotationSensitivity = GameplaySettings.RotationSensitivity;
        }

        private void OnDrawGizmos() {
            if (focusPoint != null) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(focusPoint.position, 1f);
                Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
                Gizmos.DrawSphere(focusPoint.position, _currentDistance);
                if (Mathf.Abs(_currentDistance - _desiredDistance) > 0.01f) {
                    Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                    Gizmos.DrawSphere(focusPoint.position, _desiredDistance);
                }
            }
        }
    }
}
