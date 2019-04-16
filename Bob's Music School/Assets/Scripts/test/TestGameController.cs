using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace test {
    /// <summary>
    /// ゲームシーン全体を管轄するテスト用クラス
    /// ゲームスタート・ノーツの生成命令・ノーツのヒット確認等を行っている
    /// </summary>
    public class TestGameController : MonoBehaviour {
        [SerializeField] private NoteFactory noteFactory;
        [SerializeField] private Vector3[] startPositions;
        [SerializeField] private GameObject border;
        [SerializeField] private int barCount;
        [SerializeField] private float tempo;
        [SerializeField] private Player player;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private SoundStatus[] melodys;
        [SerializeField] private SoundStatus[] rhythms;
        [SerializeField] private SoundStatus[] fxs;
        [SerializeField] private SoundStatus[] basses;
        [SerializeField] private GameObject currentLaneObj;
        private Tuple<ENoteType, SoundStatus[]>[] sounds;
        private bool isGameStart;
        private List<NoteBase> noteBases = new List<NoteBase>();
        private int currentLane;

        private void Awake() {
            player.OnNotesButtonDown.Subscribe(CheckHold);
            player.OnMoveButtonDown.Subscribe(Move);
            sounds = new[] {
                new Tuple<ENoteType, SoundStatus[]>(ENoteType.Melody, melodys),
                new Tuple<ENoteType, SoundStatus[]>(ENoteType.Rhythm, rhythms),
                new Tuple<ENoteType, SoundStatus[]>(ENoteType.Fx, fxs),
                new Tuple<ENoteType, SoundStatus[]>(ENoteType.Bass, basses)
            };
        }

        private void Update() {
            if (!isGameStart && Input.GetKeyDown(KeyCode.Alpha0)) {
                GameStart();
            }

            if (!isGameStart) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                var obj = noteFactory.Create(ENoteType.Melody);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(melodys[Random.Range(0, melodys.Length)], barCount, tempo, border.transform.position, startPositions);
                noteBases.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                var obj = noteFactory.Create(ENoteType.Rhythm);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(rhythms[Random.Range(0, rhythms.Length)], barCount, tempo, border.transform.position, startPositions);
                noteBases.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                var obj = noteFactory.Create(ENoteType.Fx);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(fxs[Random.Range(0, fxs.Length)], barCount, tempo, border.transform.position, startPositions);
                noteBases.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                var obj = noteFactory.Create(ENoteType.Bass);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(basses[Random.Range(0, basses.Length)], barCount, tempo, border.transform.position, startPositions);
                noteBases.Add(obj);
            }
        }

        private void GameStart() {
            isGameStart = true;
            player.GameStart();
            soundPlayer.GameStart(tempo);
            //一小節ごとにノーツを生成
            Observable.Interval(TimeSpan.FromSeconds(60f / (tempo / 4f)))
                .Subscribe(_ => RandomCreate());
        }

        private void CheckHold(int index) {
            if (noteBases.All(i => !i.CanHold || currentLane != i.LaneNo)) return;
            var obj = noteBases.First(i => i.CanHold && i.LaneNo == currentLane);
            player.Hold(obj, index);
            noteBases.Remove(obj);
            Destroy(obj.gameObject);
        }

        private void Move(int num) {
            currentLane = Mathf.Clamp(currentLane + num, 0, 3);
            currentLaneObj.transform.position = startPositions[currentLane] + new Vector3(-15, 0, 0);
        }

        private void RandomCreate() {
            //現在存在しているノーツ(playerのHoldNotesとnoteBasesを合成)
            var activeSounds = new List<SoundStatus>(noteBases.Select(i => i.SoundStatus));
            activeSounds.AddRange(player.HoldNotes.Where(x => x != null).Select(i => i.SoundStatus).ToList());
            //選択可能な(現在存在していない)ノーツのTypeを取得・ランダム選択
            var selectableSounds = sounds.Where(x => x.Item2.Any(y => !activeSounds.Contains(y))).ToArray();
            var selectedSound = selectableSounds[Random.Range(0, selectableSounds.Length)];
            var type = selectedSound.Item1;
            //選ばれたタイプのsoundから選択可能なsoundを取得・ランダム選択
            var selectableClips = selectedSound.Item2.Where(i => !activeSounds.Contains(i)).ToArray();
            var clip = selectableClips[Random.Range(0, selectableClips.Length)];
            //生成・ランダム配置・セットアップ
            var obj = noteFactory.Create(type);
            obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
            obj.Init(clip, barCount, tempo, border.transform.position, startPositions);
            //現在存在しているノーツに追加
            noteBases.Add(obj);
        }
    }
}
