using System;
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
    BoxCollider2D _playerBoxCollider;

    PlayerBaseState _currentState = null;
    PlayerStateFactory _factory = null;

    private bool _isJumpPress;
    private bool _isJumpRelease;
    private bool _isMovePress;
    private bool _isCrouchPress;
    private bool _isCrouchRelease;
    private bool _isJumpDownPress;
    private bool _isAttackPress;
    private bool _isBlockingPress;
    private Vector2 _lastMove = Vector2.zero;
    private int _jumpCountsLeft;
    private float _jumpTimeCounter;
    bool _canSlide; // affect by cool down
    float _wallJumpStiffTimeCounter;
    int _AttackCount;
    bool _canAttack; // affect by cool down

    [Header("Move")]
    [SerializeField] float _playerMoveSpeed;
    [SerializeField] float _playerMaxSpeedX;
    [SerializeField] float _playerCrouchMoveSpeed;
    [SerializeField] Vector2 _normalColliderSize;
    [SerializeField] Vector2 _normalColliderShift;
    [SerializeField] bool _showNormalColliderGizmos;
    public PhysicsMaterial2D NormalPhysics;
    public AudioSource FootStepSound;

    [Header("Slide")]
    [SerializeField] float _playerSlideForce;
    [SerializeField] float _playerSlideCoolDownTime;
    [SerializeField] bool _isAirSlideEnable;
    [SerializeField] Vector2 _slideColliderSize;
    [SerializeField] Vector2 _slideColliderShift;
    [SerializeField] bool _showSlideColliderGizmos;
    public PhysicsMaterial2D SlidePhysics;

    [Header("Jump")]
    [SerializeField] float _jumpHeight;
    [SerializeField] float _normalGravityScale;
    [SerializeField] float _fallGravityScale;
    [SerializeField] int _jumpCounts;
    [SerializeField] float _jumpTime; // how long will jump force be cancel
    [SerializeField] float _jumpCancelRate;
    [SerializeField] Vector2 _groundCheckBoxSize;
    [SerializeField] Vector2 _groundCheckBoxShift;
    [SerializeField] bool _showGroundCheckBoxGizmos;
    [SerializeField] LayerMask _whatIsGround;
    [SerializeField] LayerMask _whatIsOneWayPlatform;

    [Header("Wall Slide and Wall Jump")]
    [SerializeField] float _wallSlideSpeed;
    [SerializeField] float _wallCheckDistance;
    [SerializeField] bool _showCheckDistanceGizmos;
    [SerializeField] float _wallJumpForce;
    [SerializeField] Vector2 _wallJumpDirection;
    [SerializeField] float _wallJumpStiffTime;

    [Header("Attack")]
    [SerializeField] GameObject _weapon;
    [SerializeField] GameObject _attackRange;
    [SerializeField] float _attackCoolDownTime;

    public Animator PlayerAnimator { get => _playerAnimator; set => _playerAnimator = value; }
    public Rigidbody2D PlayerRigidbody { get => _playerRigidbody; set => _playerRigidbody = value; }
    public Transform PlayerTransform { get => _playerTransform; set => _playerTransform = value; }
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public BoxCollider2D PlayerBoxCollider { get => _playerBoxCollider; set => _playerBoxCollider = value; }
    public bool IsJumpPress { get => _isJumpPress; set => _isJumpPress = value; }
    public bool IsJumpRelease { get => _isJumpRelease; set => _isJumpRelease = value; }
    public bool IsMovePress { get => _isMovePress; set => _isMovePress = value; }
    public bool IsCrouchPress { get => _isCrouchPress; set => _isCrouchPress = value; }
    public bool IsCrouchRelease { get => _isCrouchRelease; set => _isCrouchRelease = value; }
    public bool IsJumpDownPress { get => _isJumpDownPress; set => _isJumpDownPress = value; }
    public bool IsAttackPress { get => _isAttackPress; set => _isAttackPress = value; }
    public Vector2 LastMove { get => _lastMove; set => _lastMove = value; }
    public int JumpCountsLeft { get => _jumpCountsLeft; set => _jumpCountsLeft = value; }
    public float JumpTimeCounter { get => _jumpTimeCounter; set => _jumpTimeCounter = value; }
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set => _playerMoveSpeed = value; }
    public float PlayerCrouchMoveSpeed { get => _playerCrouchMoveSpeed; set => _playerCrouchMoveSpeed = value; }
    public float PlayerSlideForce { get => _playerSlideForce; set => _playerSlideForce = value; }
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
    public Vector2 NormalColliderSize { get => _normalColliderSize; set => _normalColliderSize = value; }
    public Vector2 NormalColliderShift { get => _normalColliderShift; set => _normalColliderShift = value; }
    public Vector2 SlideColliderSize { get => _slideColliderSize; set => _slideColliderSize = value; }
    public Vector2 SlideColliderShift { get => _slideColliderShift; set => _slideColliderShift = value; }
    public GameObject AttackRange { get => _attackRange; set => _attackRange = value; }
    public float WallJumpStiffTimeCounter { get => _wallJumpStiffTimeCounter; set => _wallJumpStiffTimeCounter = value; }
    public float WallJumpStiffTime { get => _wallJumpStiffTime; set => _wallJumpStiffTime = value; }
    public float PlayerMaxSpeedX { get => _playerMaxSpeedX; set => _playerMaxSpeedX = value; }
    public int AttackCount { get => _AttackCount; set => _AttackCount = value; }
    public GameObject Weapon { get => _weapon; set => _weapon = value; }
    public float AttackCoolDownTime { get => _attackCoolDownTime; set => _attackCoolDownTime = value; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public bool IsBlockingPress { get => _isBlockingPress; set => _isBlockingPress = value; }
    public bool IsAirSlideEnable { get => _isAirSlideEnable; set => _isAirSlideEnable = value; }

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
        PlayerBoxCollider = GetComponent<BoxCollider2D>();

        // state setup
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Idle(); // Initial State
        _currentState.EnterState();

        // variable initialize
        JumpCountsLeft = JumpCounts;
        _wallJumpDirection = _wallJumpDirection.normalized;
        CanSlide = true;
        _canAttack = true;
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
    public float getAnimationLength(string name)
    {
        AnimationClip[] clips = PlayerAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if(clip.name == name)
            {
                return clip.length;
            }
        }
        return 0;
    }
    public void MoveWithLimit() 
    { 
        PlayerRigidbody.AddForce(LastMove * PlayerMoveSpeed, ForceMode2D.Force);
        PlayerRigidbody.velocity = new Vector2(Mathf.Clamp(PlayerRigidbody.velocity.x, -PlayerMaxSpeedX, PlayerMaxSpeedX), PlayerRigidbody.velocity.y);
    }
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
        return Physics2D.OverlapBox(transform.position + new Vector3(_groundCheckBoxShift.x, _groundCheckBoxShift.y, transform.position.z), _groundCheckBoxSize, 0, WhatIsGround);
    }
    public bool CheckOnOneWayPlatform()
    {
        return Physics2D.OverlapBox(transform.position + new Vector3(_groundCheckBoxShift.x, _groundCheckBoxShift.y, transform.position.z), _groundCheckBoxSize, 0, WhatIsOneWayPlatform);
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
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
        if (_showGroundCheckBoxGizmos)
        {
            Gizmos.DrawCube(transform.position + new Vector3(_groundCheckBoxShift.x, _groundCheckBoxShift.y, 0.0f), _groundCheckBoxSize);
        }
        if (_showCheckDistanceGizmos)
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + _wallCheckDistance, transform.position.y, transform.position.z));
        }
        if (_showNormalColliderGizmos)
        {
            Gizmos.DrawCube(transform.position + new Vector3(NormalColliderShift.x, NormalColliderShift.y, 0.0f), NormalColliderSize * transform.localScale);
        }
        if (_showSlideColliderGizmos)
        {
            Gizmos.DrawCube(transform.position + new Vector3(SlideColliderShift.x, SlideColliderShift.y, 0.0f), SlideColliderSize * transform.localScale);
        }
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
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _isAttackPress = true;
        }
    }
    public void OnShield(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            IsBlockingPress = true;
        }
        if (ctx.canceled)
        {
            IsBlockingPress = false;
        }
    }
    #endregion
}
