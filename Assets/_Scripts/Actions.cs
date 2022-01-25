using System;

public static class Actions {
  public static Action<int> OnPlayerScore;
  public static Action<bool> OnGamePause;
  public static Action OnGameStart;
  public static Action OnGameOver;
}
