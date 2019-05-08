using System.Collections;
using UniRx;
using UnityEngine;

namespace Game {
    public class Border : MonoBehaviour {
        [SerializeField] private Player player;
        private MeshRenderer renderer;

        private void Start() {
            player.OnNotesButtonDown.Subscribe(_ => StartCoroutine(ChangeColor()));
            renderer = GetComponent<MeshRenderer>();
        }

        IEnumerator ChangeColor() {
            renderer.material.color = Color.gray;
            var time = 0.1f;
            while (time > 0) {
                time -= Time.deltaTime;
                yield return null;
            }
            renderer.material.color = Color.white;
        }
    }
}
