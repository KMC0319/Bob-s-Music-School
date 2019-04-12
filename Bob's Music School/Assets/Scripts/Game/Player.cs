using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Game {
    /// <summary>
    /// プレイヤーが操作すると動く機能が書いてあるクラス
    /// ノーツの保持・破棄・ミュートの機能がある
    /// </summary>
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
            if (Input.GetKeyDown(KeyCode.Z)) onNotesButtonDown.OnNext(0);
            if (Input.GetKeyDown(KeyCode.X)) onNotesButtonDown.OnNext(1);
            if (Input.GetKeyDown(KeyCode.C)) onNotesButtonDown.OnNext(2);
            if (Input.GetKeyDown(KeyCode.V)) onNotesButtonDown.OnNext(3);

            if (Input.GetKeyDown(KeyCode.A)) Delete(0);
            if (Input.GetKeyDown(KeyCode.S)) Delete(1);
            if (Input.GetKeyDown(KeyCode.D)) Delete(2);
            if (Input.GetKeyDown(KeyCode.F)) Delete(3);

            if (Input.GetKeyDown(KeyCode.Q)) Mute(0);
            if (Input.GetKeyDown(KeyCode.W)) Mute(1);
            if (Input.GetKeyDown(KeyCode.E)) Mute(2);
            if (Input.GetKeyDown(KeyCode.R)) Mute(3);
        }

        public void GameStart() {
            isGameStart = true;
        }

        public void Hold(NoteBase noteBase, int index) {
            //保持ノーツの生成
            var obj = Instantiate(holdObject,
                holdNotesBacks[index].transform.position,
                holdNotesBacks[index].transform.rotation);
            obj.transform.parent = holdNotesBacks[index].transform;
            //保持ノーツの形を流れていたノーツと合わせる
            obj.GetComponent<MeshFilter>().mesh = noteBase.GetComponent<MeshFilter>().mesh;
            //スクリプトのアタッチ・初期化
            var script = obj.AddComponent<HoldNote>();
            script.Init(noteBase, audioSources[index]);
            audioSources[index].mute = false;
            //既存の保持ノーツの上書き
            if (holdNotes[index] != null) holdNotes[index].Delete();
            holdNotes[index] = script;
        }

        private void Delete(int index) {
            if (holdNotes[index] == null) return;
            holdNotes[index].Delete();
        }

        private void Mute(int index) {
            if (holdNotes[index] == null) return;
            audioSources[index].mute = !audioSources[index].mute;
            holdNotes[index].GetComponent<MeshRenderer>().material.color = audioSources[index].mute ? Color.yellow : Color.white;
        }

        private void BarStart() {
            soundPlayer.SoundStart(holdNotes);
        }
    }
}
