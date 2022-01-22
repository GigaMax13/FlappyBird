using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
  private const float PIPE_DESTROY_X_POSITION = -100f;
  private const float PIPE_SPAWN_X_POSITION = 100f;
  private const int INCREASES_DIFFICULTY_EVERY = 5;
  private const float PIPE_GAP_SIZE_DECREASES = 2.5f;
  private const float MIN_PIPE_GAP_SIZE = 25f;
  private const float CAMERA_SIZE = 50f;
  private const float SPEED = 30f;

  private float pipeSpawnTimerMax;
  private float pipeSpawnTimer;
  private int pipesSpawned;
  private List<Pipe> pipes;
  private float pipeGapSize;

  private void Awake() {
    pipes = new List<Pipe>();
    pipeSpawnTimerMax = 1.5f;
    pipesSpawned = 0;
    pipeGapSize = 50f;
  }

  private void FixedUpdate() {
    if (!GameHandler.getInstance().isGamePaused) {
      HandlePipeMovement();
      HandlePipeSpawning();
    }
  }

  private void HandlePipeMovement() {
    for (int i = 0;i < pipes.Count;i++) {
      Pipe pipe = pipes[i];

      pipe.Move();

      if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION) {
        pipe.DestroySelf();
        pipes.Remove(pipe);
        i--;
      }
    }
  }

  private void HandlePipeSpawning() {
    pipeSpawnTimer -= Time.deltaTime;

    if (pipeSpawnTimer <= 0) {
      pipeSpawnTimer = pipeSpawnTimerMax;

      float totalHeight = CAMERA_SIZE * 2f;
      float heightEdgeLimit = 5f;
      float minHeight = pipeGapSize / 2f + heightEdgeLimit;
      float maxHeight = totalHeight - pipeGapSize / 2f - heightEdgeLimit;
      float height = UnityEngine.Random.Range(minHeight, maxHeight);

      Debug.Log("Min: " + minHeight + " Max: " + maxHeight + " Height: " + height);

      CreateGapPipes(height, pipeGapSize, PIPE_SPAWN_X_POSITION);
      HandleDifficulty();
    }
  }

  private void HandleDifficulty() {
    if (pipesSpawned % INCREASES_DIFFICULTY_EVERY == 0 && pipeGapSize >= MIN_PIPE_GAP_SIZE + PIPE_GAP_SIZE_DECREASES) {
      pipeGapSize -= PIPE_GAP_SIZE_DECREASES;

      if (pipeSpawnTimerMax * .95f >= .8f) {
        pipeSpawnTimerMax *= .95f;
      } else {
        pipeSpawnTimerMax = .8f;
      }

      Debug.Log("GapSize: " + pipeGapSize + " SpawnTimer: " + pipeSpawnTimerMax);
    }
  }

  private void CreateGapPipes(float gapY, float gapSize, float xPosition) {
    CreatePipe(CAMERA_SIZE * 2f - gapY - gapSize / 2f, xPosition, false);
    CreatePipe(gapY - gapSize / 2f, xPosition);
    pipesSpawned++;
  }

  private void CreatePipe(float height, float xPosition, bool onGround = true) {
    pipes.Add(new Pipe(height, xPosition, onGround));
  }

  private class Pipe {
    private Transform pipeHead;
    private Transform pipeBody;
    private bool isBottom;

    public Pipe(float height, float xPosition, bool isBottom = true) {
      float yPosition = CAMERA_SIZE * (isBottom ? -1 : 1);

      // Head
      Transform pipeHead = Instantiate(GameAssets.GetInstance().pipeHead);
      SpriteRenderer headRenderer = pipeHead.GetComponent<SpriteRenderer>();
      float headH = headRenderer.size.y;
      float headY = isBottom ? yPosition + height - (headH / 2f) : yPosition - height + (headH / 2f);

      // Body
      Transform pipeBody = Instantiate(GameAssets.GetInstance().pipeBody);
      SpriteRenderer bodyRenderer = pipeBody.GetComponent<SpriteRenderer>();
      BoxCollider2D bodyCollider = pipeBody.GetComponent<BoxCollider2D>();
      float bodyW = bodyRenderer.size.x;

      pipeHead.position = new Vector3(xPosition, headY, 0f);

      pipeBody.position = new Vector3(xPosition, yPosition, 0f);

      if (!isBottom) {
        pipeBody.localScale = new Vector3(1, -1, 1);
      }

      bodyRenderer.size = new Vector2(bodyW, height);
      bodyCollider.size = new Vector2(bodyW, height);
      bodyCollider.offset = new Vector2(0, height / 2f);

      this.pipeHead = pipeHead;
      this.pipeBody = pipeBody;
      this.isBottom = isBottom;
    }

    public void Move() {
      pipeHead.position += new Vector3(-1, 0, 0) * SPEED * Time.deltaTime;
      pipeBody.position += new Vector3(-1, 0, 0) * SPEED * Time.deltaTime;
    }

    public float GetXPosition() {
      return pipeHead.position.x;
    }

    public void DestroySelf() {
      Destroy(pipeHead.gameObject);
      Destroy(pipeBody.gameObject);
    }
  }
}
