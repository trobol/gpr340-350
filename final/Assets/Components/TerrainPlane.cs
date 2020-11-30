using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlane : MonoBehaviour
{
	MeshRenderer mMeshRenderer;
	public Vector2Int mSize;

	void Start()
	{

	}
	[ContextMenu("Generate")]
	void GenerateMesh()
	{
		mMeshRenderer = GetComponent<MeshRenderer>();
		Mesh mesh = new Mesh();

		int vertexCount = (mSize.x + 1) * (mSize.y + 1);
		int squareCount = mSize.x * mSize.y;

		Vector3[] vertices = new Vector3[vertexCount];
		Vector2[] uvs = new Vector2[vertexCount];
		int[] triangles = new int[squareCount * 6];

		Vector2 tileUvSize = Vector2.one / (Vector2)mSize;
		for (int y = 0; y <= mSize.x; y++)
		{
			for (int x = 0; x <= mSize.y; x++)
			{
				int index = x + (y * mSize.x);
				vertices[index] = new Vector3(x, 0, y);
				uvs[index] = tileUvSize * new Vector2(x, y);
			}
		}
		for (int i = 0; i < squareCount; i++)
		{
			int index = i * 6;

			int offset = 0;

			int a0 = offset,
				a1 = offset + 1,
				a2 = a1 + mSize.x;

			int b0 = offset,
				b1 = offset + mSize.x,
				b2 = b1 + 1;

			triangles[index] = a0;
			triangles[index + 1] = a1;
			triangles[index + 2] = a2;


			triangles[index + 3] = b0;
			triangles[index + 4] = b1;
			triangles[index + 5] = b2;
		}
		mesh.vertices = vertices;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
