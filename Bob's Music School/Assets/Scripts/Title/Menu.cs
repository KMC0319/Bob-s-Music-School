using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UI;

namespace Title
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private float delaykey = 0.1f;

        private ButtonBase[] buttons;
        private int cursorIndex;
        private int Nowcursor;
        public Text text;
        public float flameCount;

        private float upInterval;
        private float downInterval;

        public int TitleIndex;

        // Use this for initialization
        void Start()
        {
            TitleIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetAxisRaw("Vertical") > 0) {

                if (upInterval == -0.1f) upInterval = delaykey * 0.9f;
                upInterval += Time.deltaTime;
                if (upInterval > delaykey) {
                    Nowcursor++;
                    upInterval = 0;
                }

            } else if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (downInterval == -0.1f) downInterval = delaykey * 0.9f;
                downInterval += Time.deltaTime;
                upInterval = 0;
                if (downInterval > delaykey)
                {
                    Nowcursor--;
                    downInterval = 0;
                }
            } else {
                upInterval = -0.1f;
                downInterval = -0.1f;
            }
            //ここほぼ土間スクリプト//

            /*
            if (Input.GetAxisRaw("Vertical") == 1)//十字キー操作　↑
            {
                flameCount = flameCount + 1 * Time.deltaTime;
                Nowcursor = 1;

            }
            else if (Input.GetAxisRaw("Vertical") == -1)//十字キー操作　↓
            {
                flameCount = flameCount - 1 * Time.deltaTime;
                Nowcursor = -1;
            }
            else if (Input.GetAxisRaw("Vertical") == 0)//十字キー押してない
            {
                flameCount = 0;
                Nowcursor = 0;
            }

            if (Nowcursor == 1)
            {
                cursorIndex++;
            }
            if (Nowcursor == -1)
            {
                cursorIndex--;
            }

    */


            if (TitleIndex == 0 && Input.GetButtonDown("A"))
            {
                TitleIndex = 1;
            }
            if (TitleIndex == 1 && Input.GetButtonDown("B"))
            {
                TitleIndex = 0;
            }
            if (TitleIndex ==1 && Input.GetButtonDown("A"))
            {

            }

            text.text = "カーソル" + Nowcursor + "フレーム" + TitleIndex;

        }
    }
}
