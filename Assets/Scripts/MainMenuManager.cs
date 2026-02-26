using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Runtime Main Menu generator.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        EnsureEventSystem();
        SetupMenu();
    }

    private void EnsureEventSystem()
    {
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() != null) return;
        GameObject es = new GameObject("EventSystem");
        es.AddComponent<UnityEngine.EventSystems.EventSystem>();
        es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }

    private void SetupMenu()
    {
        GameObject canvasObj = new GameObject("MenuCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.AddComponent<GraphicRaycaster>();

        // Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvas.transform, false);
        Image bg = bgObj.AddComponent<Image>();
        bg.color = new Color(0.02f, 0.02f, 0.1f, 1f);
        RectTransform bgRT = bgObj.GetComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.sizeDelta = Vector2.zero;

        // Decorative Panel
        GameObject panelObj = new GameObject("MainPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        Image panelImg = panelObj.AddComponent<Image>();
        panelImg.color = new Color(0.1f, 0.1f, 0.2f, 0.8f);
        RectTransform panelRT = panelObj.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0.5f, 0.5f);
        panelRT.anchorMax = new Vector2(0.5f, 0.5f);
        panelRT.sizeDelta = new Vector2(600, 700);

        // Title
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panelObj.transform, false);
        Text title = titleObj.AddComponent<Text>();
        title.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        title.text = "SUPER 2D\nBRAWLER";
        title.alignment = TextAnchor.MiddleCenter;
        title.fontSize = 72;
        title.color = Color.white;
        title.lineSpacing = 0.8f;
        
        Shadow titleShadow = titleObj.AddComponent<Shadow>();
        titleShadow.effectColor = new Color(1, 0.5f, 0, 1);
        titleShadow.effectDistance = new Vector2(4, -4);

        RectTransform titleRT = titleObj.GetComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0.5f, 0.85f);
        titleRT.anchorMax = new Vector2(0.5f, 0.85f);
        titleRT.sizeDelta = new Vector2(500, 200);

        // Scene list container
        GameObject listObj = new GameObject("ButtonGroup");
        listObj.transform.SetParent(panelObj.transform, false);
        RectTransform listRT = listObj.AddComponent<RectTransform>();
        listRT.anchorMin = new Vector2(0, 0);
        listRT.anchorMax = new Vector2(1, 0.7f);
        listRT.offsetMin = new Vector2(50, 50);
        listRT.offsetMax = new Vector2(-50, -20);

        VerticalLayoutGroup vlg = listObj.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.spacing = 15;
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;

        // --- BUTTONS ---
        
        // 1. Scene Selections
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        bool foundPlayableScene = false;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == SceneManager.GetActiveScene().name) continue;

            CreateButton(listObj.transform, "ENTER: " + name.ToUpper(), () => SceneManager.LoadScene(name), new Color(0.2f, 0.6f, 0.3f));
            foundPlayableScene = true;
        }
        if (!foundPlayableScene) CreateButton(listObj.transform, "START BATTLE", () => SceneManager.LoadScene("SampleScene"), new Color(0.2f, 0.6f, 0.3f));

        // 2. Settings (Placeholder)
        CreateButton(listObj.transform, "SETTINGS", () => Debug.Log("Settings Clicked"), new Color(0.4f, 0.4f, 0.4f));

        // 3. Exit
        CreateButton(listObj.transform, "EXIT GAME", () => {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }, new Color(0.7f, 0.2f, 0.2f));
    }

    private void CreateButton(Transform parent, string label, System.Action onClick, Color color)
    {
        GameObject btnObj = new GameObject("Button_" + label);
        btnObj.transform.SetParent(parent, false);
        
        // Layout Element is key for VerticalLayoutGroup
        LayoutElement le = btnObj.AddComponent<LayoutElement>();
        le.preferredHeight = 60;
        le.minHeight = 60;

        Image img = btnObj.AddComponent<Image>();
        img.color = color;
        
        // Add an outline to make it look like a button
        Outline outline = btnObj.AddComponent<Outline>();
        outline.effectColor = Color.white;
        outline.effectDistance = new Vector2(2, -2);

        Button btn = btnObj.AddComponent<Button>();
        ColorBlock colors = btn.colors;
        colors.highlightedColor = color * 1.3f;
        colors.pressedColor = Color.white;
        btn.colors = colors;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);
        Text t = textObj.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.text = label;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = 24;
        t.fontStyle = FontStyle.Bold;
        t.color = Color.white;
        
        RectTransform tRT = textObj.GetComponent<RectTransform>();
        tRT.anchorMin = Vector2.zero;
        tRT.anchorMax = Vector2.one;
        tRT.sizeDelta = Vector2.zero;

        btn.onClick.AddListener(() => onClick());
    }
}
