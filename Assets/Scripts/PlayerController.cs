using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 4;
    public float movementMultiplier = 1;
    [SerializeField] private float jumpSpeed = 5;
    public float jumpMultiplier = 1;
    [SerializeField] private float jumpCooldown = 0.1f;
    private bool canJump = true;

    [Header("Keys")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Tags")]
    [SerializeField] private string groundTag = "Ground";

    [Header("Input")]
    private bool moveInput = false;
    private bool jumpInput = false;

    [Header("Ground check")]
    [SerializeField] private float groundRaycastLength = 2;
    public bool grounded = false;
    public bool groundRay = false;
    public bool groundCollision = false;

    [Header("Extras")]
    [SerializeField] private bool snapierMovement = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        groundRay = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundRaycastLength) ? hit.collider.tag == groundTag : false;

        grounded = groundRay && groundCollision;

        moveInput = Input.GetAxis("Horizontal") != 0;

        jumpInput = Input.GetKey(jumpKey);
    }

    private void FixedUpdate()
    {
        if (moveInput && !Physics.Raycast(transform.position, new Vector3(Mathf.Round(Input.GetAxis("Horizontal")), 0, 0), 1))
            Move();

        if (grounded && canJump && jumpInput)
            StartCoroutine(Jump());
    }

    private void Move()
    {
        if (snapierMovement)
            rb.linearVelocity = new Vector3(
                (Input.GetAxis("Horizontal") > 0 ? Mathf.Ceil(Input.GetAxis("Horizontal")) : Mathf.Floor(Input.GetAxis("Horizontal"))) * moveSpeed * movementMultiplier,
                rb.linearVelocity.y,
                0);
        else
            rb.linearVelocity = new Vector3(
                Input.GetAxis("Horizontal") * moveSpeed * movementMultiplier,
                rb.linearVelocity.y,
                0);
    }
    private IEnumerator Jump()
    {
        jumpInput = false;
        canJump = false;
        rb.AddForce(Vector3.up * jumpSpeed * jumpMultiplier, ForceMode.Impulse);

        yield return new WaitForSeconds(jumpCooldown);

        canJump = true;
    }

    public void ApplyPowerUp(PowerUp powerUp, float multilpierAmount, float cooldown)
    {
        switch (powerUp)
        {
            case PowerUp.Movement:
                movementMultiplier = multilpierAmount;
                break;
            case PowerUp.Jump:
                jumpMultiplier = multilpierAmount;
                break;
            default:
                break;
        }

        StartCoroutine(ResetAfter(powerUp, cooldown));
    }

    private IEnumerator ResetAfter(PowerUp powerUp, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        switch (powerUp)
        {
            case PowerUp.Movement:
                movementMultiplier = 1;
                break;
            case PowerUp.Jump:
                jumpMultiplier = 1;
                break;
            default:
                break;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        groundCollision = collision.gameObject.tag == groundTag;
    }

    void OnCollisionExit(Collision collision)
    {
        groundCollision = false;
    }

    public enum PowerUp
    {
        Jump,
        Movement
    }
}
