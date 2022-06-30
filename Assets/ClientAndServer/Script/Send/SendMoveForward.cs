using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SendMoveForward : NetworkBehaviour
{
    public int strenght = 5;

    // Start is called before the first frame update
    private void OnEnable()
    {
        Movement.OnForward += SendTouch;
    }

    void SendTouch(float str)
    {
        if (isLocalPlayer)
        {
            CmdSendTouch(str);
        }
    }
    [Command(requiresAuthority = false)]
    void CmdSendTouch(float str)
    {
        ForwardAction(str);
    }

    [ServerCallback]
    void ForwardAction(float str)
    {
        CharacterController cc = GetComponent<CharacterController>();
        Vector3 forwardMove = transform.up;
        cc.Move(forwardMove * str *  strenght * Time.deltaTime);
        //rb.AddForce(rb.transform.up * strenght, ForceMode.Impulse);
    }
}
