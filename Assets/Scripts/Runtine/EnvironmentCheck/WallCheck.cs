using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public bool onWall;
    [Header("Collider Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float wallLength = 1.005f;
    [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask wallLayer;
    private void Update()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        onWall = Physics2D.Raycast(transform.position + colliderOffset, Vector2.right, wallLength, wallLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.right, wallLength, wallLayer)
            || Physics2D.Raycast(transform.position - colliderOffset, Vector2.left, wallLength, wallLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.left, wallLength, wallLayer);
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onWall) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.right * wallLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.right * wallLength);
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.left * wallLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.left * wallLength);
    }

    //Send ground detection to other scripts
    public bool isWall()
    {
        return onWall;
    }
}
