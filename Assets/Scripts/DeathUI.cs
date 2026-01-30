using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Death screen: red flash then RIP panel with Start Over button.
/// Add to an empty GameObject; it builds the Canvas and UI at runtime.
/// Requires GameManager in scene for Start Over.
/// </summary>
public class DeathUI : MonoBehaviour
{
    public static DeathUI Instance { get; private set; }

    [Header("Red Flash")]
    [SerializeField] private float redFlashDuration = 0.5f;
    [SerializeField] private float redMaxAlpha = 0.7f;

    [Header("RIP Panel")]
    [SerializeField] private string ripText = "RIP";
    [SerializeField] private string startOverButtonText = "Start Over";

    private Canvas _canvas;
    private Image _redOverlay;
    private GameObject _ripPanel;
    private bool _shown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        EnsureEventSystem();
        SetupCanvas();
        gameObject.SetActive(false);
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null) return;
        GameObject es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();
    }

    private void SetupCanvas()
    {
        Canvas canvasComponent = GetComponent<Canvas>();
        if (canvasComponent == null)
            canvasComponent = gameObject.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasComponent.sortingOrder = 100;

        if (GetComponent<CanvasScaler>() == null)
        {
            CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }
        if (GetComponent<GraphicRaycaster>() == null)
            gameObject.AddComponent<GraphicRaycaster>();

        _canvas = canvasComponent;

        GameObject redGo = new GameObject("RedOverlay");
        redGo.transform.SetParent(transform, false);
        _redOverlay = redGo.AddComponent<Image>();
        _redOverlay.color = new Color(1f, 0f, 0f, 0f);
        _redOverlay.raycastTarget = false;
        RectTransform redRt = _redOverlay.rectTransform;
        redRt.anchorMin = Vector2.zero;
        redRt.anchorMax = Vector2.one;
        redRt.offsetMin = redRt.offsetMax = Vector2.zero;
        redGo.SetActive(false);

        _ripPanel = CreateRIPPanel();
        _ripPanel.SetActive(false);
    }

    private GameObject CreateRIPPanel()
    {
        GameObject panel = new GameObject("RIPPanel");
        panel.transform.SetParent(transform, false);

        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        RectTransform panelRt = panel.GetComponent<RectTransform>();
        panelRt.anchorMin = Vector2.zero;
        panelRt.anchorMax = Vector2.one;
        panelRt.offsetMin = panelRt.offsetMax = Vector2.zero;

        GameObject ripTextGo = new GameObject("RIPText");
        ripTextGo.transform.SetParent(panel.transform, false);
        Text ripTextComponent = ripTextGo.AddComponent<Text>();
        ripTextComponent.text = ripText;
        ripTextComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        ripTextComponent.fontSize = 72;
        ripTextComponent.alignment = TextAnchor.MiddleCenter;
        ripTextComponent.color = Color.white;
        ripTextComponent.raycastTarget = false;
        RectTransform textRt = ripTextGo.GetComponent<RectTransform>();
        textRt.anchorMin = new Vector2(0.5f, 0.6f);
        textRt.anchorMax = new Vector2(0.5f, 0.6f);
        textRt.pivot = new Vector2(0.5f, 0.5f);
        textRt.sizeDelta = new Vector2(600, 120);
        textRt.anchoredPosition = Vector2.zero;

        GameObject buttonGo = new GameObject("StartOverButton");
        buttonGo.transform.SetParent(panel.transform, false);
        Image buttonImage = buttonGo.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.5f, 0.2f);
        Button button = buttonGo.AddComponent<Button>();
        RectTransform btnRt = buttonGo.GetComponent<RectTransform>();
        btnRt.anchorMin = new Vector2(0.5f, 0.35f);
        btnRt.anchorMax = new Vector2(0.5f, 0.35f);
        btnRt.pivot = new Vector2(0.5f, 0.5f);
        btnRt.sizeDelta = new Vector2(240, 60);
        btnRt.anchoredPosition = Vector2.zero;

        GameObject btnTextGo = new GameObject("Text");
        btnTextGo.transform.SetParent(buttonGo.transform, false);
        Text btnText = btnTextGo.AddComponent<Text>();
        btnText.text = startOverButtonText;
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.fontSize = 28;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;
        btnText.raycastTarget = false;
        RectTransform btnTextRt = btnTextGo.GetComponent<RectTransform>();
        btnTextRt.anchorMin = Vector2.zero;
        btnTextRt.anchorMax = Vector2.one;
        btnTextRt.offsetMin = btnTextRt.offsetMax = Vector2.zero;

        button.onClick.AddListener(OnStartOverClicked);

        return panel;
    }

    private void OnStartOverClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartCurrentScene();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowDeathSequence()
    {
        if (_shown) return;
        _shown = true;
        gameObject.SetActive(true);
        StartCoroutine(RedFlashThenRIP());
    }

    private IEnumerator RedFlashThenRIP()
    {
        _redOverlay.gameObject.SetActive(true);
        _redOverlay.color = new Color(1f, 0f, 0f, 0f);

        float elapsed = 0f;
        while (elapsed < redFlashDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Clamp01(elapsed / redFlashDuration) * redMaxAlpha;
            _redOverlay.color = new Color(1f, 0f, 0f, a);
            yield return null;
        }

        _redOverlay.color = new Color(1f, 0f, 0f, redMaxAlpha);
        _ripPanel.SetActive(true);
    }
}
