using UnityEngine;

public class GameHandler : MonoBehaviour {
  private bool isGameStarted = false;
  private bool isGamePaused = true;
  private bool isGameOver = false;
  private int playerScore = 0;
  private int hightScore = 0;
  private bool isSpacePressed = false;
  private bool isScapePressed = false;

  private void OnEnable() {
    Actions.OnPlayerScore += UpdatePlayerScore;
    Actions.OnGameOver += OnGameOver;
  }

  private void OnDisable() {
    Actions.OnPlayerScore -= UpdatePlayerScore;
    Actions.OnGameOver -= OnGameOver;
  }

  private void Update() {
    isSpacePressed = (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
    isScapePressed = Input.GetKeyDown(KeyCode.Escape);

    if (!isGameStarted && isSpacePressed) {
      OnGameStart();
    }

    if (isGameStarted && !isGameOver) {
      if (!isGamePaused && isScapePressed) {
        ToggleGamePause();
      }

      if (isGamePaused && isSpacePressed) {
        ToggleGamePause();
      }
    }

    if (isGameOver && isSpacePressed) {
      OnGameStart();
    }
  }

  private void UpdatePlayerScore(int newScore) {
    playerScore = newScore;
  }

  private void OnGameStart() {
    isGameStarted = true;
    isGameOver = false;
    Actions.OnGameStart();
    Actions.OnPlayerScore(0);
    SetGamePause(false);
  }

  private void OnGameOver() {
    if (playerScore > hightScore) {
      hightScore = playerScore;
    }

    isGameStarted = false;
    isGameOver = true;
    SetGamePause(true);
  }

  private void SetGamePause(bool pauseValue) {
    isGamePaused = pauseValue;
    Actions.OnGamePause(isGamePaused);
  }

  private void ToggleGamePause() {
    isGamePaused = !isGamePaused;
    Actions.OnGamePause(isGamePaused);
  }
}
