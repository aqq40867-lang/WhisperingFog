using UnityEngine;
using UnityEngine.Events;

namespace WhisperingFog
{
    /// <summary>
    /// Tracks overall progress through the fog. Wire each CorridorSegment's
    /// OnCorrectExit event (in the inspector) to this component's
    /// AdvanceSegment() method. Once every segment has been solved, raises
    /// OnEscaped.
    /// </summary>
    public class ForestProgressManager : MonoBehaviour
    {
        [SerializeField] private int _totalSegments = 3;

        public UnityEvent OnEscaped;

        public int TotalSegments => _totalSegments;
        public int SegmentsCompleted { get; private set; }
        public bool HasEscaped { get; private set; }

        public void AdvanceSegment()
        {
            if (HasEscaped)
            {
                return;
            }

            SegmentsCompleted++;

            if (SegmentsCompleted >= _totalSegments)
            {
                HasEscaped = true;
                OnEscaped?.Invoke();
            }
        }
    }
}
