using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnStartServer()
    {
        Debug.Log("serverStarted");
    }

    public override void OnStopServer()
    {
        Debug.Log("server Stopped");
    }

    public override void OnClientConnect()
    {
        Debug.Log("Connected to server");
    }
    public override void OnClientDisconnect()
    {
        Debug.Log("Disconnected from server");
    }

}
