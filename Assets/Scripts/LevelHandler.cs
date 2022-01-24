using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {
  private const float GROUND_SPAWN_TEST_POSITION = -32;
  private const float GROUND_DESTROY_X_POSITION = -224;
  private const float GROUND_SPAWN_X_POSITION = 215;

  private const float PIPE_DESTROY_X_POSITION = -100;
  private const float PIPE_GAP_SIZE_DECREASES = 2;
  private const float PIPE_GAP_DEFAULT_SIZE = 50;
  private const float PIPE_SPAWN_X_POSITION = 100;

  private const int INCREASES_DIFFICULTY_EVERY_X_PIPES = 5;
  private const float MAX_PIPE_SPAWN_TIMER = 2;
  private const float MIN_PIPE_SPAWN_TIMER = 1;
  private const float MIN_PIPE_GAP_SIZE = 25;
  private const float CAMERA_SIZE = 50;
  private const float LEVEL_SPEED = 30;

  private float pipeNextSpawnTimer;
  private float pipeSpawnTimer;
  private float pipeGapSize;
  private int pipesSpawned;

  private bool isGameStarted = false;
  private bool isGamePaused = true;

  private List<Ground> grounds;
  private List<Pipe> pipes;

  private void OnEnable() {
    Actions.OnGameStart += OnGameStart;
    Actions.OnGamePause += OnGamePause;
  }

  private void OnDisable() {
    Actions.OnGameStart -= OnGameStart;
    Actions.OnGamePause -= OnGamePause;
  }

  private void Awake() {
    grounds = new List<Ground>();
    pipes = new List<Pipe>();
  }

  private void FixedUpdate() {
    if (!isGamePaused) {
      HandlePipeSpawning();
      HandlePipeMovement();
    }

    if (!isGameStarted || (isGameStarted && !isGamePaused)) {
      HandleGroundSpawning();
      HandleGroundMovement();
    }
  }

  private void HandleGroundSpawning() {
    int length = grounds.Count;

    if (length >= 2) return;

    if (length < 1) {
      SpawnGround();
    } else if (grounds[length - 1].x <= GROUND_SPAWN_TEST_POSITION) {
      SpawnGround(GROUND_SPAWN_X_POSITION);
    }
  }

  private void HandleGroundMovement() {
    for (int i = 0; i < grounds.Count; i++) {
      Ground ground = grounds[i];

      ground.Move(LEVEL_SPEED);

      if (ground.x <= GROUND_DESTROY_X_POSITION) {
        ground.Destroy();
        grounds.Remove(ground);
        i--;
      }
    }
  }

  private void HandlePipeSpawning() {
    pipeSpawnTimer -= Time.deltaTime;

    if (pipeSpawnTimer <= 0) {
      pipeSpawnTimer = pipeNextSpawnTimer;

      float totalHeight = CAMERA_SIZE * 2;
      float heightEdgeLimit = 15;
      float minHeight = pipeGapSize * .5f + heightEdgeLimit;
      float maxHeight = totalHeight - pipeGapSize * .5f - heightEdgeLimit;
      float height = UnityEngine.Random.Range(minHeight, maxHeight);

      CreateGapPipes(height, pipeGapSize, PIPE_SPAWN_X_POSITION);
      HandleDifficulty();
    }
  }

  private void HandlePipeMovement() {
    for (int i = 0; i < pipes.Count; i++) {
      Pipe pipe = pipes[i];

      pipe.Move(LEVEL_SPEED);

      if (pipe.x <= PIPE_DESTROY_X_POSITION) {
        pipe.Destroy();
        pipes.Remove(pipe);
        i--;
      }
    }
  }

  private void HandleDifficulty() {
    if (pipesSpawned % INCREASES_DIFFICULTY_EVERY_X_PIPES == 0 && pipeGapSize >= MIN_PIPE_GAP_SIZE + PIPE_GAP_SIZE_DECREASES) {
      pipeGapSize -= PIPE_GAP_SIZE_DECREASES;

      float newSpawnTimer = pipeNextSpawnTimer * .95f;

      if (newSpawnTimer >= MIN_PIPE_SPAWN_TIMER) {
        pipeNextSpawnTimer = newSpawnTimer;
      } else {
        pipeNextSpawnTimer = MIN_PIPE_SPAWN_TIMER;
      }

      Debug.Log("Gap: " + pipeGapSize + " Speed: " + LEVEL_SPEED + "\nSpawn Timer: " + pipeNextSpawnTimer);
    }
  }

  private void CreateGapPipes(float gapY, float gapSize, float xPosition) {
    CreatePipe((CAMERA_SIZE * 2f) - gapY - (gapSize * .5f), xPosition, false);
    CreatePipe(gapY - (gapSize * .5f), xPosition);
    pipesSpawned++;
  }

  private void CreatePipe(float height, float xPosition, bool onGround = true) {
    pipes.Add(new Pipe(height, xPosition, onGround));
  }

  private void SpawnGround(float x = 40, float y = -45f) {
    grounds.Add(new Ground(x, y));
  }

  private void OnGameStart() {
    DestroyPipes();

    pipes = new List<Pipe>();

    pipeNextSpawnTimer = MAX_PIPE_SPAWN_TIMER;
    pipeGapSize = PIPE_GAP_DEFAULT_SIZE;
    isGameStarted = true;
    pipesSpawned = 0;
  }

  private void OnGamePause(bool isPaused) {
    isGamePaused = isPaused;
  }

  private void DestroyPipes() {
    int i = pipes.Count - 1;

    while (i >= 0) {
      Pipe pipe = pipes[i];

      pipe.Destroy();
      pipes.Remove(pipe);

      i--;

    }
  }

  private class Pipe {
    private Transform pipeHead;
    private Transform pipeBody;

    public Pipe(float height, float xPosition, bool isBottom = true) {
      float yPosition = CAMERA_SIZE * (isBottom ? -1 : 1);

      // Head
      Transform pipeHead = Instantiate(GameAssets.Instance.PipeHead);
      SpriteRenderer headRenderer = pipeHead.GetComponent<SpriteRenderer>();
      float headH = headRenderer.size.y;
      float headY = isBottom ? yPosition + height - (headH * .5f) : yPosition - height + (headH * .5f);

      // Body
      Transform pipeBody = Instantiate(GameAssets.Instance.PipeBody);
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
      bodyCollider.offset = new Vector2(0, height * .5f);

      this.pipeHead = pipeHead;
      this.pipeBody = pipeBody;
    }

    public void Move(float speed) {
      pipeHead.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
      pipeBody.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
    }

    public float x {
      get {
        return pipeHead.position.x;
      }
    }

    public void Destroy() {
      Object.Destroy(pipeHead.gameObject);
      Object.Destroy(pipeBody.gameObject);
    }
  }

  private class Ground {
    private Transform ground;

    public Ground(float x = 50, float y = -45.5f) {
      Transform ground = Instantiate(GameAssets.Instance.Ground);

      ground.position = new Vector3(x, y, 0);

      this.ground = ground;
    }

    public void Move(float speed) {
      ground.position += speed * Time.deltaTime * new Vector3(-1, 0, 0);
    }

    public float x {
      get {
        return ground.position.x;
      }
    }

    public void Destroy() {
      Object.Destroy(ground.gameObject);
    }
  }
}
