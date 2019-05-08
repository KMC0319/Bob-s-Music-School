using UnityEngine;

namespace Game {
    /// <summary>
    /// 現在のBGMを保持するクラス
    /// </summary>
    public class Bgm : MonoBehaviour {
        [SerializeField] private SoundStatus[] bgms;
        private AudioSource source;
        private SoundStatus currentBgm;

        private void Start() {
            source = GetComponent<AudioSource>();
            currentBgm = bgms[Random.Range(0, bgms.Length)];
        }

        public void SetBgm() {
            if (currentBgm.EndFlg) {
                currentBgm = bgms[Random.Range(0, bgms.Length)];
                source.clip = currentBgm.Clip;
            }
        }

        public void Play() {
            currentBgm.Play(source);
        }
    }
}
