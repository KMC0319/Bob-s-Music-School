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
        private readonly Subject<int> onSoundsButtonDown = new Subject<int>();
        private readonly Subject<int> onMoveButtonDown = new Subject<int>();
        public IObservable<int> OnNotesButtonDown => onNotesButtonDown;
        public IObservable<int> OnSoundsButtonDown => onSoundsButtonDown;
        public IObservable<int> OnMoveButtonDown => onMoveButtonDown;
        public HoldNote[] HoldNotes => holdNotes;
        private bool isGameStart;
        private float axisDeadLine = 0; //0-1の範囲
        private Mode nowMode;

        private void Start() {
            holdNotesBacks = itemRoot.GetComponentsInChildren<Transform>()
                .Skip(1)
                .OrderBy(i => i.position.y)
                .ThenByDescending(i => i.position.x)
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
            if (Input.GetAxisRaw(EButtonName.Vertical.ToString()) > axisDeadLine) onMoveButtonDown.OnNext(-1);
            if (Input.GetAxisRaw(EButtonName.Vertical.ToString()) < -axisDeadLine) onMoveButtonDown.OnNext(1);

            if (Input.GetAxisRaw(EButtonName.Horizontal.ToString()) > axisDeadLine) {
                nowMode = Mode.Mute;
            } else if (Input.GetAxisRaw(EButtonName.Horizontal.ToString()) < -axisDeadLine) {
                nowMode = Mode.Delete;
            } else if (Input.GetButton(EButtonName.RB.ToString())) {
                nowMode = Mode.RB;
            } else if (Input.GetAxisRaw(EButtonName.RT.ToString()) < -axisDeadLine) {
                nowMode = Mode.RT;
            } else {
                nowMode = Mode.Normal;
            }

            NoteButtonCheck(nowMode);
        }

        private void NoteButtonCheck(Mode mode) {
            var num = -1;
            if (Input.GetButtonDown(EButtonName.A.ToString())) num = 0;
            if (Input.GetButtonDown(EButtonName.B.ToString())) num = 1;
            if (Input.GetButtonDown(EButtonName.X.ToString())) num = 2;
            if (Input.GetButtonDown(EButtonName.Y.ToString())) num = 3;

            if (num == -1) return;

            switch (mode) {
                case Mode.Normal:
                    onNotesButtonDown.OnNext(num);
                    onSoundsButtonDown.OnNext(num);
                    break;
                case Mode.Mute:
                    Mute(num);
                    break;
                case Mode.Delete:
                    Delete(num);
                    break;
                case Mode.RB:
                    onSoundsButtonDown.OnNext(num + 4);
                    break;
                case Mode.RT:
                    onSoundsButtonDown.OnNext(num + 8);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
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

        private enum Mode {
            Normal,
            Mute,
            Delete,
            RB,
            RT
        }
    }
}
