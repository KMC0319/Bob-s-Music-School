using UnityEngine;

namespace Game {
    public class Spectrum : MonoBehaviour {
        [SerializeField] private GameObject lineObject;
        [SerializeField] private FFTWindow fftWindow;
        private const int NumberOfSamples = 256;
        private float[] samples = new float[NumberOfSamples * 8];
        private GameObject[] lines = new GameObject[NumberOfSamples];

        private void Start() {
            for (var i = 0; i < NumberOfSamples; i++) {
                var pos = (Vector2) transform.position + new Vector2(i, 0);
                var obj = Instantiate(lineObject, pos, Quaternion.identity, gameObject.transform);
                lines[i] = obj;
            }
        }

        private void Update() {
            AudioListener.GetSpectrumData(samples, 0, fftWindow);
            //AudioListener.GetOutputData(samples, 0);//音量で作るとき
            for (var i = 0; i < NumberOfSamples; i++) {
                var scale = lines[i].transform.localScale;
                scale.y = Mathf.Clamp(samples[i] * 4000, 1, 350);//横軸の変更度が均等なやつ
                /*横軸の変更度を調整して、低・中音がたくさん表示される
                var power = 3;
                scale.y = Mathf.Clamp(samples[(int) (samples.Length * (float) (Mathf.Pow(i, power)) / (Mathf.Pow(NumberOfSamples, power)))] * 4000, 1, 350);*/
                lines[i].transform.localScale = scale;
            }
        }
    }
}
