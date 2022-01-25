using UnityEngine;

public static class SoundManager {
  public static void PlaySound(AssetsManager.Sound sound, float volume = 1f) {
    GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
    AudioClip audio = AssetsManager.Instance.Sounds(sound);

    if (audio != null) {
      audioSource.PlayOneShot(audio, volume);
    }
  }
}
