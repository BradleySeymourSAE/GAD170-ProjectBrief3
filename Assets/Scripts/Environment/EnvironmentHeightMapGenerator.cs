using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
///		Static class for creating a height map using perlin noise 
/// </summary>
public static class EnvironmentHeightMapGenerator
{
		public static HeightMap CreateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre)
		{
			float[,] values = EnvironmentNoiseGenerator.CreateNoiseMap(width, height, settings.noiseSettings, sampleCentre);

			AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);

			float minimumValue = float.MaxValue;
			float maximumValue = float.MinValue;

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					values[i, j] *= heightCurve_threadsafe.Evaluate(values[i, j]) * settings.heightMultiplier;

					if (values[i, j] > maximumValue)
					{
						maximumValue = values[i, j];
					}
					if (values[i, j] < minimumValue)
					{
						minimumValue = values[i, j];
					}
				}
			}

			return new HeightMap(values, minimumValue, maximumValue);
	}
}