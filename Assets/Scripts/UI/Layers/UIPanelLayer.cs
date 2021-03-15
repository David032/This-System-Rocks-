using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This layer controls panels
/// Panels are Screens that are simply shown and hidden.
/// I.e HUD, minimap.
/// </summary>
public class UIPanelLayer : AUILayer<IUIPanelController>
{
    public override void ShowScreen(IUIPanelController panel)
    {
        panel.Show();
    }

    public override void ShowScreen<TProps>(IUIPanelController panel, TProps properties)
    {
        panel.Show(properties);
    }

    public override void HideScreen(IUIPanelController panel)
    {
        panel.Hide();
    }

    public bool IsPanelVisible(string panelId)
    {
        IUIPanelController _panel;
        //TODO LEARN WHAT THE HECK OUT MEANS
        if (registeredScreens.TryGetValue(panelId, out _panel))
        {
            return _panel.IsVisible;
        }

        return false;
    }
}
