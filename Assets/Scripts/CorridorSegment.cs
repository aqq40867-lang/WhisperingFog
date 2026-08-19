using UnityEngine;
using UnityEngine.Events;

namespace WhisperingFog
{
    /// <summary>
    /// One repeatable "you might already be lost" stretch of fog-bound
    /// forest. Every segment in the scene reuses the same prefab/geometry on
    /// purpose - the whole trick only works if the wrong fork looks
    /// identical to walking straight ahead.
    ///
    /// Layout expectation: a short two-fork stretch (a T or Y junction).
    /// Each fork ends in a trigger collider carrying a <see cref="SegmentExitTrigger"/>
    /// - one marked correct, one marked wrong.
    ///
    /// The wrong fork silently snaps the player back to <see cref="_resetPoint"/>:
    /// no fade, no sound sting, nothing. Because the geometry repeats, that
    /// teleport is indistinguishable from having just kept walking - the
    /// player only escapes by noticing which fork the music is coming from
    /// (see DirectionalAudioCue), not by noticing they looped.
    /// </summary>
    public class CorridorSegment : MonoBehaviour
    {
        [Tooltip("Where the player is snapped back to when they pick the wrong fork.")]
        [SerializeField] private Transform _resetPoint;

        [Tooltip("Raised once, the first time the player reaches the correct fork.")]
        public UnityEvent OnCorrectExit;

        public bool IsCompleted { get; private set; }

        public void HandleExit(bool isCorrectFork, PlayerController player)
        {
            if (IsCompleted)
            {
                // Already solved once - let the player walk through freely
                // rather than re-triggering the reset on a return visit.
                return;
            }

            if (isCorrectFork)
            {
                IsCompleted = true;
                OnCorrectExit?.Invoke();
                return;
            }

            if (_resetPoint != null && player != null)
            {
                player.Teleport(_resetPoint.position, _resetPoint.rotation);
            }
        }
    }
}
