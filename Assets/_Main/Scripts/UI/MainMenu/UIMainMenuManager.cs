using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _hostMenu, _clientMenu, _mainMenu;

    //Host Menu
    private Button _hostButton, _hostBackButton;
    private DropdownField _levelDropdown;
    private List<string> dropdownOptions = new List<string> { "Level 1", "Level 2", "Level 3", "Level 4"};

    //Client Menu
    private Button _clientButton, _clientBackButton;
    private TextField _addressField;
    
    //Main Menu
    private Button _showHostMenu, _showClientMenu, _exitButton;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        InitializeMainMenu();
        InitializeHostMenu();
        InitializeClientMenu();

        _clientMenu.style.display = DisplayStyle.None;
        _hostMenu.style.display = DisplayStyle.None;
        _mainMenu.style.display = DisplayStyle.Flex;

        _levelDropdown.choices = dropdownOptions;
        _levelDropdown.value = dropdownOptions[0];
    }

    private void InitializeMainMenu()
    {
        _mainMenu = _uiDocument.rootVisualElement.Q<VisualElement>("MainMenu");

        _showHostMenu = _mainMenu.Q<Button>("HostMenuButton");
        _showClientMenu = _mainMenu.Q<Button>("ClientMenuButton");
        _exitButton = _mainMenu.Q<Button>("ExitButton");

        _showHostMenu.clicked += () => 
        { 
            _hostMenu.style.display = DisplayStyle.Flex;
            _mainMenu.style.display = DisplayStyle.None;
        };

        _showClientMenu.clicked += () =>
        {
            _mainMenu.style.display = DisplayStyle.None;
            _hostMenu.style.display = DisplayStyle.None;
            _clientMenu.style.display = DisplayStyle.Flex;
        };
 
        _exitButton.clicked += () =>
        {
            Application.Quit();
        };
    }

    private void InitializeHostMenu()
    {
        _hostMenu = _uiDocument.rootVisualElement.Q<VisualElement>("HostMenu");

        _hostButton = _hostMenu.Q<Button>("CreateLobby");
        _hostBackButton = _hostMenu.Q<Button>("BackMenu");
        _levelDropdown = _hostMenu.Q<DropdownField>("SelectedLevel");

        _hostButton.clicked += () =>
        {
            CustomNetworkManager.Instance.CreateHost();
        };

        _hostBackButton.clicked += () =>
        {
            _clientMenu.style.display = DisplayStyle.None;
            _hostMenu.style.display = DisplayStyle.None;
            _mainMenu.style.display = DisplayStyle.Flex;
        };

        _levelDropdown.RegisterValueChangedCallback(OnDropdownValueChanged);
    }
    private void OnDropdownValueChanged(ChangeEvent<string> evt)
    {
        if (evt.newValue == dropdownOptions[0])
            GameSettings.Instance.SOGameSettings.CurrentLevel = Level.Level1;
        else if (evt.newValue == dropdownOptions[1])
            GameSettings.Instance.SOGameSettings.CurrentLevel = Level.Level2;
        else if (evt.newValue == dropdownOptions[2])
            GameSettings.Instance.SOGameSettings.CurrentLevel = Level.Level3;
        else if (evt.newValue == dropdownOptions[3])
            GameSettings.Instance.SOGameSettings.CurrentLevel = Level.Level4;
    }

    private void InitializeClientMenu()
    {
        _clientMenu = _uiDocument.rootVisualElement.Q<VisualElement>("ClientMenu");

        _addressField = _clientMenu.Q<TextField>("AddressField");
        _clientButton = _clientMenu.Q<Button>("JoinLobby");
        _clientBackButton = _clientMenu.Q<Button>("BackMenu");

        _clientButton.clicked += () =>
        {
            CustomNetworkManager.Instance.JoinHost(_addressField.value);
        };

        _clientBackButton.clicked += () =>
        {
            _clientMenu.style.display = DisplayStyle.None;
            _mainMenu.style.display = DisplayStyle.Flex;
        };
    }
  
}