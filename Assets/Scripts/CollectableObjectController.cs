using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectController : MonoBehaviour
{
    PlayerManager playerManager;

    [SerializeField] Transform sphere;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();

        sphere = transform.GetChild(0);

        if(GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();

            Rigidbody rb = GetComponent<Rigidbody>();

            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            GetComponent<MeshRenderer>().material = playerManager.collectedMat;
        }
    }

    public void DestroyObject()
    {
        playerManager.collidedList.Remove(gameObject);
        Destroy(gameObject);

        Transform particle = Instantiate(playerManager.particlePrefab, transform.position, Quaternion.identity);
        particle.GetComponent<ParticleSystem>().startColor = playerManager.collectedMat.color;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("CollectibleObj"))
        {
            if (!playerManager.collidedList.Contains(other.gameObject))
            {
                other.gameObject.tag = "CollectedObj";
                other.transform.parent = playerManager.collectedPoolTransform;
                playerManager.collidedList.Add(other.gameObject);
                other.gameObject.AddComponent<CollectableObjectController>();
                //other.gameObject.GetComponent<MeshRenderer>().material = playerManager.collectedMat;
            }
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            DestroyObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollectibleList"))
        {
            other.transform.GetComponent<BoxCollider>().enabled = false;
            other.transform.parent = playerManager.collectedPoolTransform;

            foreach(Transform child in other.transform)
            {
                if (!playerManager.collidedList.Contains(child.gameObject))
                {
                    playerManager.collidedList.Add(child.gameObject);
                    child.gameObject.tag = "CollectedObj";
                    child.gameObject.AddComponent<CollectableObjectController>();
                    
                }
            }
        }

        if (other.gameObject.CompareTag("FinishLine"))
        {
            if(playerManager.levelState != PlayerManager.LevelState.Finished)
            {
                playerManager.levelState = PlayerManager.LevelState.Finished;
                playerManager.CallMakeSphere();
            }
        }

        
    }

    public void MakeSphere()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;


        sphere.gameObject.GetComponent<MeshRenderer>().enabled = true;
        sphere.gameObject.GetComponent<SphereCollider>().enabled = true;
        sphere.gameObject.GetComponent<SphereCollider>().isTrigger = true;

        sphere.gameObject.GetComponent<Renderer>().material = playerManager.collectedMat;
    }

    public void DropObject()
    {
        sphere.gameObject.layer = 8;

        sphere.gameObject.GetComponent<SphereCollider>().isTrigger = false;
        sphere.gameObject.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().useGravity = true;
    }
}
