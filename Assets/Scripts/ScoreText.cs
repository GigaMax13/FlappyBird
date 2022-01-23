using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour {
  private TextMeshProUGUI scoreText;

  private void Awake() {
    scoreText = GetComponent<TextMeshProUGUI>();
  }
  void Update() {
    scoreText.text = ScoreHandler.getInstance().score.ToString();
  }
}
