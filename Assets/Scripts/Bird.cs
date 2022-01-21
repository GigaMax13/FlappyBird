using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour {
  private Rigidbody2D birdRigidbody2D;

  private const float JUMP = 100f;

  private void Awake() {
    birdRigidbody2D = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
      Jump();
    }
  }

  private void Jump() {
    birdRigidbody2D.velocity = Vector2.up * JUMP;
  }

  private void OnTriggerEnter2D(Collider2D collider) {
    CMDebug.TextPopupMouse("Dead!");
  }
}
