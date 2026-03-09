using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Runtime health bar UI generator.
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    private RectTransform _container;
    private Image _fill;
    private Text _nameLabel;

    public void Setup(string playerName, Color color, Vector2 anchorPos)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("HealthCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create a simple white sprite if it doesn't exist
        Sprite whiteSprite = CreateWhiteSprite();

        GameObject barObj = new GameObject(playerName + "_HealthBar");
        barObj.transform.SetParent(canvas.transform, false);
        _container = barObj.AddComponent<RectTransform>();
        _container.anchorMin = anchorPos;
        _container.anchorMax = anchorPos;
        _container.anchoredPosition = Vector2.zero;
        _container.sizeDelta = new Vector2(200, 20);

        // Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(barObj.transform, false);
        Image bg = bgObj.AddComponent<Image>();
        bg.sprite = whiteSprite;
        bg.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        RectTransform bgRT = bgObj.GetComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;

        // Fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(barObj.transform, false);
        _fill = fillObj.AddComponent<Image>();
        _fill.sprite = whiteSprite;
        _fill.type = Image.Type.Filled;
        _fill.fillMethod = Image.FillMethod.Horizontal;
        _fill.fillAmount = 1.0f;
        _fill.color = color;
        RectTransform fillRT = fillObj.GetComponent<RectTransform>();
        fillRT.anchorMin = Vector2.zero;
        fillRT.anchorMax = Vector2.one;
        fillRT.offsetMin = fillRT.offsetMax = Vector2.zero;

        // Label
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(barObj.transform, false);
        _nameLabel = labelObj.AddComponent<Text>();
        _nameLabel.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        _nameLabel.text = playerName;
        _nameLabel.alignment = TextAnchor.MiddleLeft;
        _nameLabel.fontSize = 16;
        _nameLabel.color = Color.white;
        _nameLabel.fontStyle = FontStyle.Bold;
        RectTransform labelRT = labelObj.GetComponent<RectTransform>();
        labelRT.anchorMin = new Vector2(0, 1);
        labelRT.anchorMax = new Vector2(1, 1);
        labelRT.pivot = new Vector2(0, 0);
        labelRT.anchoredPosition = new Vector2(0, 5);
        labelRT.sizeDelta = new Vector2(0, 25);
        
        // Add a shadow to the label
        labelObj.AddComponent<Shadow>().effectDistance = new Vector2(1, -1);
    }

    private Sprite CreateWhiteSprite()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }

    public void UpdateHealth(int current, int max)
    {
        if (_fill != null)
        {
            float amount = (float)current / max;
            _fill.fillAmount = amount;
            Debug.Log($"UI Update: {_fill.transform.parent.name} fillAmount set to {amount} ({current}/{max})");
        }
    }
}
