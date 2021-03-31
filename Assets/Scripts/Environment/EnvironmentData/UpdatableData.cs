using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class UpdatableData : ScriptableObject {

    public event Action OnValuesUpdatedEvent;
    public bool AutoUpdate;


    #if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        if (AutoUpdate)
        {
           EditorApplication.update += NotifyOfUpdatedValues;
        }
    }

    /// <summary>
    ///    Updated values event listener.
    /// </summary>
    public void NotifyOfUpdatedValues()
    {
        EditorApplication.update -= NotifyOfUpdatedValues;

        if (OnValuesUpdatedEvent != null)
        {
            // Update the values 
            OnValuesUpdatedEvent();
        }
    }

    #endif
}