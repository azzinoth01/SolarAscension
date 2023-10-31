using UnityEngine;

namespace SolarAscension {
    public class CursorManager : MonoBehaviour {
        [SerializeField] private Texture2D _defaultCursor, _buildCursor, _buildDeleteCursor, _buildMoveCursor;

        private static CursorManager _instance;
        public static CursorManager Instance { get { return _instance; } }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void SetCursor(InteractionState state) {
            switch (state) {
                case InteractionState.BuildSingle:
                    Cursor.SetCursor(_buildCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case InteractionState.BuildMultiple:
                    Cursor.SetCursor(_buildCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case InteractionState.BuildDeleting:
                    Cursor.SetCursor(_buildDeleteCursor, Vector2.zero, CursorMode.Auto);
                    break;
                case InteractionState.BuildMoving:
                    Cursor.SetCursor(_buildMoveCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
    }
}