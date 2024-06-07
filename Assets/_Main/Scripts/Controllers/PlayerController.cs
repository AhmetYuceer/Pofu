using UnityEngine;
using Mirror;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 4.5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadius = 0.4f;
    [SerializeField] private Transform _groundCheck;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private Rigidbody2D _rb;
    private Animator _animator;
    private float _moveDirection;
    private Collider2D _playerCollider;

    private CinemachineVirtualCamera _virtualCamera;

    public GameObject playerUIPrefab; 

    void Start()
    {
        if (!isLocalPlayer)
        {
            this.enabled = false;
            return;
        }

        _groundCheck = transform.GetChild(0).transform;

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerCollider = GetComponent<CircleCollider2D>();
        
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        _virtualCamera.LookAt = transform;
        _virtualCamera.Follow = transform;

        //GameSceneUI.Instance.InitializeController(LeftMove, RightMove, Jump, StopMove);
        GameSceneUI playerUI = Instantiate(playerUIPrefab).GetComponent<GameSceneUI>();
        playerUI.InitializeController(LeftMove, RightMove, Jump, StopMove);
    }
     
    void FixedUpdate()
    {
        CheckGrounded();
        MoveCharacter();
    }

    private void RightMove()
    {
        _moveDirection = 1;
        FlipCharacter();
        AnimateCharacter();
    }

    private void LeftMove()
    {
        _moveDirection = -1;
        FlipCharacter();
        AnimateCharacter();
    }

    private void StopMove()
    {
        _moveDirection = 0;
        AnimateCharacter();
    }

    private void MoveCharacter()
    {
        Vector2 velocity = _rb.velocity;
        velocity.x = _moveDirection * _moveSpeed;
        _rb.velocity = velocity;
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }

    private void FlipCharacter()
    {
        if (_moveDirection > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveDirection < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    private void CheckGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundCheckRadius, _groundLayer);

        _isGrounded = false;
        foreach (var collider in colliders)
        {
            if (collider != _playerCollider)
            {
                _isGrounded = true;
                break;
            }
        }
    }

    private void AnimateCharacter()
    {
        _animator.SetBool("Idle", _moveDirection == 0);
        _animator.SetBool("Run", _moveDirection != 0);
    }
}