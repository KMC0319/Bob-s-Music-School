using System.Linq;
using UI;
using UnityEngine;

namespace Title {
    public class TitleMenu : MonoBehaviour {
        [SerializeField] private GameObject root;
        private ButtonBase[] buttons;
        private int cursorIndex;
        
        private void Start() {
            buttons = root.GetComponentsInChildren<ButtonBase>();
            buttons = buttons.OrderBy(i => i.transform.position.y).ToArray();
            Debug.Log(buttons.First());
        }

        private void Update() {
            if(buttons.Length <= 0) return;
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                cursorIndex++;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                cursorIndex--;
            }

            cursorIndex = Mathf.Clamp(cursorIndex, 0, buttons.Length - 1);
            if (Input.GetKeyDown(KeyCode.Z)) {
                OnButtonDown();
            }
        }

        private void OnButtonDown() {
            buttons[cursorIndex].OnButtonDown();
        }
    }
}
