using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCheck : MonoBehaviour
{
    private bool onLadder;
    [Header("Collider Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float ladderLength = 1.005f;
    [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask ladderLayer;
    private void Update()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        onLadder = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, ladderLength, ladderLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, ladderLength, ladderLayer);
        
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onLadder) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * ladderLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * ladderLength);
    }

    //Send ground detection to other scripts
    public bool IsOnLadder()
    {
        return onLadder;
    }
}
