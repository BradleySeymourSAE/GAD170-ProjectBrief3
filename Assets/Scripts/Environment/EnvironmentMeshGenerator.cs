using System.Collections;
using UnityEngine;


/// <summary>
///		Static class for creating terrain mesh 
/// </summary>
public static class EnvironmentMeshGenerator
{
	/// <summary>
	///		Creates a terrain mesh using a HeightMap x,y values, Mesh Settings & Level Of Detail 
	/// </summary>
	/// <param name="HeightMap"></param>
	/// <param name="Settings"></param>
	/// <param name="DetailLevel"></param>
	/// <returns></returns>
	public static MeshData CreateTerrainMesh(float[,] HeightMap, MeshSettings Settings, int DetailLevel)
	{
		int skipDetailIncrement = (DetailLevel == 0) ? 1 : DetailLevel * 2;
		int totalMeshVerticesPerLine = Settings.NumberOfVerticesPerLine;

		Vector2 topLeft = new Vector2(-1, 1) * Settings.MeshWorldSize / 2f;

		MeshData meshData = new MeshData(totalMeshVerticesPerLine, skipDetailIncrement);

		int[,] vertexIndicesMap = new int[totalMeshVerticesPerLine, totalMeshVerticesPerLine];
		int meshVertexIndex = 0;
		int outOfMeshVertexIndex = -1;

		for (int y = 0; y < totalMeshVerticesPerLine; y++)
		{
			for (int x = 0; x < totalMeshVerticesPerLine; x++)
			{
				bool isOutOfMeshVertex = y == 0 || y == totalMeshVerticesPerLine - 1 || x == 0 || x == totalMeshVerticesPerLine - 1;
				bool isSkippedVertex = x > 2 && x < totalMeshVerticesPerLine - 3 && y > 2 && y < totalMeshVerticesPerLine - 3 && ((x - 2) % skipDetailIncrement != 0 || (y - 2) % skipDetailIncrement != 0);
				if (isOutOfMeshVertex)
				{
					vertexIndicesMap[x, y] = outOfMeshVertexIndex;
					outOfMeshVertexIndex--;
				}
				else if (!isSkippedVertex)
				{
					vertexIndicesMap[x, y] = meshVertexIndex;
					meshVertexIndex++;
				}
			}
		}

		for (int y = 0; y < totalMeshVerticesPerLine; y++)
		{
			for (int x = 0; x < totalMeshVerticesPerLine; x++)
			{
				bool isSkippedVertex = x > 2 && x < totalMeshVerticesPerLine - 3 && y > 2 && y < totalMeshVerticesPerLine - 3 && ((x - 2) % skipDetailIncrement != 0 || (y - 2) % skipDetailIncrement != 0);

				if (!isSkippedVertex)
				{
					bool isOutOfMeshVertex = y == 0 || y == totalMeshVerticesPerLine - 1 || x == 0 || x == totalMeshVerticesPerLine - 1;
					bool isMeshEdgeVertex = (y == 1 || y == totalMeshVerticesPerLine - 2 || x == 1 || x == totalMeshVerticesPerLine - 2) && !isOutOfMeshVertex;
					bool isMainVertex = (x - 2) % skipDetailIncrement == 0 && (y - 2) % skipDetailIncrement == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
					bool isEdgeConnectionVertex = (y == 2 || y == totalMeshVerticesPerLine - 3 || x == 2 || x == totalMeshVerticesPerLine - 3) && !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

					int vertexIndex = vertexIndicesMap[x, y];
					Vector2 percent = new Vector2(x - 1, y - 1) / (totalMeshVerticesPerLine - 3);
					Vector2 vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * Settings.MeshWorldSize;
					float height = HeightMap[x, y];

					if (isEdgeConnectionVertex)
					{
						bool isVertical = x == 2 || x == totalMeshVerticesPerLine - 3;
						int dstToMainVertexA = ((isVertical) ? y - 2 : x - 2) % skipDetailIncrement;
						int dstToMainVertexB = skipDetailIncrement - dstToMainVertexA;
						float dstPercentFromAToB = dstToMainVertexA / (float)skipDetailIncrement;

						float heightMainVertexA = HeightMap[(isVertical) ? x : x - dstToMainVertexA, (isVertical) ? y - dstToMainVertexA : y];
						float heightMainVertexB = HeightMap[(isVertical) ? x : x + dstToMainVertexB, (isVertical) ? y + dstToMainVertexB : y];

						height = heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB;
					}

					meshData.AddVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), percent, vertexIndex);

					bool createTriangle = x < totalMeshVerticesPerLine - 1 && y < totalMeshVerticesPerLine - 1 && (!isEdgeConnectionVertex || (x != 2 && y != 2));

					if (createTriangle)
					{
						int currentIncrement = (isMainVertex && x != totalMeshVerticesPerLine - 3 && y != totalMeshVerticesPerLine - 3) ? skipDetailIncrement : 1;

						int a = vertexIndicesMap[x, y];
						int b = vertexIndicesMap[x + currentIncrement, y];
						int c = vertexIndicesMap[x, y + currentIncrement];
						int d = vertexIndicesMap[x + currentIncrement, y + currentIncrement];
						meshData.AddTriangle(a, d, c);
						meshData.AddTriangle(d, a, b);
					}
				}
			}
		}

		meshData.ProcessMesh();

		return meshData;
	}
}