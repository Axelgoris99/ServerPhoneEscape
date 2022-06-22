using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHolaCountChanged))]
    int holaCount = 0;
    public float speed = 0.1f;

    void HandleMovement()
    {
        if(isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * speed, moveVertical * speed, 0);
            transform.position += movement;
        }
    }

    private void Update()
    {
        HandleMovement();
        if (isLocalPlayer && Input.GetKeyDown("x"))
        {
            Debug.Log("Sending Hola to Server");
            Hola();
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("Player spawned ");
    }

    [Command]
    void Hola()
    {
        Debug.Log("Received Hola from Client");
        holaCount += 1;
        ReplyHola();
    }

    [TargetRpc]
    void ReplyHola()
    {
        Debug.Log("Reply Hola to Server");
    }

    [ClientRpc]
    void TooHigh()
    {
        Debug.Log("Too High!");
    }

    void OnHolaCountChanged(int oldCount, int newCount)
    {
        Debug.Log($"We had {oldCount} holas and now we have {newCount} holas");
    }
}
