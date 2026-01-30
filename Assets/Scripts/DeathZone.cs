using UnityEngine;

/// <summary>
/// Optional: when the player enters this trigger (e.g. pit), they die.
/// Add to a GameObject with Collider2D set to Is Trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var handler = other.GetComponent<PlayerDeathHandler>();
        if (handler != null)
            handler.Die();
    }
}
