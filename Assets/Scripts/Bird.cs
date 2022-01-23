using UnityEngine;

public class Bird : MonoBehaviour {
  private float gravityScale = 35;
  private string currentAnimaton;
  private float jumpForce = 100;
  private bool isJumpPressed;

  private Transform birdTransform;
  private Animator animator;
  private Rigidbody2D rb2d;

  private void Awake() {
    birdTransform = GetComponent<Transform>();
    animator = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (GameHandler.getInstance().isGameOver) {
      ChangeAnimationState("BirdDead");
    } else {
      ChangeAnimationState("BirdFlapping");
    }

    if (GameHandler.getInstance().isGamePaused) {
      rb2d.velocity = new Vector2();
      rb2d.gravityScale = 0;
    } else {
      if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
        isJumpPressed = true;
      }

      if (rb2d.gravityScale == 0) {
        rb2d.gravityScale = gravityScale;
      }
    }
  }

  private void FixedUpdate() {
    if (GameHandler.getInstance().isGamePaused) {
      return;
    }

    if (isJumpPressed) {
      isJumpPressed = false;
      rb2d.velocity = Vector2.up * jumpForce;
    }

    birdTransform.eulerAngles = new Vector3(0, 0, rb2d.velocity.y * .2f);
  }

  private void OnTriggerEnter2D() {
    GameHandler.getInstance().gameOver();
  }

  void ChangeAnimationState(string newAnimation) {
    if (currentAnimaton == newAnimation) return;

    animator.Play(newAnimation);
    currentAnimaton = newAnimation;
  }
}
