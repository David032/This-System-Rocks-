using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISettings", menuName = "UI/UI Settings")]
public class UISettings : ScriptableObject
{
    [Tooltip("Prefab for the UI Manager structure")] [SerializeField]
    private UIManager templateUIManager = null;

    [Tooltip("Prefabs for all the Screens ( Panels and Windows ) that will be registered by the Manager.")]
    [SerializeField]
    private List<GameObject> screensToRegister = null;

    [Tooltip(
        "Deactivate the screen GameObject once instantiated? If false, it will be visible at start.")]
    [SerializeField]
    private bool deactivateScreenObjects = true;

    public UIManager CreateUIInstance(bool instantiateAndRegisterScreens = true)
    {
        var newUI = Instantiate(templateUIManager);
        if (instantiateAndRegisterScreens)
        {
            foreach (var screen in screensToRegister)
            {
                var screenInstance = Instantiate(screen);
                var screenController = screenInstance.GetComponent<IUIScreenController>();

                if (screenController != null)
                {
                    newUI.RegisterScreen(screen.name, screenController, screenInstance.transform);
                    
                    if (deactivateScreenObjects && screenInstance.activeSelf)
                        screenInstance.SetActive(false);
                }
                else
                Debug.LogError("[UIConfig] Screen does not contain controller. Skipping " +screen.name);
            }
        }

        return newUI;
    }

    private void OnValidate()
    {
        List<GameObject> objectsToRemove = new List<GameObject>();
        for (int i = 0; i < screensToRegister.Count; i++)
        {
            var screenCont1 = screensToRegister[i].GetComponent<IUIScreenController>();
            if (screenCont1 == null)
                objectsToRemove.Add(screensToRegister[i]);
        }

        if (objectsToRemove.Count > 0)
        {
            foreach (var obj in objectsToRemove)
            {
                Debug.LogError("[UISettings] Some objects that were added to the UIPrefab list do not " +
                               "have ScreenControllers attached.");
                screensToRegister.Remove(obj);
            }
            
        }
    }
    
}
