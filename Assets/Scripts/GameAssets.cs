using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {
  private static GameAssets instance;

  public static GameAssets Instance {
    get {
      return instance;
    }
  }

  [SerializeField]
  private Transform pipeHead;
  [SerializeField]
  private Transform pipeBody;
  [SerializeField]
  private Transform ground;
  [SerializeField]
  private Transform cloud1;
  [SerializeField]
  private Transform cloud2;
  [SerializeField]
  private Transform cloud3;

  private List<Transform> cloud;

  private void Awake() {
    cloud = new List<Transform>() {
      cloud1,
      cloud2,
      cloud3
    };
    instance = this;
  }

  public Transform PipeHead {
    get {
      return pipeHead;
    }
  }

  public Transform PipeBody {
    get {
      return pipeBody;
    }
  }

  public Transform Ground {
    get {
      return ground;
    }
  }

  public List<Transform> Cloud {
    get {
      return cloud;
    }
  }
}
