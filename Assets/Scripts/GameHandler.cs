using UnityEngine;

public class GameHandler : MonoBehaviour {
  private static GameHandler instance;

  public static GameHandler getInstance() {
    return instance;
  }

  private bool isPaused = true;
  private bool isOver = false;

  private void Awake() {
    instance = this;
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
      if (isGamePaused) {
        // Start
        if (!isGameOver) {
          tooglePause();
        }
        // Restart
        else {

        }
      }
    }
  }

  public void gameOver() {
    isPaused = true;
    isOver = true;
  }

  public void tooglePause() {
    if (!isGameOver) {
      isPaused = !isPaused;
    }
  }

  public bool isGameOver {
    get {
      return isOver;
    }
  }

  public bool isGamePaused {
    get {
      return isPaused;
    }
  }
}
