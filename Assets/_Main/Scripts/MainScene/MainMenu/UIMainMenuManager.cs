using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Mirror;

public class UIMainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _hostMenu;

    //PlayButtons 
    private Button _hostButton, _clientButton;
    private TextField _hostName;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        InitializeHostMenu();
    }

    private void InitializeHostMenu()
    {
        _hostMenu = _uiDocument.rootVisualElement.Q<VisualElement>("HostMenu");
        _hostButton = _hostMenu.Q<Button>("CreateLobby");
        _clientButton = _hostMenu.Q<Button>("BackMenu");
        _hostName = _hostMenu.Q<TextField>("ServerName");

        _hostButton.clicked += CreateServer;
        _clientButton.clicked += ShowServers;
    }

    private void CreateServer()
    {
        LobbyManager.Instance.CreateServer("a");
    }

    private void ShowServers()
    {
        LobbyManager.Instance.UpdateServerList();
    }
}