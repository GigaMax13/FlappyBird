using UnityEngine;

public class PauseScript : MonoBehaviour {
  private bool isGameStarted = false;
  private bool isGamePaused = true;
  private bool isGameOver = false;
  private string currentAnimaton;

  private Animator animator;

  private void Awake() {
    animator = GetComponent<Animator>();
  }

  private void OnEnable() {
    Actions.OnGameStart += OnGameStart;
    Actions.OnGamePause += OnGamePause;
    Actions.OnGameOver += OnGameOver;
  }

  private void OnDisable() {
    Actions.OnGameStart -= OnGameStart;
    Actions.OnGamePause -= OnGamePause;
    Actions.OnGameOver -= OnGameOver;
  }

  private void FixedUpdate() {
    if (isGameStarted && isGamePaused && !isGameOver) {
      if (currentAnimaton != "BlinkPause") {
        ChangeAnimationState("ShowPause");
      }
    } else {
      ChangeAnimationState("HidePause");
    }
  }

  private void OnGameStart() {
    isGameOver = false;
    isGameStarted = true;
  }

  private void OnGamePause(bool isPaused) {
    isGamePaused = isPaused;
  }

  private void OnGameOver() {
    isGameOver = true;
  }

  private void ChangeAnimationState(string newAnimation) {
    if (currentAnimaton != newAnimation) {
      animator.Play(newAnimation);
      currentAnimaton = newAnimation;
    }
  }

  public void BlinkPauseText() {
    ChangeAnimationState("BlinkPause");
  }
}
