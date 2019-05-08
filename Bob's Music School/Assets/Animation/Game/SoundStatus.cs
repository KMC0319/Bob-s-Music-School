using System;
using UnityEngine;

namespace Game {
    /// <summary>
    /// soundの状態を定義するクラス
    /// 実際の再生は何故かここから実行している
    /// </summary>
    [Serializable]
    public class SoundStatus {
        [SerializeField] private AudioClip clip;
        [SerializeField] private int barCount;
        
        public AudioClip Clip => clip;
        public int BarCount => barCount;
        public int RemainBarCount { get; private set; }

        public bool EndFlg => RemainBarCount <= 0;
        
        /// <summary>
        /// 定義した小節分再生していたら頭から再生しなおす
        /// </summary>
        public void Play(AudioSource source) {
            if (EndFlg) {
                Reset();
                source.Play();
            }
            RemainBarCount--;
        }

        private void Reset() {
            RemainBarCount = barCount;
        }
    }
}