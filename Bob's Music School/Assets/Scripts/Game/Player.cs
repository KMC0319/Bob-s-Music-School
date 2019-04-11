using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Game {
    public class Player : MonoBehaviour {
        [SerializeField] private GameObject itemRoot;
        [SerializeField] private GameObject holdObject;
        private GameObject[] holdNotesBacks;
        private HoldNote[] holdNotes = new HoldNote[4];
        private readonly Subject<int> onNotesButtonDown = new Subject<int>();
        public IObservable<int> OnNotesButtonDown => onNotesButtonDown;
        private bool isGameStart;

        private void Start() {
            holdNotesBacks = itemRoot.GetComponentsInChildren<Transform>()
                .Skip(1)
                .OrderBy(i => i.position.x)
                .Select(i=>i.gameObject)
                .ToArray();
        }

        private void Update() {
            if (!isGameStart) return;
            if (Input.GetKeyDown(KeyCode.Z)) {
                onNotesButtonDown.OnNext(0);
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                onNotesButtonDown.OnNext(1);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                onNotesButtonDown.OnNext(2);
            }

            if (Input.GetKeyDown(KeyCode.V)) {
                onNotesButtonDown.OnNext(3);
            }
        }

        public void GameStart() {
            isGameStart = true;
        }

        public void Hold(NoteBase noteBase, int index) {
            var obj = Instantiate(holdObject);
            obj.transform.position = holdNotesBacks[index].transform.position;
            obj.AddComponent<HoldNote>().Init(noteBase);
        }
    }
}
