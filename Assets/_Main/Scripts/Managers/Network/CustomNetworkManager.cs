using UnityEngine;
using System.Collections;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public static CustomNetworkManager Instance;

    public const int MAX_PLAYER_COUNT = 2;
    private int _currentPlayers = 0;

    public override void Awake()
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


    public override void Start()
    {
        base.Start();
        _currentPlayers = 0;
    }

    public void CreateHost()
    {
        StartHost();
    }
     
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        _currentPlayers++;

        if (_currentPlayers == MAX_PLAYER_COUNT && NetworkServer.active)
            StartCoroutine(WaitToStartGame());
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        _currentPlayers--;
        StartCoroutine(ReturnToLobby());
    }

    public void JoinHost(string address)
    {
        if (string.IsNullOrEmpty(address))
            networkAddress = "localhost";
        else
            networkAddress = address;

        StartClient(); 
    }

    private IEnumerator WaitToStartGame()
    {
        yield return new WaitForSeconds(2f);
        LoadScene(GameSettings.Instance.SOGameSettings);
    }

    public IEnumerator ReturnMainMenu()
    {
        yield return null;

        if (NetworkServer.active && NetworkClient.isConnected)
            StopHost();
        else if (NetworkClient.isConnected)
            StopClient();

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(2f);
        ServerChangeScene("Lobby");
    }

    private void LoadScene(SO_GameSettings gameSettings)
    {
        switch (gameSettings.CurrentLevel)
        {
            case Level.Level1: ServerChangeScene("Level1"); break;
            case Level.Level2: ServerChangeScene("Level2"); break;
            case Level.Level3: ServerChangeScene("Level3"); break;
            case Level.Level4: ServerChangeScene("Level4"); break;
        }
    }

    public override void OnServerChangeScene(string sceneName)
    {
        base.OnServerChangeScene(sceneName);
        if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level3" || sceneName == "Level4")
            NetworkServer.SpawnObjects();
    }
}