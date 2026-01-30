using UnityEngine;

/// <summary>
/// Makes the camera follow a target in 2D (smooth follow, keeps Z for orthographic).
/// Attach to Main Camera and assign the player Transform in the inspector.
/// </summary>
public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Assign the player (e.g. Square) Transform.")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private bool useSmoothDamp = true;
    [SerializeField] private float zPosition = -10f;

    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 goal = new Vector3(target.position.x, target.position.y, zPosition);

        if (useSmoothDamp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, goal, ref _velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, goal, 1f - Mathf.Exp(-10f * Time.deltaTime));
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
    }
}
