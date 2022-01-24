using UnityEngine;

public class ScoreHandler : MonoBehaviour {
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
    if (score != lastEmittedEvent) {
      lastEmittedEvent = score;
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
      return Mathf.FloorToInt(pipesColisions * .25f);
    }
  }
}
