using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
///		Class for holding terrain mesh data 
/// </summary>
public class MeshData
{

	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;
	Vector3[] bakedNormals;

	Vector3[] outsideMeshVertices;
	int[] outsideMeshTriangles;

	int outsideMeshTriangleIndex;
	int currentTriangleIndex;


	public MeshData(int numVertsPerLine, int skipIncrement)
	{

		int numMeshEdgeVertices = (numVertsPerLine - 2) * 4 - 4;
		int numEdgeConnectionVertices = (skipIncrement - 1) * (numVertsPerLine - 5) / skipIncrement * 4;
		int numMainVerticesPerLine = (numVertsPerLine - 5) / skipIncrement + 1;
		int numMainVertices = numMainVerticesPerLine * numMainVerticesPerLine;

		vertices = new Vector3[numMeshEdgeVertices + numEdgeConnectionVertices + numMainVertices];
		uvs = new Vector2[vertices.Length];

		int numMeshEdgeTriangles = 8 * (numVertsPerLine - 4);
		int numMainTriangles = (numMainVerticesPerLine - 1) * (numMainVerticesPerLine - 1) * 2;
		triangles = new int[(numMeshEdgeTriangles + numMainTriangles) * 3];

		outsideMeshVertices = new Vector3[numVertsPerLine * 4 - 4];
		outsideMeshTriangles = new int[24 * (numVertsPerLine - 2)];
	}

	/// <summary>
	///		Creates a new vertex 
	/// </summary>
	/// <param name="vertexPosition"></param>
	/// <param name="uv"></param>
	/// <param name="vertexIndex"></param>
	public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
	{
		if (vertexIndex < 0)
		{
			outsideMeshVertices[-vertexIndex - 1] = vertexPosition;
		}
		else
		{
			vertices[vertexIndex] = vertexPosition;
			uvs[vertexIndex] = uv;
		}
	}

	/// <summary>
	///		Creates a triangle 
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	public void AddTriangle(int a, int b, int c)
	{
		if (a < 0 || b < 0 || c < 0)
		{
			outsideMeshTriangles[outsideMeshTriangleIndex] = a;
			outsideMeshTriangles[outsideMeshTriangleIndex + 1] = b;
			outsideMeshTriangles[outsideMeshTriangleIndex + 2] = c;
			outsideMeshTriangleIndex += 3;
		}
		else
		{
			triangles[currentTriangleIndex] = a;
			triangles[currentTriangleIndex + 1] = b;
			triangles[currentTriangleIndex + 2] = c;
			currentTriangleIndex += 3;
		}
	}

	/// <summary>
	///		Calculates the baked normals 
	/// </summary>
	/// <returns></returns>
	Vector3[] CalculateNormals()
	{

		Vector3[] vertexNormals = new Vector3[vertices.Length];
		int triangleCount = triangles.Length / 3;
		for (int i = 0; i < triangleCount; i++)
		{
			int normalTriangleIndex = i * 3;
			int vertexIndexA = triangles[normalTriangleIndex];
			int vertexIndexB = triangles[normalTriangleIndex + 1];
			int vertexIndexC = triangles[normalTriangleIndex + 2];

			Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			vertexNormals[vertexIndexA] += triangleNormal;
			vertexNormals[vertexIndexB] += triangleNormal;
			vertexNormals[vertexIndexC] += triangleNormal;
		}

		int borderTriangleCount = outsideMeshTriangles.Length / 3;
		for (int i = 0; i < borderTriangleCount; i++)
		{
			int normalTriangleIndex = i * 3;
			int vertexIndexA = outsideMeshTriangles[normalTriangleIndex];
			int vertexIndexB = outsideMeshTriangles[normalTriangleIndex + 1];
			int vertexIndexC = outsideMeshTriangles[normalTriangleIndex + 2];

			Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			if (vertexIndexA >= 0)
			{
				vertexNormals[vertexIndexA] += triangleNormal;
			}
			if (vertexIndexB >= 0)
			{
				vertexNormals[vertexIndexB] += triangleNormal;
			}
			if (vertexIndexC >= 0)
			{
				vertexNormals[vertexIndexC] += triangleNormal;
			}
		}


		for (int i = 0; i < vertexNormals.Length; i++)
		{
			vertexNormals[i].Normalize();
		}

		return vertexNormals;

	}

	/// <summary>
	///		Returns a vector3 surface normal using the 3 vertice points  
	/// </summary>
	/// <param name="indexA"></param>
	/// <param name="indexB"></param>
	/// <param name="indexC"></param>
	/// <returns></returns>
	Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
	{
		Vector3 pointA = (indexA < 0) ? outsideMeshVertices[-indexA - 1] : vertices[indexA];
		Vector3 pointB = (indexB < 0) ? outsideMeshVertices[-indexB - 1] : vertices[indexB];
		Vector3 pointC = (indexC < 0) ? outsideMeshVertices[-indexC - 1] : vertices[indexC];

		Vector3 sideAB = pointB - pointA;
		Vector3 sideAC = pointC - pointA;
		return Vector3.Cross(sideAB, sideAC).normalized;
	}

	/// <summary>
	///		Processes the mesh 
	/// </summary>
	public void ProcessMesh()
	{
		bakedNormals = CalculateNormals();
	}

	/// <summary>
	///  Creates a terrain mesh 
	/// </summary>
	/// <returns></returns>
	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = bakedNormals;
		return mesh;
	}

}