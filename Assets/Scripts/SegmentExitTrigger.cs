using UnityEngine;

namespace WhisperingFog
{
    /// <summary>
    /// Drop this on each of a CorridorSegment's two fork-end trigger
    /// colliders (Collider.isTrigger must be on). Set _isCorrectPath to
    /// true on exactly one of the two per segment.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class SegmentExitTrigger : MonoBehaviour
    {
        [SerializeField] private CorridorSegment _segment;
        [SerializeField] private bool _isCorrectPath;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerController>();
            if (player == null || _segment == null)
            {
                return;
            }

            _segment.HandleExit(_isCorrectPath, player);
        }
    }
}
