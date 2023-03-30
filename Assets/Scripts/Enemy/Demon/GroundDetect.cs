using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetect : MonoBehaviour
{
    [Header("Ground Detect")]
    [SerializeField] public float detectRange;
    [SerializeField] public LayerMask whatIsGround;

    [Header("Result")]
    [SerializeField] public bool outOfRange; 

    [Header("Gizmos")]
    public bool showGizmos = false;
    public Color gizmosColor;

    // Update is called once per frame
    void Update()
    {
        Collider2D ground = Physics2D.Raycast(transform.position, -transform.up, detectRange, whatIsGround).collider;
        outOfRange = (ground == null);
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -detectRange,0));
        }
    }
}
