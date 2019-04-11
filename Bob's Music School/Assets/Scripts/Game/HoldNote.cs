using UnityEngine;

namespace Game {
    public class HoldNote : MonoBehaviour {
        private ENoteType type;
        public void Init(NoteBase noteBase) {
            type = noteBase.Type;
        }
    }
}
