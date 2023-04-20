using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningGhoulStateManager : EnemyStateManager
{
    BurningGhoulBaseState _currentState = null;
    BurningGhoulStateFactory _factory = null;

    private PlayerDetect _playerDetection;
    private AttackDetect _attackDetection;
    private WallDetect _wallDetection;
    private GroundDetect _groundDetection;
    private Rigidbody2D _rigidbody2D;
    private Animator _anim;
    private bool _facingRight;
    private bool _canAttack = true;
    private bool _attacking = false;
    private Material _material;

    [SerializeField] private bool faceRightAtRotationZero;   //TRUE when rotation.y == 0, sprite face right.
    [SerializeField] private float _movingSpeed;
    [SerializeField] private GameObject _attackCollider;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _attackCDTime;
    [SerializeField] private float _attackDamage;
    [SerializeField] private Shader _colorTintShader;
    [SerializeField] private Shader _attackingShader;
    [SerializeField] private Color _hurtColor;
    [SerializeField] private float _hurtFadeSpeed;


    public BurningGhoulStateFactory Factory { get => _factory; set => _factory = value; }
    public BurningGhoulBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public bool PlayerDetected { get => _playerDetected; }
    public bool AttackDetected { get => _attackDetected; }
    public bool WallDetected { get => _wallDetected; }
    public bool GroundDetected { get => _groundDetected; }
    public Rigidbody2D Rigidbody2D { get => _rigidbody2D; }
    public Animator Anim { get => _anim; }
    public float MovingSpeed { get => _movingSpeed; }
    public Vector3 Target { get => _playerDetection.target; }
    public float AttackDelay { get => _attackDelay; }
    public float AttackCDTime { get => _attackCDTime; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public Vector3 AttackCenter { get => _attackDetection.transform.position; }
    public bool Attacking { get => _attacking; }
    public Shader ColorTintShader { get => _colorTintShader; }
    public Shader AttackingShader { get => _attackingShader; }
    public Material Material { get => _material; }
    public Color HurtColor { get => _hurtColor; }
    public float HurtFadeSpeed { get => _hurtFadeSpeed; }

    #region readonly inspector
    [ReadOnly]
    [SerializeField]
    private bool _playerDetected;
    [ReadOnly]
    [SerializeField]
    private bool _attackDetected;
    [ReadOnly]
    [SerializeField]
    private bool _wallDetected;
    [ReadOnly]
    [SerializeField]
    private bool _groundDetected;
    [ReadOnly]
    [SerializeField]
    private string CurrentStateString;
    #endregion

    private void Start()
    {
        // get component
        _playerDetection = GetComponentInChildren<PlayerDetect>();
        _attackDetection = GetComponentInChildren<AttackDetect>();
        _wallDetection = GetComponentInChildren<WallDetect>();
        _groundDetection = GetComponentInChildren<GroundDetect>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _attackCollider.GetComponent<EnemyAttackCollide>().SetDamage(_attackDamage);
        _material = GetComponent<SpriteRenderer>().material;

        // state setup
        Factory = new BurningGhoulStateFactory(this);
        _currentState = Factory.Patrol(); // Initial State
        _currentState.EnterState();

        //init value
        _facingRight = faceRightAtRotationZero;
        _material.shader = AttackingShader;
    }

    private void Update()
    {
        _currentState.UpdateState();

        CurrentStateString = _currentState.ToString();
        //update detection
        _playerDetected = _playerDetection.detected;
        _attackDetected = _attackDetection.detected;
        _wallDetected = _wallDetection.outOfRange;
        _groundDetected = _groundDetection.outOfRange;
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    public void SwitchState(BurningGhoulBaseState newState)
    {
        _currentState.ExitState();

        newState.EnterState();

        this.CurrentState = newState;
    }

    public override void HurtState()
    {
        SwitchState(_factory.Hurt());
    }

    #region interact function

    public void attackStart() 
    {
        _attackCollider.SetActive(true);
        _attacking = true;
    }
    public void attackEnd()
    {
        _attackCollider.SetActive(false);
        _attacking = false;
    }

    #endregion

    #region useful function
    public void startCorutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    public void LookAtPlayer()
    {
        //on player detected
        if (_playerDetection.detected)
        {
            if (_playerDetection.target.x < Rigidbody2D.position.x && _facingRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                _facingRight = false;
            }
            else if (_playerDetection.target.x > Rigidbody2D.position.x && !_facingRight)
            {
                transform.localRotation = Quaternion.Euler(0, -180, 0);
                _facingRight = true;
            }
        }
    }

    public void TurnAtEdge()
    {
        if (_facingRight)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, -180, 0);

        _facingRight = !_facingRight;
    }
    #endregion
}

