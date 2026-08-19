using UnityEngine;

namespace WhisperingFog
{
    /// <summary>
    /// Minimal first-person walk + mouse-look. Not networked - this is a
    /// single-player exploration puzzle, so there's nothing to sync.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera _headCamera;
        [SerializeField] private float _moveSpeed = 3.2f;
        [SerializeField] private float _lookSensitivity = 2f;

        private CharacterController _controller;
        private float _pitch;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Look();
            Move();
        }

        private void Look()
        {
            var mouseX = Input.GetAxis("Mouse X") * _lookSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * _lookSensitivity;

            transform.Rotate(Vector3.up, mouseX);

            _pitch = Mathf.Clamp(_pitch - mouseY, -80f, 80f);
            if (_headCamera != null)
            {
                _headCamera.transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
            }
        }

        private void Move()
        {
            var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            var motion = transform.TransformDirection(input) * _moveSpeed;
            _controller.Move((motion + Physics.gravity) * Time.deltaTime);
        }

        /// <summary>Used by CorridorSegment to snap the player back without physics fighting it.</summary>
        public void Teleport(Vector3 position, Quaternion rotation)
        {
            _controller.enabled = false;
            transform.SetPositionAndRotation(position, rotation);
            _controller.enabled = true;
        }
    }
}
