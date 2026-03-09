using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple game manager: restart scene, quit. Optional singleton.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Optional")]
    [SerializeField] private bool dontDestroyOnLoad;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    [Header("Player Setup")]
    [SerializeField] private bool autoSetupPlayers = true;
    [SerializeField] private Color p1Color = Color.cyan;
    [SerializeField] private Color p2Color = Color.red;

    private void Start()
    {
        if (autoSetupPlayers)
            SetupPlayers();
    }

    private void SetupPlayers()
    {
        // 1. Find all potential player objects (usually objects with PlayerController2D)
        PlayerController2D[] controllers = FindObjectsByType<PlayerController2D>(FindObjectsSortMode.None);
        
        // 2. Sort by position for consistent P1/P2 assignment
        System.Array.Sort(controllers, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < controllers.Length; i++)
        {
            PlayerController2D ctrl = controllers[i];
            GameObject go = ctrl.gameObject;

            // --- AUTO-ATTACH SYSTEMS ---
            PlayerHealth health = go.GetComponent<PlayerHealth>();
            if (health == null) health = go.AddComponent<PlayerHealth>();

            WeaponSystem weapon = go.GetComponent<WeaponSystem>();
            if (weapon == null) weapon = go.AddComponent<WeaponSystem>();

            PlayerDeathHandler pdh = go.GetComponent<PlayerDeathHandler>();
            if (pdh == null) pdh = go.AddComponent<PlayerDeathHandler>();
            // ---------------------------

            string pName = i == 0 ? "P1 - BLUE" : "P2 - RED";
            go.name = pName;
            Color pColor = (i == 0) ? p1Color : p2Color;
            Vector2 anchor = (i == 0) ? new Vector2(0.15f, 0.9f) : new Vector2(0.85f, 0.9f);

            // Configure Inputs
            if (i == 0) // P1 (WASD + F)
            {
                ctrl.ConfigureInput(KeyCode.Space, KeyCode.A, KeyCode.D);
                weapon.ConfigureInput(KeyCode.F);
            }
            else // P2 (Arrows + /)
            {
                ctrl.ConfigureInput(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
                weapon.ConfigureInput(KeyCode.Slash);
            }

            // --- WEAPON CONFIG ---
            // Allow them to hit everything (players are usually on default or have colliders)
            // But we already handle self-hit in WeaponSystem.cs
            // ---------------------

            // Setup Health Bar
            HealthBarUI bar = go.GetComponent<HealthBarUI>();
            if (bar == null) bar = go.AddComponent<HealthBarUI>();
            
            bar.Setup(pName, pColor, anchor);
            health.OnHealthChanged += bar.UpdateHealth;
            health.RefreshUI();
            
            // Give them colors for easier identification
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = pColor;
        }
    }

    public void OnPlayerDied(GameObject deadPlayer)
    {
        // Find the other player
        PlayerHealth[] allPlayers = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        string winnerName = "NOBODY";
        
        foreach (var p in allPlayers)
        {
            if (p.gameObject != deadPlayer && p.GetCurrentHealth() > 0)
            {
                // We found a winner!
                winnerName = p.gameObject.name;
                // If it's the "Player" prefab name, try to make it nicer
                if (winnerName.Contains("PLAYER")) winnerName = winnerName.Replace("(Clone)", "").Trim();
                break;
            }
        }

        if (DeathUI.Instance != null)
        {
            DeathUI.Instance.ShowWinnerSequence(winnerName);
        }
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}