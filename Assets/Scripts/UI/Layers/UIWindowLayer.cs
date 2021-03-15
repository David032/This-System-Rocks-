using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowLayer : AUILayer<IUIWindowController>
{
    public IUIWindowController currentWindow;
    public override void ShowScreen(IUIWindowController screen)
    {
        ShowScreen<IUIWindowProperties>(screen, null);
    }  
    public override void ShowScreen<TProp>(IUIWindowController screen, TProp properties)
    {
        ShowScreen<IUIWindowProperties>(screen, null);
    }

    public override void HideScreen(IUIWindowController screen)
    {
        if (screen == currentWindow)
        {
            screen.Hide();
            currentWindow = null;
        }
        else
        {
            Debug.LogError("[UIWindowLayer] No screen assigned to ID: " +screen.ScreenID);
        }
    } 
}