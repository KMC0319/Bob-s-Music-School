using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Game {
    public class Player : MonoBehaviour {
        [SerializeField] private GameObject itemRoot;
        [SerializeField] private GameObject holdObject;
        [SerializeField] private GameObject soundRoot;
        private GameObject[] holdNotesBacks;
        private HoldNote[] holdNotes = new HoldNote[4];
        private AudioSource[] audioSources;
        private SoundPlayer soundPlayer;
        private readonly Subject<int> onNotesButtonDown = new Subject<int>();
        public IObservable<int> OnNotesButtonDown => onNotesButtonDown;
        private bool isGameStart;

        private void Start() {
            holdNotesBacks = itemRoot.GetComponentsInChildren<Transform>()
                .Skip(1)
                .OrderBy(i => i.position.x)
                .Select(i => i.gameObject)
                .ToArray();
            audioSources = soundRoot.GetComponentsInChildren<AudioSource>()
                .Skip(1)
                .ToArray();
            soundPlayer = soundRoot.GetComponent<SoundPlayer>();
            soundPlayer.OnBarStart.Subscribe(_ => BarStart());
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
            obj.GetComponent<MeshFilter>().mesh = noteBase.GetComponent<MeshFilter>().mesh;
            var script = obj.AddComponent<HoldNote>();
            script.Init(noteBase);
            holdNotes[index] = script;
        }

        private void BarStart() {
            for (int i = 0; i < audioSources.Length; i++) {
                if(holdNotes[i] == null) continue;
                audioSources[i].clip = holdNotes[i].Clip;
            }
            soundPlayer.SoundStart();
        }
    }
}
