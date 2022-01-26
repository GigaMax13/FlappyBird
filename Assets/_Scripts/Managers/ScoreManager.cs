using UnityEngine;

public class ScoreManager : MonoBehaviour {
  private int lastEmittedEvent;
  private float pipesColisions;

  private void Awake() {
    lastEmittedEvent = 0;
    pipesColisions = 0;
  }

  private void OnEnable() {
    Actions.OnGameStart += OnGameStart;
  }

  private void OnDisable() {
    Actions.OnGameStart -= OnGameStart;
  }

  private void FixedUpdate() {
    if (lastEmittedEvent != score) {
      lastEmittedEvent = score;

      if (lastEmittedEvent > 0) {
        SoundManager.PlaySound(AssetsManager.Sound.Point, .5f);
      }

      Actions.OnPlayerScore(lastEmittedEvent);
    }
  }

  private void OnTriggerEnter2D(Collider2D collider) {
    pipesColisions++;
  }

  private void OnGameStart() {
    lastEmittedEvent = 0;
    pipesColisions = 0;
  }

  private int score {
    get {
      int score = Mathf.FloorToInt(pipesColisions * .25f);
      return score;
    }
  }
}
