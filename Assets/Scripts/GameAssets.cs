using UnityEngine;

public class GameAssets : MonoBehaviour {
  private static GameAssets instance;

  public static GameAssets GetInstance() {
    return instance;
  }

  private void Awake() {
    instance = this;
  }

  public Transform pipeHead;
  public Transform pipeBody;
}
