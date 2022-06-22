using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 
public class ReceiveTap : NetworkBehaviour
{
    public struct ScoreMessage : NetworkMessage
    {
        public int score;
    }

    public void SendScore(int score, Vector3 scorePos, int lives)
    {
        ScoreMessage msg = new ScoreMessage()
        {
            score = 42,
        };

        NetworkServer.SendToAll(msg);
    }

    public void SetupClient()
    {
        NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
        NetworkClient.Connect("localhost");
    }

    public void OnScore(ScoreMessage msg)
    {
        Debug.Log("OnScoreMessage " + msg.score);
    }
}
