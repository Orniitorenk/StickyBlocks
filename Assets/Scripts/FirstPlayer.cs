using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    private Rigidbody rb;

    [SerializeField] bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Renderer>().material = playerManager.collectedMat;

        playerManager.collidedList.Add(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Grounded();
        }
    }

    void Grounded()
    {
        isGrounded = true;
        playerManager.playerState = PlayerManager.PlayerState.Move;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        Destroy(this, 1);
    }
}
