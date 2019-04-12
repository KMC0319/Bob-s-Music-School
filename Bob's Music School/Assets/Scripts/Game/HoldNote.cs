using UnityEngine;

namespace Game {
    /// <summary>
    /// 保持しているノーツのクラス
    /// Playerが生成・破棄している
    /// </summary>
    public class HoldNote : MonoBehaviour {
        private ENoteType type;
        private AudioSource source;
        private SoundStatus soundStatus;
        //public bool DeleteFlg { get; private set; }

        public void Init(NoteBase noteBase, AudioSource audioSource) {
            type = noteBase.Type;
            source = audioSource;
            soundStatus = noteBase.SoundStatus;
            source.clip = soundStatus.Clip;
        }

        public void Play() {
            soundStatus.Play(source);
        }

        public void Delete() {
            source.Stop();
            //DeleteFlg = true;
            Destroy(gameObject);
        }
    }
}
