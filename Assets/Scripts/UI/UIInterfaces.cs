using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI Interfaces.
/// "Screens" are an instance of either of the below UI elements.
/// "Panels" are small UI elements to constantly display stats.
/// "Windows" are invasive UI elements such as menus.
/// </summary>

public interface IUIScreenProperties { }


public interface IUIPanelController : IUIScreenController
{
     //TODO enum for sorting priority of panels
}
public interface IUIPanelProperties : IUIScreenProperties
{
     IUIScreenProperties Priority { get; set; }
}

public interface IUIWindowController : IUIScreenController
{
     // For "Dialogs", called "Popups" in this project.
     bool isPopup { get; set; }
}

public interface IUIWindowProperties : IUIScreenProperties
{
     bool IsPopup { get; }
}
/// <summary>
/// Must be implemented by all Screens directly or indirectly
/// </summary>
public interface IUIScreenController
{
     string ScreenID { get; set;}
     bool IsVisible { get; }

     void Show(IUIScreenProperties properties = null);
     void Hide(bool animate = true);
     
     Action<IUIScreenController>  InTransitionFinished { get; set; }
     Action<IUIScreenController>  OutTransitionFinished { get; set; }
     Action<IUIScreenController>  CloseRequest { get; set; }
     Action<IUIScreenController>  ScreenDestroyed { get; set; }

}
