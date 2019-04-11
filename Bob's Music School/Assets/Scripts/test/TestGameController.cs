using System.Collections.Generic;
using System.Linq;
using Game;
using UniRx;
using UnityEngine;

namespace test {
    public class TestGameController : MonoBehaviour {
        [SerializeField] private NoteFactory noteFactory;
        [SerializeField] private Vector3[] startPositions;
        [SerializeField] private GameObject border;
        [SerializeField] private int barCount;
        [SerializeField] private float tempo;
        [SerializeField] private Player player;
        private bool isGameStart;
        private List<NoteBase> noteBases = new List<NoteBase>();

        private void Awake() {
            player.OnNotesButtonDown.Subscribe(CheckHold);
        }

        private void Update() {
            if (!isGameStart && Input.GetKeyDown(KeyCode.Alpha0)) {
                GameStart();
            }

            if (!isGameStart) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                var obj = noteFactory.Create(ENoteType.Melody);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(barCount, tempo, border.transform.position);
                noteBases.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                var obj = noteFactory.Create(ENoteType.Rhythm);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(barCount, tempo, border.transform.position);
                noteBases.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                var obj = noteFactory.Create(ENoteType.Fx);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(barCount, tempo, border.transform.position);
                noteBases.Add(obj);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                var obj = noteFactory.Create(ENoteType.Lyrics);
                obj.transform.position = startPositions[Random.Range(0, startPositions.Length)];
                obj.Init(barCount, tempo, border.transform.position);
                noteBases.Add(obj);
            }
        }

        private void GameStart() {
            isGameStart = true;
            player.GameStart();
        }

        private void CheckHold(int index) {
            if (noteBases.All(i => !i.CanHold)) return;
            var obj = noteBases.First(i => i.CanHold);
            player.Hold(obj, index);
            noteBases.Remove(obj);
            Destroy(obj.gameObject);
        }
    }
}
