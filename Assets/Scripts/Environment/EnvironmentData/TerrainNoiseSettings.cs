using UnityEngine;

/// <summary>
///    Noise map data class 
/// </summary>
public class TerrainNoiseSettings
{

    /// <summary>
    ///     The Noisemap Generator Normalization Mode Enum
    ///     Can be Local or Global 
    /// </summary>
    public EnvironmentNoiseGenerator.Mode mode;

    /// <summary>
    ///     The noise map scale 
    /// </summary>
    public float scale = 50;

    /// <summary>
    ///     The noise scaling factor 
    /// </summary>
    public int scaleFactors = 6;

    /// <summary>
    ///     Clamped noise persistence 
    /// </summary>
    [Range(0, 1)]
    public float persistance = 0.6f;
   
    /// <summary>
    ///     Lacunarity (Sharpness) 
    /// </summary>
    public float lacunarity = 2;
    
    /// <summary>
    ///  Randomly Generated value for terrain 
    /// </summary>
    public int seed;

    /// <summary>
    /// The terrain noise offset vector2 for the x & y axis 
    /// </summary>
    public Vector2 offset;

    public void ValidateValues()
	{
        scale = Mathf.Max(scale, 0.01f);
        scaleFactors = Mathf.Max(scaleFactors, 1);
        persistance = Mathf.Clamp01(persistance);
        lacunarity = Mathf.Max(lacunarity, 1);
	}
}
