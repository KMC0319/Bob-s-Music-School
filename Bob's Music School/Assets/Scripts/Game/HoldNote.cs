using UnityEngine;

namespace Game {
    public class HoldNote : MonoBehaviour {
        private ENoteType type;
        public AudioClip Clip { get; private set; }

        public void Init(NoteBase noteBase) {
            type = noteBase.Type;
            Clip = noteBase.Clip;
        }
    }
}
