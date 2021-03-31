using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Height Map Settings", menuName = "Environment/Settings")]
public class HeightMapSettings : UpdatableData {

    public TerrainNoiseSettings noiseSettings;

    public bool useFalloff;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    /// <summary>
    ///     Minimum Height Animation Curve Value 
    /// </summary>
    public float minimumHeight
    {
        get
        {
            return heightMultiplier * heightCurve.Evaluate(0);
        }
    }

    /// <summary>
    ///     Maximum Height Animation Curve Value 
    /// </summary>
    public float maximumHeight
    {
        get
        {
            return heightMultiplier * heightCurve.Evaluate(1);
        }
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        noiseSettings.ValidateValues();
        base.OnValidate();
    }
#endif
}