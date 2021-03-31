
using UnityEngine;
using System.Collections;




public static class EnvironmentNoiseGenerator
{ 
	/// <summary>
	///		The noise normalizer mode, Either locally or globally 
	/// </summary>
	public enum Mode { Local, Global };

	/// <summary>
	///		Creates a noise map using width, height, NoiseSettings scriptable obj and a Vector2.
	/// </summary>
	/// <param name="Width"></param>
	/// <param name="Height"></param>
	/// <param name="Settings"></param>
	/// <param name="Center"></param>
	/// <returns></returns>
	public static float[,] CreateNoiseMap(int Width, int Height, TerrainNoiseSettings Settings, Vector2 Center)
	{
		float[,] newNoiseMap = new float[Width, Height];

		// Generate a randomly seeded  value 
		System.Random rand = new System.Random(Settings.seed);
		Vector2[] s_scalingFactorOffsets = new Vector2[Settings.scaleFactors];
	
		float maximumHeight = 0;
		float amplitude = 1;
		float freq = 1;

		float maximumLocalNoiseHeight = float.MinValue;
		float minimumLocalNoiseHeight = float.MaxValue;

		float halfWidth = Width / 2f;
		float halfHeight = Height / 2f;


		// Loop through the noise scaling factors offsets and adjust the offset settings with random index.
		for (int i = 0; i < Settings.scaleFactors; i++)
		{
			float offsetX = rand.Next(-100000, 100000) + Settings.offset.x + Center.x;
			float offsetY = rand.Next(-100000, 100000) + Settings.offset.y - Center.y;

			s_scalingFactorOffsets[i] = new Vector2(offsetX, offsetY);

			maximumHeight += amplitude;
			amplitude *= Settings.persistance;
		}
	
		

		for (int y = 0; y < Height; y++)
		{
			for (int x = 0; x < Width; x++)
			{

				amplitude = 1;
				freq = 1;

				float newSampleNoiseHeight = 0;

				for (int i = 0; i < Settings.scaleFactors; i++)
				{
						float sampleX = (x - halfWidth + s_scalingFactorOffsets[i].x) / Settings.scale * freq;
						float sampleY = (y - halfHeight + s_scalingFactorOffsets[i].y) / Settings.scale * freq;



					float perlinV = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;


						newSampleNoiseHeight += perlinV * amplitude;


						amplitude *= Settings.persistance;
						freq *= Settings.lacunarity;
				}

				if (newSampleNoiseHeight > maximumLocalNoiseHeight)
				{
					maximumLocalNoiseHeight = newSampleNoiseHeight;
				}

				if (newSampleNoiseHeight < minimumLocalNoiseHeight)
				{
					minimumLocalNoiseHeight = newSampleNoiseHeight;
				}


				newNoiseMap [x, y] = newSampleNoiseHeight;



				if (Settings.mode == Mode.Global)
				{
					float normalized = (newNoiseMap[x, y] + 1) / (maximumHeight / 0.9f);

					newNoiseMap[x, y] = Mathf.Clamp(normalized, 0, int.MaxValue);
				}
			}
		}

		if (Settings.mode == Mode.Local)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					newNoiseMap[x, y] = Mathf.InverseLerp(minimumLocalNoiseHeight, maximumLocalNoiseHeight, newNoiseMap[x, y]);
				}
			}
		}



		return newNoiseMap;
	}
}