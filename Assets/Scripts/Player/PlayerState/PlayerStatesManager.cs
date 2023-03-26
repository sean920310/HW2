using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatesManager : MonoBehaviour
{
    Animator _playerAnimator;
    Transform _playerTransform;
    Rigidbody2D _playerRigidbody;

    PlayerBaseState _currentState = null;
    PlayerStateFactory _factory = null;

    private bool _isJumpPress;
    private bool _isJumpRelease;
    private bool _isMovePress;
    private bool _isCrouchPress;
    private bool _isCrouchRelease;
    private bool _isJumpDownPress;
    private Vector2 _lastMove = Vector2.zero;
    private int _jumpCountsLeft;
    private float _jumpTimeCounter;
    bool _canSlide; // affect by cool down

    [Header("Move")]
    [SerializeField] float _playerMoveSpeed;
    [SerializeField] float _playerCrouchMoveSpeed;

    [Header("Slide")]
    [SerializeField] float _playerSlideForce;
    [SerializeField] float _playerSlideCancelRate;
    [SerializeField] float _playerSlideCoolDownTime;

    [Header("Jump")]
    [SerializeField] float _jumpHeight;
    [SerializeField] float _normalGravityScale;
    [SerializeField] float _fallGravityScale;
    [SerializeField] int _jumpCounts;
    [SerializeField] float _jumpTime; // how long will jump force be cancel
    [SerializeField] float _jumpCancelRate;
    [SerializeField] Vector2 _groundCheckBoxSize;
    [SerializeField] LayerMask _whatIsGround;
    [SerializeField] LayerMask _whatIsOneWayPlatform;

    [Header("Wall Slide and Wall Jump")]
    [SerializeField] float _wallSlideSpeed;
    [SerializeField] float _wallCheckDistance;
    [SerializeField] float _wallJumpForce;
    [SerializeField] Vector2 _wallJumpDirection;

    public Animator PlayerAnimator { get => _playerAnimator; set => _playerAnimator = value; }
    public Rigidbody2D PlayerRigidbody { get => _playerRigidbody; set => _playerRigidbody = value; }
    public Transform PlayerTransform { get => _playerTransform; set => _playerTransform = value; }
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public bool IsJumpPress { get => _isJumpPress; set => _isJumpPress = value; }
    public bool IsJumpRelease { get => _isJumpRelease; set => _isJumpRelease = value; }
    public bool IsMovePress { get => _isMovePress; set => _isMovePress = value; }
    public bool IsCrouchPress { get => _isCrouchPress; set => _isCrouchPress = value; }
    public bool IsCrouchRelease { get => _isCrouchRelease; set => _isCrouchRelease = value; }
    public bool IsJumpDownPress { get => _isJumpDownPress; set => _isJumpDownPress = value; }
    public Vector2 LastMove { get => _lastMove; set => _lastMove = value; }
    public int JumpCountsLeft { get => _jumpCountsLeft; set => _jumpCountsLeft = value; }
    public float JumpTimeCounter { get => _jumpTimeCounter; set => _jumpTimeCounter = value; }
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set => _playerMoveSpeed = value; }
    public float PlayerCrouchMoveSpeed { get => _playerCrouchMoveSpeed; set => _playerCrouchMoveSpeed = value; }
    public float PlayerSlideForce { get => _playerSlideForce; set => _playerSlideForce = value; }
    public float PlayerSlideCancelRate { get => _playerSlideCancelRate; set => _playerSlideCancelRate = value; }
    public bool CanSlide { get => _canSlide; set => _canSlide = value; }
    public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }
    public float PlayerSlideCoolDownTime { get => _playerSlideCoolDownTime; set => _playerSlideCoolDownTime = value; }
    public float NormalGravityScale { get => _normalGravityScale; set => _normalGravityScale = value; }
    public float FallGravityScale { get => _fallGravityScale; set => _fallGravityScale = value; }
    public int JumpCounts { get => _jumpCounts; set => _jumpCounts = value; }
    public float JumpTime { get => _jumpTime; set => _jumpTime = value; }
    public float JumpCancelRate { get => _jumpCancelRate; set => _jumpCancelRate = value; }
    public LayerMask WhatIsGround { get => _whatIsGround; set => _whatIsGround = value; }
    public LayerMask WhatIsOneWayPlatform { get => _whatIsOneWayPlatform; set => _whatIsOneWayPlatform = value; }
    public float WallSlideSpeed { get => _wallSlideSpeed; set => _wallSlideSpeed = value; }
    public float WallJumpForce { get => _wallJumpForce; set => _wallJumpForce = value; }
    public Vector2 WallJumpDirection { get => _wallJumpDirection; set => _wallJumpDirection = value; }

    #region readonly inspector
    [ReadOnly]
    [SerializeField]
    private string CurrentStateString;
    #endregion

    private void Start()
    {
        // get component
        _playerAnimator = GetComponent<Animator>();
        PlayerTransform = GetComponent<Transform>();
        _playerRigidbody = GetComponent<Rigidbody2D>();

        // state setup
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Idle(); // Initial State
        _currentState.EnterState();

        // variable initialize
        JumpCountsLeft = JumpCounts;
        _wallJumpDirection = _wallJumpDirection.normalized;
        CanSlide = true;
    }

    private void Update()
    {
        _currentState.UpdateState();

        CurrentStateString = _currentState.ToString();
    }
    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();

    }

    public void SwitchState(PlayerBaseState newState)
    {
        _currentState.ExitState();

        newState.EnterState();

        this.CurrentState = newState;
    }

    #region useful function
    public void startCorutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    public float facingDirection()
    {
        return transform.rotation.eulerAngles.y == 0 ? 1f : -1f;
    }
    public bool CheckOnGround()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(0.0f, 0.02f, 0.0f), _groundCheckBoxSize, 0, WhatIsGround);
    }
    public bool CheckOnOneWayPlatform()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(0.0f, 0.02f, 0.0f), _groundCheckBoxSize, 0, WhatIsOneWayPlatform);
    }
    public bool CheckIsTouchingWall()
    {
        return Physics2D.Raycast(transform.position, transform.right, _wallCheckDistance, WhatIsGround);
    }
    public bool canJump()
    {
        return CheckOnGround() || (JumpCountsLeft > 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.7f);
        Gizmos.DrawCube(transform.position - new Vector3(0.0f, 0.02f, 0.0f), _groundCheckBoxSize);
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + _wallCheckDistance, transform.position.y, transform.position.z));
    }

    public void FacingRight()
    {
        PlayerTransform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
    }
    public void FacingLeft()
    {
        PlayerTransform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }
    #endregion

    #region input callback
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            _isJumpPress = true;
            _isJumpRelease = false;
        }
        if (ctx.canceled)
        {
            _isJumpPress = false;
            _isJumpRelease = true;
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _isMovePress = true;

            LastMove = Vector2.right * ctx.ReadValue<float>();
        }
        if (ctx.canceled)
        {
            _isMovePress = false;
            LastMove = Vector2.right * ctx.ReadValue<float>();
        }
    }
    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            IsCrouchPress = true;
            IsCrouchRelease = false;
        }
        if (ctx.canceled)
        {
            IsCrouchPress = false;
            IsCrouchRelease = true;
        }
    }
    public void OnJumpDown(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _isJumpDownPress = true;
        }
        if (ctx.canceled)
        {
            _isJumpDownPress = false;
        }
    }
    #endregion
}
