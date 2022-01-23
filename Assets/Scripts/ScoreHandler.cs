using UnityEngine;

public class ScoreHandler : MonoBehaviour {
  private static ScoreHandler instance;

  public static ScoreHandler getInstance() {
    return instance;
  }

  private int playserScore;

  private void Awake() {
    playserScore = 0;
    instance = this;
  }

  private void OnTriggerEnter2D(Collider2D collider) {
    Debug.Log("Colision! " + collider.name);
    playserScore++;
  }

  public int score {
    get {
      return playserScore / 4;
    }
  }
}
