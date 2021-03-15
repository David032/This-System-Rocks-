using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all panels that do not have properties
/// </summary>
public abstract class AUIPanelController
{
}

public abstract class AUIPanelController<T> : AUIScreenController<T>, IUIPanelController where T : IUIPanelProperties
{
    // protected override void SetProperties(T props)
    // {
    //     base.SetProperties(props);
    // }
}

