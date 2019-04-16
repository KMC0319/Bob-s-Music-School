using UnityEngine;

namespace Game {
    /// <summary>
    /// 保持しているノーツのクラス
    /// Playerが生成・破棄している
    /// </summary>
    public class HoldNote : MonoBehaviour {
        private ENoteType type;
        private AudioSource source;

        public SoundStatus SoundStatus { get; private set; }
        //public bool DeleteFlg { get; private set; }

        public void Init(NoteBase noteBase, AudioSource audioSource) {
            type = noteBase.Type;
            source = audioSource;
            SoundStatus = noteBase.SoundStatus;
            source.clip = SoundStatus.Clip;
        }

        public void Play() {
            SoundStatus.Play(source);
        }

        public void Delete() {
            source.Stop();
            //DeleteFlg = true;
            Destroy(gameObject);
        }
    }
}
