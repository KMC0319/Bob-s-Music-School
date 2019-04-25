using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    /// <summary>
    /// ノーツの基底クラス
    /// 生成時に判定までの残り時間が発生する
    /// ノーツの情報（sound等）は全てSoundStatusが持っている
    /// </summary>
    public abstract class NoteBase : MonoBehaviour {
        [SerializeField] protected ENoteGenre genre;
        [SerializeField] protected List<ENoteTag> tagList = new List<ENoteTag>();
        private float remainingTime;
        private float speed; //一秒で移動する距離
        private float allowableRange = 1f; //前後合わせた許容範囲
        private bool isActive;
        private int laneNo;

        public ENoteGenre Genre => genre;
        public List<ENoteTag> TagList => tagList;
        public abstract ENoteType Type { get; }
        public SoundStatus SoundStatus { get; private set; }
        public int LaneNo => laneNo;

        private void Update() {
            if (isActive) Move();
            Debug.Log(CanHold);
        }
        /*
        public void Init(SoundStatus soundStatus, float time, Vector3 borderPosition, Vector3[] startPostions) {
            SoundStatus = soundStatus;
            remainingTime = time;
            SetSpeed(borderPosition);
            laneNo = Array.IndexOf(startPostions, transform.position);
        }*/

        public void Init(SoundStatus soundStatus, int barCount, float tempo, Vector3 borderPosition, Vector3[] startPostions) {
            SoundStatus = soundStatus;
            remainingTime = barCount * 60f / (tempo / 4f); //残り時間 ＝ 残り小節数 * 一小節の時間
            SetSpeed(borderPosition);
            laneNo = Array.IndexOf(startPostions, transform.position - new Vector3(125, 0, 0));
            allowableRange = 60f / (tempo / 4f) * 2;
        }

        /// <summary>
        /// 初期スピード設定
        /// </summary>
        protected void SetSpeed(Vector3 borderPosition) {
            var distance = transform.position.x - borderPosition.x;
            speed = distance / remainingTime;
            isActive = true;
        }

        /// <summary>
        /// とりあえず右から左に流れる
        /// </summary>
        protected virtual void Move() {
            remainingTime -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        }

        public bool CanHold => -allowableRange <= remainingTime && remainingTime <= 1;
    }
}
