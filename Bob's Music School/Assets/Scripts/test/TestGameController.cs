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
        private bool isGameStart;
        private List<NoteBase> noteBases = new List<NoteBase>();
        private int currentLane;

        private void Awake() {
            player.OnNotesButtonDown.Subscribe(CheckHold);
            player.OnMoveButtonDown.Subscribe(Move);
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
            Observable.Interval(TimeSpan.FromSeconds(0.5f))
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
            currentLane = Mathf.Clamp(currentLane + num, 0, 4);
            currentLaneObj.transform.position = startPositions[currentLane] + new Vector3(-16, 0, 0);
        }

        private void RandomCreate() {
            var type = (ENoteType) Random.Range(0, 3);
            var obj = noteFactory.Create(type);
            obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
            switch (type) {
                case ENoteType.Melody:
                    obj.Init(melodys[Random.Range(0, melodys.Length)], barCount, tempo, border.transform.position, startPositions);
                    break;
                case ENoteType.Rhythm:
                    obj.Init(rhythms[Random.Range(0, rhythms.Length)], barCount, tempo, border.transform.position, startPositions);
                    break;
                case ENoteType.Fx:
                    obj.Init(fxs[Random.Range(0, fxs.Length)], barCount, tempo, border.transform.position, startPositions);
                    break;
                case ENoteType.Bass:
                    obj.Init(basses[Random.Range(0, basses.Length)], barCount, tempo, border.transform.position, startPositions);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            noteBases.Add(obj);
        }
    }
}
