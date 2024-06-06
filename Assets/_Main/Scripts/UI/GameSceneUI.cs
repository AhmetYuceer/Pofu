using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSceneUI : MonoBehaviour
{
    public static GameSceneUI Instance;

    private UIDocument _uiDocument;
    private VisualElement _controller;
    private Button _leftButton, _rightButton, _upButton;

    private bool _isLeftButtonHeld, _isRightButtonHeld, _isInitialize = false;
    private Action _onLeftAction, _onRightAction, _stopMoveAction;

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

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    public void InitializeController(Action OnLeft, Action OnRight, Action OnUp, Action stopAction)
    {
        _controller = _uiDocument.rootVisualElement.Q<VisualElement>("Controller");

        _leftButton = _controller.Q<Button>("Left");
        _rightButton = _controller.Q<Button>("Right");
        _upButton = _controller.Q<Button>("Up");

        _onLeftAction = OnLeft;
        _onRightAction = OnRight;
        _stopMoveAction = stopAction;

        _upButton.clicked += OnUp;

        _leftButton.RegisterCallback<PointerDownEvent>(e =>
        {
            _isLeftButtonHeld = true;
        }, TrickleDown.TrickleDown);

        _leftButton.RegisterCallback<PointerUpEvent>(e =>
        {
            _stopMoveAction?.Invoke();
            _isLeftButtonHeld = false;
        }, TrickleDown.TrickleDown);


        _rightButton.RegisterCallback<PointerDownEvent>(e =>
        {
            _isRightButtonHeld = true;
        }, TrickleDown.TrickleDown);

        _rightButton.RegisterCallback<PointerUpEvent>(e =>
        {
            _stopMoveAction?.Invoke();
            _isRightButtonHeld = false;
        }, TrickleDown.TrickleDown);

        _isInitialize = true;
    }

    private void Update()
    {
        if (_isLeftButtonHeld)
        {
            _onLeftAction?.Invoke();
        }

        if (_isRightButtonHeld)
        {
            _onRightAction?.Invoke();
        }
    }
}