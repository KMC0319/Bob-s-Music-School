using Manager;
using UnityEngine;

namespace UI {
	public class SceneMoveButton : ButtonBase {
		[SerializeField] private ESceneName sceneName;

		public override void OnButtonDown() {
			SceneManager.LoadScene(sceneName);
		}
	}
}
