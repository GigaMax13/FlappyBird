using UnityEngine;

public class Bird : MonoBehaviour {
  private float gravityScale = 35;
  private string currentAnimaton;
  private float jumpForce = 100;
  private bool isJumpPressed = false;
  private bool isGameStarted = false;
  private bool isGamePaused = true;
  private bool isGameOver = false;

  private Transform birdTransform;
  private Animator animator;
  private Rigidbody2D rb2d;

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

  private void Awake() {
    birdTransform = GetComponent<Transform>();
    animator = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (!isGameStarted || (isGameStarted && !isGamePaused)) {
      ChangeAnimationState("BirdFlapping");
    } else {
      ChangeAnimationState("BirdDead");
    }

    if (!isGamePaused) {
      if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
        isJumpPressed = true;
      }

      if (rb2d.gravityScale == 0) {
        rb2d.gravityScale = gravityScale;
      }
    } else {
      rb2d.velocity = new Vector2();
      rb2d.gravityScale = 0;
    }
  }

  private void FixedUpdate() {
    if (!isGamePaused) {
      if (isJumpPressed) {
        isJumpPressed = false;
        rb2d.velocity = Vector2.up * jumpForce;
      }

      birdTransform.eulerAngles = new Vector3(0, 0, rb2d.velocity.y * .2f);
    }
  }

  private void OnTriggerEnter2D(Collider2D collider) {
    Actions.OnGameOver();
    Debug.Log("Colision with: " + collider);
  }

  private void ChangeAnimationState(string newAnimation) {
    if (currentAnimaton != newAnimation) {
      animator.Play(newAnimation);
      currentAnimaton = newAnimation;
    }
  }

  private void OnGameStart() {
    rb2d.position = new Vector3(0, 0, 0);
    isGameOver = false;
    isGameStarted = true;
  }

  private void OnGamePause(bool isPaused) {
    isGamePaused = isPaused;
  }

  private void OnGameOver() {
    isGameOver = true;
  }
}
