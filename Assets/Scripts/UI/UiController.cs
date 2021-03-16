using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private UISettings defSettings = null;
    [SerializeField] private Camera cam = null;
    [SerializeField] private Text displayText = null;
    private string astDataCheck;
    private UIFrame uiFrame;
    void Awake()
    {
        uiFrame = defSettings.CreateUIInstance();
        cam = uiFrame.UICamera;
        Locator.Instance.GameEvents.gameStartMsg += OnGameStart;
        Locator.Instance.GameEvents.gamePauseMsg += OnGamePause;
        Locator.Instance.GameEvents.asteroidClickMsg += OnAsteroidClick;
        Locator.Instance.GameEvents.asteroidDestroyedMsg += OnAsteroidDestroyed;
    }
    void Update()
    {
        if (Input.GetAxis("Jump") > 0)
            Locator.Instance.GameEvents.gamePauseMsg?.Invoke();
    }

    private void OnGameStart()
    {
        uiFrame.ShowPanel("AstTracker");
    }

    private void OnAsteroidCountUpdate()
    {
    }

    private void OnAsteroidClick( DataViewer data)
    {
        var payload = new AstDataProperty(data);
        var strtest = payload._data.Name.text;
        uiFrame.OpenWindow("AstDataWindow",payload);
        astDataCheck = data.Name.text;
    }

    private void OnAsteroidDestroyed(AsteroidData data)
    {
        if (astDataCheck == data.scientificName)
            uiFrame.CloseWindow("AstDataWindow");
    }

    private void OnGamePause()
    {
        uiFrame.HidePanel("AstTracker");
        uiFrame.CloseWindow("AstDataWindow");
    }
}
