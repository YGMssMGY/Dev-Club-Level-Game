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
    [SerializeField] private float range = 1f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

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
        // Simple raycast or overlap circle in front
        // We assume the player is looking right if scale.x > 0
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 origin = transform.position;
        Vector2 offset = new Vector2(direction * range * 0.5f, 0f);
        
        // --- VISUAL FEEDBACK (SLASH) ---
        GameObject slash = new GameObject("SlashEffect");
        slash.transform.position = (Vector2)transform.position + offset;
        slash.transform.localScale = new Vector3(range, range, 1);
        SpriteRenderer sr = slash.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Background.psd"); // Simple white square
        sr.color = new Color(1, 1, 1, 0.5f);
        sr.sortingOrder = 5;
        Destroy(slash, 0.1f);
        // ------------------------------

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(origin + offset, range * 0.5f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject == gameObject) continue;

            PlayerHealth health = enemy.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log($"{gameObject.name} hit {enemy.name} for {damage} damage!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 origin = transform.position;
        Vector2 offset = new Vector2(direction * range * 0.5f, 0f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin + offset, range * 0.5f);
    }
}
