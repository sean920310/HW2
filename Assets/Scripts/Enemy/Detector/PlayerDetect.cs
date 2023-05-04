using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetect : MonoBehaviour
{
    [Header("Player Detect")]
    [SerializeField] public bool faceRightAtRotationZero;   //TRUE when rotation.y == 0, sprite face right.
    [SerializeField] public float playerDetectRange;
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public LayerMask visibleLayer;

    [Header("Result")]
    [SerializeField]
    [ReadOnly]
    public Vector3 target;
    [SerializeField]
    [ReadOnly]
    public bool detected = false;

    [Header("Gizmos")]
    public bool showGizmos = false;
    public Color gizmosColor;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        detected = DetectedPlayer();
    }

    private bool DetectedPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, playerDetectRange, whatIsPlayer);
        if (player != null)
        {
            target = player.transform.position;

            RaycastHit2D result = Physics2D.Raycast(transform.position, (target - transform.position).normalized, playerDetectRange, visibleLayer);
            if (result.collider != null)
            {
                return (whatIsPlayer & (1 << result.collider.gameObject.layer)) != 0;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(transform.position, playerDetectRange);
        }
    }
}
