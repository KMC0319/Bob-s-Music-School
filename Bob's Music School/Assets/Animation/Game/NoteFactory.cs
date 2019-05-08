using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {
    public class NoteFactory: MonoBehaviour {
        [SerializeField] private List<NoteBase> notes;
        public NoteBase Create(ENoteType type) {
            return Instantiate(notes.First(i => i.Type == type));
        }
    }
}
