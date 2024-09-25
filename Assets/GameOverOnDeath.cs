using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOnDeath : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }
    public void CubKilled()
    {
        player.GetComponent<DeathManager>().StartDeathCountdown(DeathManager.DeathReason.CubDeath);
    }
}
