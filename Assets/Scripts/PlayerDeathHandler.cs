using UnityEngine;
using System.Collections;

/// <summary>
/// Handles death when the player falls off the ground: break-apart effect, then death UI.
/// Add to the same GameObject as PlayerController2D. Set death Y below your lowest platform.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Death Trigger")]
    [Tooltip("Player dies when position.y falls below this (e.g. -5 for platform at ~-1.5).")]
    [SerializeField] private float deathYThreshold = -5f;

    [Header("Break Apart")]
    [SerializeField] private int pieceCount = 4;
    [SerializeField] private float pieceScale = 0.4f;
    [SerializeField] private float explodeForce = 4f;
    [SerializeField] private float pieceLifetime = 2f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private PlayerController2D _controller;
    private bool _isDead;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        if (_isDead) return;
        if (transform.position.y < deathYThreshold)
            Die();
    }

    /// <summary>
    /// Call from DeathZone (trigger) or when fall is detected.
    /// </summary>
    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        if (_controller != null) _controller.enabled = false;
        if (_rb != null) _rb.simulated = false;
        _spriteRenderer.enabled = false;

        var cam = FindFirstObjectByType<CameraFollow2D>();
        if (cam != null) cam.SetTarget(null);

        SpawnBreakPieces();
        StartCoroutine(NotifyDeathUIAfterDelay(0.3f));
    }

    private void SpawnBreakPieces()
    {
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == null) return;

        Vector2 pos = transform.position;
        float angleStep = 360f / pieceCount;

        for (int i = 0; i < pieceCount; i++)
        {
            GameObject piece = new GameObject("DeathPiece");
            piece.transform.position = pos;
            piece.transform.localScale = Vector3.one * pieceScale;

            SpriteRenderer sr = piece.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 10;

            Rigidbody2D prb = piece.AddComponent<Rigidbody2D>();
            float angle = (angleStep * i) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            prb.linearVelocity = dir * explodeForce;
            prb.gravityScale = 1f;

            Destroy(piece, pieceLifetime);
        }
    }

    private IEnumerator NotifyDeathUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (DeathUI.Instance != null)
            DeathUI.Instance.ShowDeathSequence();
    }
}
