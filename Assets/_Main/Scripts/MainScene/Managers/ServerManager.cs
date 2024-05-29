using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LobbyManager : NetworkManager
{
    public static LobbyManager Instance;
    public List<Server> servers = new List<Server>();

    public override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    public void CreateServer(string serverName)
    {
        // Start the host (server + client)
        StartHost();

        // Create a new server instance and add it to the list
        Server newServer = new Server(serverName, networkAddress);
        servers.Add(newServer);

        Debug.Log("Server created: " + serverName);
    }

    public List<Server> GetServers()
    {
        return servers;
    }
    
    public void UpdateServerList()
    {
        List<Server> servers = GetServers();
        foreach (Server server in servers)
        {
            Debug.Log(server.serverName);
        }
    }

}

public class Server
{
    public string serverName;
    public string address;

    public Server(string serverName, string address)
    {
        this.serverName = serverName;
        this.address = address;
    }
}