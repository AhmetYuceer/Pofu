using UnityEngine;
using Mirror;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject playerUIPrefab; 
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _moveSpeed = 4.5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckRadius = 0.4f;

    private CinemachineVirtualCamera _virtualCamera;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Collider2D _playerCollider;
    private bool _isGrounded;
    private float _moveDirection;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Coin"))
        {
            CmdCollectCoin(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            CmdTakeDamage();
        } 
    }

    [Command(requiresAuthority = false)]
    private void CmdTakeDamage()
    {
        TakeDamageRPC();
    }

    [ClientRpc]
    private void TakeDamageRPC()
    {
        this.transform.position = Vector3.zero;
    }
   
    [Command]
    private void CmdCollectCoin(GameObject coin)
    {
        GameManager.Instance.CollectCoin(coin);
    }
}