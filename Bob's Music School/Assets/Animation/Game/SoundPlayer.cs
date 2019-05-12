using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Game {
	/// <summary>
	/// 小節ごとにBGM・保持ノーツの再生命令をするクラス
	/// </summary>
	public class SoundPlayer : MonoBehaviour {
		//private Bgm bgm;
		private float tempo;
		private bool isGameStart;
		private readonly Subject<Unit> barStream = new Subject<Unit>();
		public IObservable<Unit> OnBarStart => barStream;
		
		private void Start () {
			//bgm = GetComponent<Bgm>();
		}

		public void GameStart(float _tempo) {
			isGameStart = true;
			tempo = _tempo;
		//	bgm.SetBgm();
		//	bgm.Play();
			StartCoroutine(SoundLoop());
		}

		public void GameEnd() {
			isGameStart = false;
		}

		private IEnumerator SoundLoop() {
			var time = 60f / tempo * 4f;
			while (isGameStart) {
				time -= Time.deltaTime;
				if (time <= 0) {
					time = 60f / tempo * 4f;
					barStream.OnNext(Unit.Default);
				}
				yield return null;
			}
		}

		public void SoundStart(HoldNote[] holdNotes) {
		//	bgm.SetBgm();
		//	bgm.Play();
			foreach (var holdNote in holdNotes.Where(i=>i != null)) {
				holdNote.Play();
			}
		}
	}
}
