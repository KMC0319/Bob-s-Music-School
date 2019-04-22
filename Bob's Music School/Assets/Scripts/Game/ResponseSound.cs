using UniRx;
using UnityEngine;

namespace Game {
    public class ResponseSound : MonoBehaviour {
        [SerializeField] private Player player;
        [SerializeField] private AudioClip[] standardSounds;
        private AudioSource audioSource;
        
        private void Start() {
            player.OnSoundsButtonDown.Subscribe(PlaySound);
            audioSource = GetComponent<AudioSource>();
        }

        private void PlaySound(int num) {
            Debug.Log(num);
            if (player.HoldNotes[num % 4] != null) return;
            audioSource.PlayOneShot(standardSounds[num]);
        }
    }
}
