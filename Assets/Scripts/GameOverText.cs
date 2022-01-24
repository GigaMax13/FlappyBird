using UnityEngine;

public class GameOverText : MonoBehaviour {
  private string currentAnimaton;

  private Animator animator;

  private void Awake() {
    animator = GetComponent<Animator>();
  }

  private void OnEnable() {
    Actions.OnGameStart += OnGameStart;
    Actions.OnGameOver += OnGameOver;
  }

  private void OnDisable() {
    Actions.OnGameStart -= OnGameStart;
    Actions.OnGameOver -= OnGameOver;
  }

  private void OnGameStart() {
    ChangeAnimationState("HideGameOver");
  }

  private void OnGameOver() {
    ChangeAnimationState("ShowGameOver");
  }

  private void ChangeAnimationState(string newAnimation) {
    if (currentAnimaton != newAnimation) {
      animator.Play(newAnimation);
      currentAnimaton = newAnimation;
    }
  }
}
