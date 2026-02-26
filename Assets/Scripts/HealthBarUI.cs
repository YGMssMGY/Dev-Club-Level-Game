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
        bg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        RectTransform bgRT = bgObj.GetComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;

        // Fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(barObj.transform, false);
        _fill = fillObj.AddComponent<Image>();
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
        _nameLabel.fontSize = 14;
        _nameLabel.color = Color.white;
        RectTransform labelRT = labelObj.GetComponent<RectTransform>();
        labelRT.anchorMin = new Vector2(0, 1);
        labelRT.anchorMax = new Vector2(1, 1);
        labelRT.pivot = new Vector2(0, 0);
        labelRT.anchoredPosition = new Vector2(0, 5);
        labelRT.sizeDelta = new Vector2(0, 20);

        // Weapon Slot
        GameObject slotObj = new GameObject("WeaponSlot");
        slotObj.transform.SetParent(barObj.transform, false);
        Image slotImg = slotObj.AddComponent<Image>();
        slotImg.color = new Color(1, 1, 1, 0.3f);
        RectTransform slotRT = slotObj.GetComponent<RectTransform>();
        slotRT.anchorMin = new Vector2(1, 0.5f);
        slotRT.anchorMax = new Vector2(1, 0.5f);
        slotRT.pivot = new Vector2(0, 0.5f);
        slotRT.anchoredPosition = new Vector2(10, 0);
        slotRT.sizeDelta = new Vector2(30, 30);

        GameObject slotLabel = new GameObject("SlotLabel");
        slotLabel.transform.SetParent(slotObj.transform, false);
        Text sl = slotLabel.AddComponent<Text>();
        sl.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        sl.text = "W";
        sl.alignment = TextAnchor.MiddleCenter;
        sl.fontSize = 12;
        sl.color = Color.white;
        RectTransform slRT = slotLabel.GetComponent<RectTransform>();
        slRT.anchorMin = Vector2.zero;
        slRT.anchorMax = Vector2.one;
        slRT.sizeDelta = Vector2.zero;
    }

    public void UpdateHealth(int current, int max)
    {
        if (_fill != null)
        {
            _fill.fillAmount = (float)current / max;
        }
    }
}
