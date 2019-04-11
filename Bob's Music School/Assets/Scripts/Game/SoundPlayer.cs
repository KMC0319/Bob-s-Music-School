using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Game {
	public class SoundPlayer : MonoBehaviour {
		private AudioSource bgm;
		private AudioSource[] sounds;
		private float tempo;
		private bool isGameStart;
		private readonly Subject<Unit> barStream = new Subject<Unit>();
		public IObservable<Unit> OnBarStart => barStream;
		
		private void Start () {
			bgm = GetComponent<AudioSource>();
			sounds = GetComponentsInChildren<AudioSource>().Skip(1).ToArray();
		}

		public void GameStart(float _tempo) {
			isGameStart = true;
			tempo = _tempo;
			bgm.Play();
			foreach (var audioSource in sounds) {
				audioSource.clip = null;
			}

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

		public void SoundStart() {/*
			bgm.Stop();
			foreach (var audioSource in sounds) {
				audioSource.Stop();
			}*/
			if (!bgm.isPlaying) bgm.Play();
			foreach (var audioSource in sounds.Where(i=>i.clip!=null)) {
				if (!audioSource.isPlaying) audioSource.Play();
			}
		}
	}
}
