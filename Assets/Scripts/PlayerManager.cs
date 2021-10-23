using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public Material collectedMat;
    public PlayerState playerState;
    public LevelState levelState;

    public Transform particlePrefab;

    public List<GameObject> collidedList;

    public Transform collectedPoolTransform;

    public enum PlayerState
    {
        Stop,
        Move
    }

    public enum LevelState
    {
        NotFinished,
        Finished
    }

    public void CallMakeSphere()
    {
        foreach(GameObject obj in collidedList)
        {
            obj.GetComponent<CollectableObjectController>().MakeSphere();
        }
    }
}
