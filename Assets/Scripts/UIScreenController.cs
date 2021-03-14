using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

/// <summary>
/// Base implementation of UI Screens.
/// You should instead inherit from the children UIWindowController or UIPanelController.
/// </summary>
/// <typeparam name="TProps"></typeparam>
public abstract class UIScreenController<TProps> : MonoBehaviour, IUIScreenController where TProps : IUIScreenProperties
{
    /// <summary>
    /// Is this screen visible?
    /// </summary>
    public bool IsVisible { get; private set; }
    
    /// <summary>
    /// A string that is used to register the screen to a layer.
    /// </summary>
    public string ScreenID { get; set; }

    /// <summary>
    /// An optional data payload that can be customised from inspector.
    /// </summary>
    protected TProps Properties { get; set; }

    public void Show(IUIScreenPanelProperties properties = null)
    {
        if (properties != null)
        {
            if (properties is TProps props)
            {
                SetProperties(props);
            }
            else
            {
                Debug.LogError("Properties passed are wrong type. (" + props.GetType() + 
                               " Instead of (" +Properties.GetType());
                return;
            }
        }
        HierarchyFixOnShow();
        OnPropertiesSet();

        if (!gameObject.activeSelf)
        {
            //TODO: MOVE THIS INTO ANIMATION SYSTEM
            gameObject.SetActive(IsVisible);
            OnTransitionInFinished();
        }
        else
        {
            InTransitionFinished?.Invoke(this);
        }
    }

    public void Hide(bool animateOut = true)
    {
        //TODO: URGENT - FINISH IMPLEMENTING THIS
    }

    /// <summary>
    /// Deallocate for some reason ?
    /// </summary>
    protected virtual void OnDestroy()
    {
        ScreenDestroyed?.Invoke(this);

        InTransitionFinished = null;
        OutTransitionFinished = null;
        CloseRequest = null;
        ScreenDestroyed = null;
        RemoveListeners();
    }

    protected virtual void Awake()
    {
        AddListeners();
    }

    /// <summary>
    /// Invokes on Awake, adds listeners for events.
    /// </summary>
    protected virtual void AddListeners()
    {throw new NotImplementedException();}

    /// <summary>
    /// Invokes on OnDestroy, removes all listeners.
    /// </summary>
    protected virtual void RemoveListeners()
    {
        throw new NotImplementedException();}
    /// <summary>
    /// Invokes if properties are passed in. Allows access to properties.
    /// </summary>
    protected virtual void OnPropertiesSet()
    {throw new NotImplementedException();}
    /// <summary>
    /// Invokes while told to Hide by parent Layer.
    /// </summary>
    protected virtual void WhileHiding()
    {throw new NotImplementedException();}

    protected virtual void SetProperties(TProps props)
    {
        Properties = props;
    }

    /// <summary>
    /// If screen needs to execute behaviour when Hierarchy is adjusted
    /// </summary>
    protected virtual void HierarchyFixOnShow()
    {}

    // TODO: Add animation method calls and delegates for future.
    
    /// <summary>
    /// Occurs when the In Transition has finished.
    /// </summary>
    public Action<IUIScreenController> InTransitionFinished { get; set; }

    private void OnTransitionInFinished()
    {
        IsVisible = true;
        InTransitionFinished?.Invoke(this);
    }
    /// <summary>
    /// Occurs when the Out Transition has finished.
    /// </summary>
    public Action<IUIScreenController> OutTransitionFinished { get; set; }

    private void OnTransitionOutFinished()
    {
        IsVisible = false;
        OutTransitionFinished?.Invoke(this);
    }
    /// <summary>
    /// Screen sends this to its parent layer to be closed
    /// </summary>
    public Action<IUIScreenController> CloseRequest { get; set; }
    /// <summary>
    /// If the screen is destroyed, it must warn the LayerController.
    /// </summary>
    public Action<IUIScreenController> ScreenDestroyed { get; set; }
}
