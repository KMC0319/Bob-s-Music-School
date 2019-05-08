using UniRx;
using UnityEngine;

namespace Game {
    public class ResponseSound : MonoBehaviour {
        [SerializeField] private Player player;
        [SerializeField] private AudioClip[] sounds;
        private AudioSource audioSource;
        
        private void Start() {
            player.OnSoundsButtonDown.Subscribe(PlaySound);
            audioSource = GetComponent<AudioSource>();
        }

        private void PlaySound(int num) {
            if (player.HoldNotes[num % 4] != null) return;
            audioSource.PlayOneShot(sounds[num]);
        }
    }
}
