using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour {
  private TextMeshProUGUI scoreText;

  private void Awake() {
    scoreText = GetComponent<TextMeshProUGUI>();
  }

  private void OnEnable() {
    scoreText.enabled = false;
    Actions.OnGameStart += showPlayerScore;
    Actions.OnPlayerScore += updatePlayerScore;
  }

  private void OnDisable() {
    Actions.OnGameStart -= showPlayerScore;
    Actions.OnPlayerScore -= updatePlayerScore;
  }

  private void showPlayerScore() {
    scoreText.enabled = true;
    updatePlayerScore(0);
  }

  private void updatePlayerScore(int score) {
    if (scoreText.enabled) {
      scoreText.text = "Score\n" + score.ToString();
    }
  }
}
