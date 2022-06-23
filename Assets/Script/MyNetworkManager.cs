using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public Dictionary<int, Camera>  playersCamera = new Dictionary<int, Camera>();
    private float[][,] ScreenSizeDependingOnPlayers = new float[4][,];
    public List<int> connectionIds = new List<int>();
    public override void OnStartServer()
    {
        Debug.Log("serverStarted");
        // The values are all in percent
        // Each int has an array like "Screen[2] (3 players) = { {Player 1 Viewport Camera}, {player2}, {Player 3 } }"
        ScreenSizeDependingOnPlayers[0] = new float[,] { { 0f, 0f, 1.0f, 1.0f} };
        ScreenSizeDependingOnPlayers[1] = new float[,] { { 0f, 0f, 0.5f, 1.0f}, { 0.5f, 0f, 0.5f, 1.0f } };
        ScreenSizeDependingOnPlayers[2] = new float[,] { { 0f, 0.5f, 0.5f, 0.5f }, { 0.5f, 0.5f, 0.5f, 0.5f }, {0f, 0f, 1.0f, 0.5f } };
        ScreenSizeDependingOnPlayers[3] = new float[,] { { 0f, 0.5f, 0.5f, 0.5f}, { 0.5f, 0.5f, 0.5f, 0.5f }, { 0f, 0f, 0.5f, 0.5f }, { 0.5f, 0f, 0.5f, 0.5f } };
    }

    public override void OnStopServer()
    {
        Debug.Log("server Stopped");
    }

    public override void OnClientConnect()
    {
        Debug.Log("Connected to server");
    }

    /// <summary>
    /// When we add a player, we want to split the screen just like in a Mario Kart setting.
    /// </summary>
    /// <param name="conn">The connection that has been created between the client and the server.</param>
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Player Added");
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        // instantiating a "Player" prefab gives it the name "Player(clone)"
        // => appending the connectionId is WAY more useful for debugging!
        player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
        playersCamera.Add(conn.connectionId, player.GetComponentInChildren<Camera>());
        connectionIds.Add(conn.connectionId);
        int nbCam = playersCamera.Count;
        for (int i = 0; i<nbCam; i++)
        {
            float[,] cam = ScreenSizeDependingOnPlayers[nbCam - 1];
            playersCamera[connectionIds[i]].rect= new Rect(cam[i, 0], cam[i, 1], cam[i, 2], cam[i, 3]);
        }
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        playersCamera.Remove(conn.connectionId);
        connectionIds.Remove(conn.connectionId);
        base.OnServerDisconnect(conn);
    }
    public override void OnClientDisconnect()
    {
        Debug.Log("Disconnected from server");
    }

}
