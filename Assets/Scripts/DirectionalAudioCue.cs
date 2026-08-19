using UnityEngine;

namespace WhisperingFog
{
    /// <summary>
    /// Sits next to the AudioSource placed down the correct fork of a
    /// CorridorSegment. Unity's normal 3D distance rolloff already makes it
    /// quieter far away, but this adds a second, cheap cue on top: extra
    /// volume when the listener is roughly facing the source, so a player
    /// who stops and turns their head toward a fork gets a noticeably
    /// clearer signal than one who's just walking past it.
    ///
    /// This is the mechanical equivalent of "the melody carries from the
    /// correct exit" in the design this project is inspired by - the
    /// direction of a sound is itself information the player can act on.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class DirectionalAudioCue : MonoBehaviour
    {
        [Tooltip("Usually the player's head/camera transform.")]
        [SerializeField] private Transform _listener;

        [SerializeField, Range(0f, 1f)] private float _baseVolume = 0.35f;
        [SerializeField, Range(0f, 1f)] private float _facingBonusVolume = 0.65f;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.spatialBlend = 1f; // fully 3D positioned
            _audioSource.loop = true;

            if (_listener == null)
            {
                var player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    _listener = player.transform;
                }
            }

            _audioSource.Play();
        }

        private void Update()
        {
            if (_listener == null)
            {
                return;
            }

            var toSource = (transform.position - _listener.position).normalized;
            var facingAmount = Mathf.Clamp01(Vector3.Dot(_listener.forward, toSource));

            _audioSource.volume = _baseVolume + _facingBonusVolume * facingAmount;
        }
    }
}
