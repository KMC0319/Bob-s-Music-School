using UnityEngine;

namespace Game {
    public class Spectrum : MonoBehaviour {
        [SerializeField] private GameObject lineObject;
        private const int NumberOfSamples = 256;
        private float[] samples = new float[NumberOfSamples];
        private GameObject[] lines = new GameObject[NumberOfSamples];
        private void Start() {
            for (var i = 0; i < NumberOfSamples; i++) {
                var pos = (Vector2) transform.position + new Vector2(i, 0);
                var obj = Instantiate(lineObject, pos, Quaternion.identity, gameObject.transform);
                lines[i] = obj;
            }
        }

        private void Update() {
            AudioListener.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
            for (var i = 0; i < NumberOfSamples; i++) {
                var scale = lines[i].transform.localScale;
                scale.y = Mathf.Clamp(samples[i] * 2000, 1, 300);
                lines[i].transform.localScale = scale;
            }
        }
    }
}
