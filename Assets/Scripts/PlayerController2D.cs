using UnityEngine;

/// <summary>
/// Basic 2D player controller: horizontal movement and jump.
/// Attach to the player GameObject (e.g. "Square") that has a Rigidbody2D and Collider2D.
/// Uses legacy Input (Horizontal axis, Jump button).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer = ~0;
    [Tooltip("Larger radius (0.2–0.3) helps catch ground reliably.")]
    [SerializeField] private float groundCheckRadius = 0.25f;
    [Tooltip("Slightly below feet so circle overlaps ground (e.g. -0.55 for 1-unit tall sprite).")]
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.55f);
    [Tooltip("Allow jump when moving down slowly (coyote / landing frame).")]
    [SerializeField] private float coyoteVelocityThreshold = 0.5f;

    private Rigidbody2D _rb;
    private bool _isGrounded;
    private bool _jumpRequested;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGrounded();

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            _jumpRequested = true;
    }

    private void FixedUpdate()
    {
        // Apply jump in FixedUpdate so it syncs with physics
        if (_jumpRequested)
        {
            _jumpRequested = false;
            bool canJump = _isGrounded || (_rb.linearVelocity.y >= -coyoteVelocityThreshold && _rb.linearVelocity.y <= coyoteVelocityThreshold);
            if (canJump)
            {
                Vector2 v = _rb.linearVelocity;
                v.y = jumpForce;
                _rb.linearVelocity = v;
            }
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = horizontal * moveSpeed;
        _rb.linearVelocity = velocity;
    }

    private void CheckGrounded()
    {
        Vector2 point = (Vector2)transform.position + groundCheckOffset;
        Collider2D hit = Physics2D.OverlapCircle(point, groundCheckRadius, groundLayer);
        _isGrounded = hit != null && hit.gameObject != gameObject;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector2 point = (Vector2)transform.position + groundCheckOffset;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(point, groundCheckRadius);
    }
#endif
}
