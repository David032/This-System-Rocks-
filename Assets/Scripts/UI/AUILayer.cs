using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUILayer<TScreen> : MonoBehaviour where TScreen : IUIScreenController
{
    protected Dictionary<string, TScreen> registeredScreens;
    
    /// <summary>
    /// Show a screen
    /// </summary>
    /// <param name="screen">ScreenController to be shown.</param>
    public abstract void ShowScreen(TScreen screen);
    /// <summary>
    /// Show a screen and pass in properties.
    /// </summary>
    /// <param name="screen">ScreenController to be shown.</param>
    /// <param name="properties">Property data.</param>
    /// <typeparam name="TProps">Property data type.</typeparam>
    public abstract void ShowScreen<TProps>(TScreen screen, TProps properties) where TProps : IUIScreenProperties;
    
    /// <summary>
    /// Hide a screen.
    /// </summary>
    /// <param name="screen">ScreenController to be hidden.</param>
    public abstract void HideScreen(TScreen screen);

    /// <summary>
    /// Initialise This LayerController.
    /// </summary>
    public virtual void Initialise()
    {
        registeredScreens = new Dictionary<string, TScreen>();
    }

    /// <summary>
    /// Reparent given screen transform to this LayerController.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="screenTransform"></param>
    public virtual void ReparentScreen(IUIScreenController controller, Transform screenTransform)
    {
        screenTransform.SetParent(transform,false);
    }
    
    /// <summary>
    /// Register a ScreenController to a screenId
    /// </summary>
    /// <param name="screenId">Target screenId</param>
    /// <param name="controller">Screen controller to be registered</param>
    public void RegisterScreen(string screenId, TScreen controller)
    {
        if (!registeredScreens.ContainsKey(screenId))
        {
            ProcessScreenRegister(screenId, controller);
        }
        else
        {
            Debug.LogError("[UILayerController] Screen controller already registered for ID: " + screenId);
        }
    }

    /// <summary>
    /// Unregisters a controller from a screenId
    /// </summary>
    /// <param name="screenId">The screenId</param>
    /// <param name="controller"> The controller to be unregistered</param>
    public void UnregisterScreen(string screenId, TScreen controller)
    {
        if (registeredScreens.ContainsKey(screenId))
        {
            ProcessScreenUnregister(screenId, controller);
        }
        else
        {
            Debug.LogError("[UILayerController] Screen controller not registered for ID: " + screenId);
        }
    }

    /// <summary>
    /// Attempt to find a screen that matches passed screenId
    /// and shows it.
    /// </summary>
    /// <param name="screenId">Desired screenId</param>
    public void ShowScreenByID(string screenId)
    {
        TScreen ct1;
        if (registeredScreens.TryGetValue(screenId, out ct1))
        {
            ShowScreen(ct1);
        }
        else
        {
            Debug.LogError("[UILayerController] Cannot show Screen ID " +screenId +" as it is not registered!");
        }
    } 
    /// <summary>
    /// Attempt to find a screen that matches passed screenId,
    /// passes property data and shows it.
    /// </summary>
    /// <param name="screenId">Desired screenId</param>
    /// <param name="properties">Data properties to pass in</param>
    /// <typeparam name="TProps">Type of data property</typeparam>
    public void ShowScreenByID<TProps>(string screenId, TProps properties) where TProps : IUIScreenProperties
    {
        TScreen ct1;
        if (registeredScreens.TryGetValue(screenId, out ct1))
        {
            ShowScreen(ct1, properties);
        }
        else
        {
            Debug.LogError("[UILayerController] Cannot show Screen ID " +screenId +" as it is not registered!");
        }
    }
    
    /// <summary>
    /// Attempt to find a screen that matches passed screenId
    /// and hides it.
    /// </summary>
    /// <param name="screenId">Desired screenId</param>
    public void HideScreenByID(string screenId)
    {
        TScreen ct1;
        if (registeredScreens.TryGetValue(screenId, out ct1))
        {
            HideScreen(ct1);
        }
        else
        {
            Debug.LogError("[UILayerController] Cannot hide Screen ID " +screenId +" as it is not registered!");
        }
    }

    /// <summary>
    /// Checks if a screen is registered on this LayerController.
    /// </summary>
    /// <param name="screenId"></param>
    /// <returns>True if registered, false if not.</returns>
    public bool IsScreenRegistered(string screenId)
    {
        return registeredScreens.ContainsKey(screenId);
    }

    /// <summary>
    /// Hides all screens registered to this layer.
    /// </summary>
    /// <param name="shouldAnimateOut">Should the screens animate out when hiding?</param>
    public virtual  void HideAllScreens(bool shouldAnimateOut = true)
    {
        foreach (var screen in registeredScreens)
        {
            screen.Value.Hide(shouldAnimateOut);
        }
    }
    protected virtual void ProcessScreenRegister(string screenId, TScreen controller)
    {
        controller.ScreenID = screenId;
        registeredScreens.Add(screenId, controller);
        controller.ScreenDestroyed += OnScreenDestroyed;
    }
    protected virtual void ProcessScreenUnregister(string screenId, TScreen controller)
    {
        controller.ScreenDestroyed -= OnScreenDestroyed;
        registeredScreens.Remove(screenId);
    }

    private void OnScreenDestroyed(IUIScreenController screen)
    {
        if (!string.IsNullOrEmpty(screen.ScreenID)
            && registeredScreens.ContainsKey(screen.ScreenID))
        {
            UnregisterScreen(screen.ScreenID, (TScreen)screen);
        }
    }
}