using UnityEngine;

public class Bird : MonoBehaviour {
  private float gravityScale = 35;
  private float jumpForce = 100;
  private bool isJumpPressed;

  private Rigidbody2D rb2d;

  private void Awake() {
    rb2d = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (GameHandler.getInstance().isGamePaused) {
      rb2d.velocity = new Vector2();
      rb2d.gravityScale = 0;
    } else {
      if (rb2d.gravityScale == 0) {
        rb2d.gravityScale = gravityScale;
      }

      if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
        isJumpPressed = true;
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
  }

  private void OnTriggerEnter2D(Collider2D collider) {
    GameHandler.getInstance().gameOver();
    // CMDebug.TextPopupMouse("Dead!");
  }
}
