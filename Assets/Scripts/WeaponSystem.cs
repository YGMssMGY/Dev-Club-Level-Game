using UnityEngine;

/// <summary>
/// Simple weapon system that performs a melee attack in front of the player.
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode attackKey = KeyCode.F;
    [SerializeField] private bool useDefaultFire = true;
    [SerializeField] private string fireButton = "Fire1";

    [Header("Attack Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float range = 1.2f;
    [SerializeField] private float cooldown = 0.4f;
    [SerializeField] private LayerMask enemyLayer = ~0; // Default to hit everything

    private float _nextAttackTime;

    public void ConfigureInput(KeyCode attack)
    {
        useDefaultFire = false;
        attackKey = attack;
    }

    private void Update()
    {
        bool attackRequested = false;
        if (useDefaultFire)
        {
            if (Input.GetButtonDown(fireButton)) attackRequested = true;
        }
        else
        {
            if (Input.GetKeyDown(attackKey)) attackRequested = true;
        }

        if (attackRequested && Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + cooldown;
        }
    }

    private void Attack()
    {
        // Circular attack around the player (knife range)
        Vector2 origin = transform.position;
        
        // --- VISUAL FEEDBACK (SLASH) ---
        // A quick expanding circle or flash to indicate the attack area
        GameObject slash = new GameObject("KnifeSlash");
        slash.transform.position = origin;
        slash.transform.localScale = Vector3.one * 0.1f;
        SpriteRenderer sr = slash.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd"); // Circular sprite
        sr.color = new Color(1, 1, 1, 0.6f);
        sr.sortingOrder = 5;
        
        // Simple animation via script (expand and fade)
        Destroy(slash, 0.15f);
        StartCoroutine(AnimateSlash(slash.transform, sr));
        // ------------------------------

        // Find all colliders in the attack circle
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin, range, enemyLayer);
        Debug.Log($"{gameObject.name} attack hit {hitEnemies.Length} colliders.");

        foreach (Collider2D enemy in hitEnemies)
        {
            // Don't hit yourself
            if (enemy.gameObject == gameObject) continue;

            // Use GetComponentInParent in case the collider is on a child object
            PlayerHealth health = enemy.GetComponentInParent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log($"{gameObject.name} slashed {enemy.name} for {damage} damage! Current target health: {health.GetCurrentHealth()}");
            }
            else
            {
                Debug.Log($"{gameObject.name} hit {enemy.name} but no PlayerHealth found on it or its parents.");
            }
        }
    }

    private System.Collections.IEnumerator AnimateSlash(Transform t, SpriteRenderer sr)
    {
        float duration = 0.15f;
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 0.5f;
        Vector3 endScale = Vector3.one * (range * 2f); // Diameter

        while (elapsed < duration)
        {
            if (t == null) yield break;
            elapsed += Time.deltaTime;
            float pct = elapsed / duration;
            t.localScale = Vector3.Lerp(startScale, endScale, pct);
            sr.color = new Color(1, 1, 1, Mathf.Lerp(0.6f, 0, pct));
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
