using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehavor : MonoBehaviour
{

    [Header("Attribute")]
    public float AttackCD = 2f;
    public float speed = 2f;


    private Rigidbody2D rb;
    private Animator animator;
    private PlayerDetect playerDetect;
    private WallDetect wallDetect;
    private GroundDetect groundDetect;
    [SerializeField] private bool noticePlayer = true;
    private bool canAttack = true;
    private bool faceRight = false;
    private bool outOfRange = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerDetect = GetComponentInChildren<PlayerDetect>();
        wallDetect = GetComponentInChildren<WallDetect>();
        groundDetect = GetComponentInChildren<GroundDetect>();
    }

    // Update is called once per frame
    void Update()
    {
        outOfRange = wallDetect.outOfRange || groundDetect.outOfRange;
    }

    void FixedUpdate()
    {
        LookAtPlayer();
        if (playerDetect.detected && noticePlayer && !outOfRange)
            ChasePlayer();
        else if (!outOfRange)
            Patrol();

    }

    public void Patrol()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, rb.position - (Vector2)rb.transform.right, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            StartCoroutine(AttackCoolDown());
        }
    }

    public void ChasePlayer()
    {
        Vector2 target = new Vector2(playerDetect.target.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public void TakeDamage(float damage)
    {

    }

    public void TurnAtEdge()
    {
        if (!playerDetect.detected)
        {
            if (faceRight)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            else
                transform.localRotation = Quaternion.Euler(0, -180, 0);
            
            faceRight = !faceRight;
        }
    }

    private void LookAtPlayer()
    {
        if (playerDetect.detected)
        {
            //noticePlayer = !((playerDetect.target.x > rb.position.x) ^ faceRight);
            //if (!noticePlayer)
                //return;

            if (playerDetect.target.x < rb.position.x && faceRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                faceRight = false;
            }
            else if (playerDetect.target.x > rb.position.x && !faceRight)
            {
                transform.localRotation = Quaternion.Euler(0, -180, 0);
                faceRight = true;
            }
        }
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(AttackCD);
        canAttack = true;
    }
}
