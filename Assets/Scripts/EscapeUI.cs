using UnityEngine;

namespace WhisperingFog
{
    /// <summary>
    /// Bare on-screen status text - current progress, and a final message
    /// once ForestProgressManager reports the player has escaped. Wire
    /// ForestProgressManager's OnEscaped event to this component's
    /// MarkEscaped() method in the inspector.
    ///
    /// Text content, font size and color are all editable in the Inspector
    /// below - no code changes needed to tweak wording or look.
    /// </summary>
    public class EscapeUI : MonoBehaviour
    {
        [SerializeField] private ForestProgressManager _progress;

        [Header("Text")]
        [Tooltip("Shown once the player reaches the correct exit. Nothing is shown before that.")]
        [SerializeField] private string _escapedMessage = "You found your way out of the fog.";

        [Header("Style")]
        [SerializeField] private int _fontSize = 24;
        [SerializeField] private Color _textColor = Color.white;
        [Tooltip("Optional - leave empty to use Unity's default legacy GUI font.")]
        [SerializeField] private Font _customFont;
        [SerializeField] private float _boxWidth = 420f;
        [SerializeField] private float _boxHeight = 100f;

        private bool _escaped;
        private GUIStyle _style;

        public void MarkEscaped() => _escaped = true;

        private void OnGUI()
        {
            if (_progress == null)
            {
                return;
            }

            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    wordWrap = true
                };
            }

            _style.fontSize = _fontSize;
            _style.normal.textColor = _textColor;
            if (_customFont != null)
            {
                _style.font = _customFont;
            }

            if (!_escaped)
            {
                // Nothing shown while still wandering - only the final message matters.
                return;
            }

            var centeredRect = new Rect(
                (Screen.width - _boxWidth) / 2f,
                (Screen.height - _boxHeight) / 2f,
                _boxWidth,
                _boxHeight);

            GUILayout.BeginArea(centeredRect, GUI.skin.box);
            GUILayout.Label(_escapedMessage, _style);
            GUILayout.EndArea();
        }
    }
}
