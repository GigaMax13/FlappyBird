using UnityEngine;

public class GameAssets : MonoBehaviour {
  private static GameAssets instance;

  public static GameAssets getInstance() {
    return instance;
  }

  private void Awake() {
    instance = this;
  }

  public Transform pipeHead;
  public Transform pipeBody;
  public Transform ground;
  public Transform cloud1;
  public Transform cloud2;
  public Transform cloud3;
}
