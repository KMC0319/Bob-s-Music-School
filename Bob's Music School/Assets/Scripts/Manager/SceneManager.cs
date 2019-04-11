using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager {
	public static class SceneManager {
		public static ESceneName NowScene { get; private set; }

		static SceneManager() {
			NowScene = GetActiveScene();
		}

		private static ESceneName GetActiveScene() {
			return (ESceneName) UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		}

		public static void LoadScene(ESceneName next) {
			NowScene = next;
			UnityEngine.SceneManagement.SceneManager.LoadScene((int) next);
		}
	}
}
