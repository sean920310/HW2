using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallDetect : MonoBehaviour
{
    [Header("Ground Detect")]
    [SerializeField] public float detectRange;
    [SerializeField] public LayerMask whatIsGround;

    [Header("Result")]
    [SerializeField] public bool outOfRange;

    [Header("Event")]
    public UnityEvent<GameObject> OnNotDetected;

    [Header("Gizmos")]
    public bool showGizmos = false;
    public Color gizmosColor;

    // Update is called once per frame
    void Update()
    {
        Collider2D wall = Physics2D.Raycast(transform.position, -transform.right, detectRange, whatIsGround).collider;
        if (wall != null)
        {
            OnNotDetected?.Invoke(wall.gameObject);
            outOfRange = true;
        }
        else
            outOfRange = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(-detectRange, 0, 0));
        }
    }
}
