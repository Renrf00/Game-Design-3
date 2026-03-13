using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Tooltip("Default offset from player")]
    [SerializeField] private Vector3 offset;

    [SerializeField] private float lookAheadMult;
    [SerializeField] private float lookAheadInertia;

    [SerializeField] private float lookAheadOffsetCutoff;
    [SerializeField] private Rigidbody playerRB;

    [SerializeField] private Vector3 oldLookAheadOffset;

    private void Update()
    {
        // Ideal offset we want
        Vector3 newLookAheadOffset = playerRB.linearVelocity * lookAheadMult;

        // Using inertia: a type of easing
        Vector3 lookAheadOffset = newLookAheadOffset * lookAheadInertia + oldLookAheadOffset * (1-lookAheadInertia);
        if (lookAheadOffset.magnitude < lookAheadOffsetCutoff) lookAheadOffset = Vector3.zero;

        // Camera position is player position + offset + lookahead
        transform.SetPositionAndRotation(playerRB.transform.position + offset + lookAheadOffset, transform.rotation);

        oldLookAheadOffset = lookAheadOffset;
    }

}
