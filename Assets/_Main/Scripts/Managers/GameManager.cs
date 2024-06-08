using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    private int _totalCoins = 0;
    private List<PlayerController> players = new List<PlayerController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
     
    [Server]
    public void CollectCoin(GameObject coin)
    {
        _totalCoins++;
        NetworkServer.Destroy(coin);
    } 

    [Server]
    public void EndGame(PlayerController player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);

            if (players.Count == 2)
                StartCoroutine(CustomNetworkManager.Instance.ReturnMainMenu());
        }
    }
}