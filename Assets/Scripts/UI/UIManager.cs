using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Make all calls to UI through here
/// </summary>
public class UIManager : MonoBehaviour
{
    // Call to spawn in UI interfaces.
    public static UIManager instance;
    
    [SerializeField] private bool initialiseOnAwake = true;

    private UIPanelLayer _panelLayer;
    private UIWindowLayer _windowLayer;
     private void Awake()
     {
         if (instance == null) instance = this;
         if (initialiseOnAwake)
             Initialise();
     }
    

     private Canvas mainCanvas;
     private GraphicRaycaster graphicRaycaster;
     public Canvas MainCanvas
     {
         get
         {
             if (mainCanvas == null)
             {
                 mainCanvas = GetComponent<Canvas>();
             }

             return mainCanvas;
         }
     }

     /// <summary>
     /// Main camera used by mainCanvas
     /// </summary>
     public Camera UICamera
     {
         get { return mainCanvas.worldCamera; }
     }

     public virtual void Initialise()
     {
         if (_panelLayer == null)
         {
             _panelLayer = gameObject.GetComponentInChildren<UIPanelLayer>(true);
             if (_panelLayer == null)
             {
                 Debug.LogError("[UI Frame] UI Frame lacks panel layer");
             }
             else
             {
                 _panelLayer.Initialise();
             }
             Debug.Log("Finished initialising UI");
         }

         if (_windowLayer == null)
         {
             _windowLayer = gameObject.GetComponentInChildren<UIWindowLayer>(true);
             if (_windowLayer == null)
             {
                 Debug.LogError("[UI Frame] UI Frame lacks window layer");
             }
             else
             {
                 _windowLayer.Initialise();
                 //TODO screenblock priority stuff goes here
             }
         }

         graphicRaycaster = MainCanvas.GetComponent<GraphicRaycaster>();
     }

     /// <summary>
     /// Show a panel by its Id
     /// </summary>
     /// <param name="screenId">Id associated with panel</param>
     public void ShowPanel(string screenId)
     {
         _panelLayer.ShowScreenByID(screenId);
     }
     
     
     public void ShowPanel<T>(string screenId, T properties) where T: IUIPanelProperties
     {
         _panelLayer.ShowScreenByID<T>(screenId, properties);
     }

     public void HidePanel(string screenId)
     {
         _panelLayer.HideScreenByID(screenId);
     }

     public void OpenWindow(string screenId)
     {
         _windowLayer.ShowScreenByID(screenId);
     }

     public void OpenWindow<T>(string screenId, T properties) where T: IUIWindowProperties
     {
         _windowLayer.ShowScreenByID(screenId, properties);
     }

     public void CloseWindow(string screenId)
     {
        _windowLayer.HideScreenByID(screenId);
     }

     /// <summary>
     /// Close current window
     /// </summary>
     public void CloseCurrentWindow()
     {
         if (_windowLayer.currentWindow != null)
         {
             CloseWindow(_windowLayer.currentWindow.ScreenID);
         }
     }

     public void ShowScreen(string screenId)
     {
         Type type;
         if (IsScreenRegistered(screenId, out type))
         {
             if (type == typeof(IUIWindowController))
                 OpenWindow(screenId);
             else if (type == typeof(IUIPanelController))
                 ShowPanel(screenId);
         }
     }

     public void RegisterScreen(string screenId, IUIScreenController controller, Transform screenTransform)
     {
         IUIWindowController window = controller as IUIWindowController;
         if (window != null)
         {
             _windowLayer.RegisterScreen(screenId, window);
             if (screenTransform != null)
                 _windowLayer.ReparentScreen(controller, screenTransform);
             return;
         }
         IUIPanelController panel = controller as IUIPanelController;
         if (panel != null)
         {
             _panelLayer.RegisterScreen(screenId, panel);
             if (screenTransform != null)
                 _panelLayer.ReparentScreen(controller, screenTransform);
         }
     }
     public void RegisterPanel<TPanel>(string screenId, TPanel controller) where TPanel : IUIPanelController
     {
         _panelLayer.RegisterScreen(screenId, controller);
     }
     
     public void UnregisterPanel<TPanel>(string screenId, TPanel controller) where TPanel : IUIPanelController
     {
         _panelLayer.UnregisterScreen(screenId, controller);
     }
     
     public void RegisterWindow<TWindow>(string screenId, TWindow controller) where TWindow : IUIWindowController
     {
         _windowLayer.RegisterScreen(screenId, controller);
     }
     
     public void UnregisterWindow<TWindow>(string screenId, TWindow controller) where TWindow : IUIWindowController
     {
         _windowLayer.UnregisterScreen(screenId, controller);
     }
     
     public bool IsPanelOpen(string panelId)
     {
         return _panelLayer.IsPanelVisible(panelId);
     }
     
     public void HideAll(bool animate = true)
     {
         CloseAllWindows(animate);
         HideAllPanels(animate);
     }
     
     public void HideAllPanels(bool animate = true)
     {
         _panelLayer.HideAllScreens(animate);
     }
     
     public void CloseAllWindows(bool animate = true)
     {
         _windowLayer.HideAllScreens(animate);
             
     }
     
     /// <summary>
     /// Checks if a screen ID is assigned to the window or panel layers.
     /// </summary>
     /// <param name="screenId"></param>
     /// <returns></returns>
     public bool IsScreenRegistered(string screenId)
     {
         if (_windowLayer.IsScreenRegistered(screenId))
             return true;
         if (_panelLayer.IsScreenRegistered(screenId))
            return true;
         
         return false;
     }

     public bool IsScreenRegistered(string screenId, out Type type)
     {
         if (_windowLayer.IsScreenRegistered(screenId))
         {
             type = typeof(IUIWindowController);
             return true;
         }

         if (_panelLayer.IsScreenRegistered(screenId))
         {
             type = typeof(IUIPanelController);
             return true;
         }

         type = null;
         return false;
     }
     
}
