using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLayerWindow: UIWindowLayer
{
    public override void ShowScreen(IUIWindowController screen)
    {
        screen.Show();
        print("It works you fucker [WINDOW]");
    }

    public override void ShowScreen<TProps>(IUIWindowController screen, TProps properties)
    {
        throw new NotImplementedException();
    }

    public override void HideScreen(IUIWindowController screen)
    {
        screen.Hide();
        print("It's gone you fucker [WINDOW]");
    }

    private void Awake()
    {
        Initialise();
    }
}
