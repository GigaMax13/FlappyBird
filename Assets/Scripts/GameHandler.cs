using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {
  public bool debug = false;

  void Start() {
    Debug.Log("GameHandler.Start");

    //GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));
    //gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHeadSprite;

    if (debug) {
      int count = 0;

      FunctionPeriodic.Create(() => {
        CMDebug.TextPopupMouse("Ding! " + count);
        count++;
      }, .300f);
    }
  }
}
