using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
  private static LevelHandler instance;

  public static LevelHandler getInstance() {
    return instance;
  }

  private const float GROUND_DESTROY_X_POSITION = -250f;
  private const float GROUND_SPAWN_X_POSITION = 240f;

  private const float PIPE_DESTROY_X_POSITION = -100f;
  private const float PIPE_GAP_SIZE_DECREASES = 2.5f;
  private const float PIPE_SPAWN_X_POSITION = 100f;

  private const int INCREASES_DIFFICULTY_EVERY = 5;
  private const float MIN_PIPE_GAP_SIZE = 25f;
  private const float CAMERA_SIZE = 50f;
  private const float SPEED = 30f;

  private float pipeSpawnTimerMax;
  private float pipeSpawnTimer;
  private float pipeGapSize;
  private int pipesSpawned;

  private List<Ground> grounds;
  private List<Pipe> pipes;

  private void Awake() {
    grounds = new List<Ground>();
    pipes = new List<Pipe>();
    pipeSpawnTimerMax = 1.5f;
    pipeGapSize = 50f;
    pipesSpawned = 0;
    instance = this;
  }

  private void FixedUpdate() {
    if (!GameHandler.getInstance().isGamePaused) {
      HandlePipeSpawning();
      HandlePipeMovement();
    }

    if (!GameHandler.getInstance().isGameOver) {
      HandleGroundSpawning();
      HandleGroundMovement();
    }
  }

  private void HandleGroundSpawning() {
    int length = grounds.Count;

    if (length < 1) {
      SpawnGround();
    } else if (grounds[length - 1].x <= CAMERA_SIZE * -1) {
      SpawnGround(GROUND_SPAWN_X_POSITION);
    }
  }

  private void HandleGroundMovement() {
    for (int i = 0;i < grounds.Count;i++) {
      Ground ground = grounds[i];

      ground.move();

      if (ground.x < GROUND_DESTROY_X_POSITION) {
        ground.destroy();
        grounds.Remove(ground);
        i--;
      }
    }
  }

  private void HandlePipeSpawning() {
    pipeSpawnTimer -= Time.deltaTime;

    if (pipeSpawnTimer <= 0) {
      pipeSpawnTimer = pipeSpawnTimerMax;

      float totalHeight = CAMERA_SIZE * 2;
      float heightEdgeLimit = 15;
      float minHeight = pipeGapSize / 2 + heightEdgeLimit;
      float maxHeight = totalHeight - pipeGapSize / 2 - heightEdgeLimit;
      float height = UnityEngine.Random.Range(minHeight, maxHeight);

      //Debug.Log("Min: " + minHeight + " Max: " + maxHeight + " Height: " + height);

      CreateGapPipes(height, pipeGapSize, PIPE_SPAWN_X_POSITION);
      HandleDifficulty();
    }
  }

  private void HandlePipeMovement() {
    for (int i = 0;i < pipes.Count;i++) {
      Pipe pipe = pipes[i];

      pipe.move();

      if (pipe.x < PIPE_DESTROY_X_POSITION) {
        pipe.destroy();
        pipes.Remove(pipe);
        i--;
      }
    }
  }

  private void HandleDifficulty() {
    if (pipesSpawned % INCREASES_DIFFICULTY_EVERY == 0 && pipeGapSize >= MIN_PIPE_GAP_SIZE + PIPE_GAP_SIZE_DECREASES) {
      pipeGapSize -= PIPE_GAP_SIZE_DECREASES;

      float newSpawnTimer = pipeSpawnTimerMax * .95f;

      if (newSpawnTimer >= .8f) {
        pipeSpawnTimerMax = newSpawnTimer;
      } else {
        pipeSpawnTimerMax = .8f;
      }
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

  private void SpawnGround(float x = 50, float y = -45.5f) {
    grounds.Add(new Ground(x, y));
  }

  private class Pipe {
    private Transform pipeHead;
    private Transform pipeBody;

    public Pipe(float height, float xPosition, bool isBottom = true) {
      float yPosition = CAMERA_SIZE * (isBottom ? -1 : 1);

      // Head
      Transform pipeHead = Instantiate(GameAssets.getInstance().pipeHead);
      SpriteRenderer headRenderer = pipeHead.GetComponent<SpriteRenderer>();
      float headH = headRenderer.size.y;
      float headY = isBottom ? yPosition + height - (headH / 2f) : yPosition - height + (headH / 2f);

      // Body
      Transform pipeBody = Instantiate(GameAssets.getInstance().pipeBody);
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
    }

    public void move() {
      pipeHead.position += new Vector3(-1, 0, 0) * SPEED * Time.deltaTime;
      pipeBody.position += new Vector3(-1, 0, 0) * SPEED * Time.deltaTime;
    }

    public float x {
      get {
        return pipeHead.position.x;
      }
    }

    public void destroy() {
      Destroy(pipeHead.gameObject);
      Destroy(pipeBody.gameObject);
    }
  }

  private class Ground {
    private Transform ground;

    public Ground(float x = 50, float y = -45.5f) {
      Transform ground = Instantiate(GameAssets.getInstance().ground);

      ground.position = new Vector3(x, y, 0);

      this.ground = ground;
    }

    public void move() {
      ground.position += new Vector3(-1, 0, 0) * SPEED * Time.deltaTime;
    }

    public float x {
      get {
        return ground.position.x;
      }
    }

    public void destroy() {
      Destroy(ground.gameObject);
    }
  }
}
