using System.Collections.Generic;
using UnityEngine;
using System;

public class AssetsManager : MonoBehaviour {
  private static AssetsManager instance;
  public static AssetsManager Instance {
    get {
      return instance;
    }
  }

  [SerializeField]
  private Transform pipeHead;
  [SerializeField]
  private Transform pipeBody;
  [SerializeField]
  private Transform ground;

  [SerializeField]
  private Transform cloud1;
  [SerializeField]
  private Transform cloud2;
  [SerializeField]
  private Transform cloud3;

  public enum Sound {
    Flappy,
    Point,
    Die
  }

  [SerializeField]
  private SoundAudioClip[] sounds;

  private List<Transform> cloud;

  private void Awake() {
    cloud = new List<Transform>() {
      cloud1,
      cloud2,
      cloud3
    };
    instance = this;
  }

  public Transform PipeHead => pipeHead;

  public Transform PipeBody => pipeBody;

  public Transform Ground => ground;

  public List<Transform> Cloud => cloud;

  public AudioClip Sounds(Sound sound) {
    foreach (SoundAudioClip soundAudioClip in sounds) {
      if (soundAudioClip.sound == sound) {
        return soundAudioClip.audioClip;
      }
    }

    Debug.LogError("Sound not found: " + sound);
    return null;
  }

  [Serializable]
  public class SoundAudioClip {
    public Sound sound;
    public AudioClip audioClip;
  }
}
