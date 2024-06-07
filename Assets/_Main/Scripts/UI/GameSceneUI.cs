using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSceneUI : MonoBehaviour
{
    public static GameSceneUI Instance;

    private bool _isLeftButtonHeld, _isRightButtonHeld;
    private Action _onLeftAction, _onRightAction, _stopMoveAction;
 
    public void InitializeController(Action OnLeft, Action OnRight, Action OnUp, Action stopAction)
    {
        UIDocument _uiDocument = GetComponent<UIDocument>();

        VisualElement _controller = _uiDocument.rootVisualElement.Q<VisualElement>("Controller");

        Button _leftButton = _controller.Q<Button>("Left");
        Button _rightButton = _controller.Q<Button>("Right");
        Button _upButton = _controller.Q<Button>("Up");

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