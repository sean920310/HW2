using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackDetect : MonoBehaviour
{
    [Header("Attack Detect")]
    [SerializeField] public float attackDetectRange;
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

    // Update is called once per frame
    void Update()
    {
        detected = DetectedPlayer();
    }

    private bool DetectedPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackDetectRange, whatIsPlayer);
        if (player != null)
        {
            target = player.transform.position;
            RaycastHit2D result = Physics2D.Raycast(transform.position, (target - transform.position).normalized, attackDetectRange, visibleLayer);
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
            Gizmos.DrawWireSphere(transform.position, attackDetectRange);
        }
    }
}
