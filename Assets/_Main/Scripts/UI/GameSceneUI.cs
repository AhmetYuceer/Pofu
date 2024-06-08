using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSceneUI : MonoBehaviour
{
    private bool _isLeftButtonHeld, _isRightButtonHeld;
    private Action _onLeftAction, _onRightAction, _stopMoveAction;

    VisualElement _controller, _backController, _target;
    Button _leftButton, _rightButton, _upButton, _backButton;
    Label _targetText;

    public void InitializeController(Action OnLeft, Action OnRight, Action OnUp, Action stopAction)
    {
        UIDocument _uiDocument = GetComponent<UIDocument>();

        _controller = _uiDocument.rootVisualElement.Q<VisualElement>("Controller");
        _backController = _uiDocument.rootVisualElement.Q<VisualElement>("BackController");
        _target = _uiDocument.rootVisualElement.Q<VisualElement>("Target");

        _leftButton = _controller.Q<Button>("Left");
        _rightButton = _controller.Q<Button>("Right");
        _upButton = _controller.Q<Button>("Up");
        _backButton = _backController.Q<Button>("BackMainMenu");

        _targetText = _target.Q<Label>("TargetText");

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

        _backButton.clicked += () =>
        {
            StartCoroutine(CustomNetworkManager.Instance.ReturnMainMenu());
        };
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