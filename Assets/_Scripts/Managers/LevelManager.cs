using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  private const float GROUND_SPAWN_X_POSITION = 320;

  private const float CLOUD_DESTROY_X_POSITION = -150;
  private const float CLOUD_SPAWN_X_POSITION = 150;

  private const float PIPE_DESTROY_X_POSITION = -100;
  private const float PIPE_GAP_SIZE_DECREASES = 2;
  private const float PIPE_SPAWN_X_POSITION = 100;
  private const float PIPE_GAP_DEFAULT_SIZE = 50;

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
  private List<Cloud> clouds;
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
    clouds = new List<Cloud>();
    pipes = new List<Pipe>();
  }

  private void Start() {
    HandleGroundSpawning();
  }

  private void FixedUpdate() {
    if (!isGamePaused) {
      HandlePipeSpawning();
      HandlePipeMovement();
    }

    if (!isGameStarted || (isGameStarted && !isGamePaused)) {
      HandleGroundMovement();
      HandleCloudSpawning();
      HandleCloudMovement();
    }
  }

  private void HandleGroundSpawning() {
    SpawnGround();
    SpawnGround(GROUND_SPAWN_X_POSITION);
  }

  private void HandleGroundMovement() {
    for (int i = 0; i < grounds.Count; i++) {
      Ground ground = grounds[i];

      ground.Move(LEVEL_SPEED);

      if (ground.x <= GROUND_SPAWN_X_POSITION * -.5f) {
        ground.x = GROUND_SPAWN_X_POSITION;
      }
    }
  }

  private void HandleCloudSpawning() {
    float threshold = Random.Range(0f, 50f);
    int length = clouds.Count;

    if (length == 0 || clouds[length - 1].x <= threshold) {
      float y = Random.Range(0f, 50f);
      clouds.Add(new Cloud(CLOUD_SPAWN_X_POSITION, y));
    }
  }

  private void HandleCloudMovement() {
    for (int i = 0; i < clouds.Count; i++) {
      Cloud cloud = clouds[i];

      cloud.Move(LEVEL_SPEED);

      if (cloud.x <= CLOUD_DESTROY_X_POSITION) {
        cloud.Destroy();
        clouds.Remove(cloud);
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
      float height = Random.Range(minHeight, maxHeight);

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

      //Debug.Log("Gap: " + pipeGapSize + " Speed: " + LEVEL_SPEED + "\nSpawn Timer: " + pipeNextSpawnTimer);
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

  private void SpawnGround(float x = 40) {
    grounds.Add(new Ground(x));
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

  private class Ground {
    private const float GROUND_Y_POSITION = -45;
    private Transform ground;

    public Ground(float x = 50) {
      ground = Instantiate(AssetsManager.Instance.Ground, new Vector3(x, GROUND_Y_POSITION, 0), Quaternion.identity);
    }

    public void Move(float speed) {
      ground.position += speed * Time.deltaTime * new Vector3(-1, 0, 0);
    }

    public float x {
      set => ground.position = new Vector3(value, GROUND_Y_POSITION, 0);
      get => ground.position.x;
    }
  }

  private class Cloud {
    private const float CLOUD_SPEED = .2f;
    private Transform cloud;

    public Cloud(float x = 0, float y = 0) {
      int index = Random.Range(0, 100) % AssetsManager.Instance.Cloud.Count;
      cloud = Instantiate(AssetsManager.Instance.Cloud[index], new Vector3(x, y, 0), Quaternion.identity);
    }

    public void Move(float speed) {
      cloud.position += speed * CLOUD_SPEED * Time.deltaTime * new Vector3(-1, 0, 0);
    }

    public float x {
      get {
        return cloud.position.x;
      }
    }

    public void Destroy() {
      Object.Destroy(cloud.gameObject);
    }
  }

  private class Pipe {
    private Transform pipeHead;
    private Transform pipeBody;

    public Pipe(float height, float xPosition, bool isBottom = true) {
      float yPosition = CAMERA_SIZE * (isBottom ? -1 : 1);

      // Head
      Transform pipeHead = Instantiate(AssetsManager.Instance.PipeHead);
      SpriteRenderer headRenderer = pipeHead.GetComponent<SpriteRenderer>();
      float headH = headRenderer.size.y;
      float headY = isBottom ? yPosition + height - (headH * .5f) : yPosition - height + (headH * .5f);

      // Body
      Transform pipeBody = Instantiate(AssetsManager.Instance.PipeBody);
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
}
