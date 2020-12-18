using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;



struct Edge
{
	public Edge(Vector3 a, Vector3 b, bool isOutside)
	{
		this.a = a;
		this.b = b;
		this.isOutside = isOutside;
	}
	public Vector3 a;
	public Vector3 b;
	public bool isOutside;
	public void Draw(Color color)
	{
		Debug.DrawLine(a, b, color);
	}
	float cross(Vector2 v0, Vector2 v1)
	{
		return (v0.x * v1.y - v0.y * v1.x);
	}
	public float GetIntersection(Ray ray)
	{
		Vector2 v1 = new Vector2(ray.origin.x, ray.origin.z) - new Vector2(a.x, a.z);
		Vector2 v2 = new Vector2(b.x, b.z) - new Vector2(a.x, a.z);
		Vector2 v3 = new Vector2(-ray.direction.z, ray.direction.x);


		float dot = Vector2.Dot(v2, v3);
		if (Math.Abs(dot) < 0.000001)
			return Mathf.NegativeInfinity;

		float t1 = ((v1.x * v2.y) - (v1.y * v2.x)) / dot;
		float t2 = Vector2.Dot(v1, v3) / dot;

		if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
			return t1;

		return Mathf.NegativeInfinity;
	}


	public Vector2? GetIntersectionU(Ray ray)
	{


		Vector2 q = new Vector2(a.x, a.z);
		Vector2 s = new Vector2(b.x, b.z) - q;

		// u = (q - p) x r / (r x s)
		Vector2 p = new Vector2(ray.origin.x, ray.origin.y);
		Vector2 r = new Vector2(ray.direction.x, ray.direction.z);


		float h = cross(r, s);
		float g = cross((q - p), r);
		float u = g / h;

		if (h >= 0.0 && (g >= 0.0 && g <= 1.0))
			return q + u * s;

		return null;
	}
}
struct Ring
{
	public Ring(Edge[] edges)
	{
		this.edges = edges;
	}
	public Edge[] edges;
	public Vector2? GetGetExitIntersection(Ray ray)
	{
		Vector2? vector = null;

		for (int i = 0; i < edges.Length; i++)
		{
			Vector2? v = edges[i].GetIntersectionU(ray);
			if (vector == null || (v != null && v.Value.magnitude > vector.Value.magnitude))
			{
				vector = v;
			}
		}
		return vector;
	}
	public void DrawOutline(Color color)
	{
		foreach (Edge e in edges)
		{
			e.Draw(color);
		}
	}

	public Vector3 GetCenter()
	{
		Vector3 center = Vector3.zero;
		foreach (Edge e in edges)
		{
			center += e.a + e.b;
		}
		center /= edges.Length * 2;
		return center;
	}
}

struct Grid
{
	List<Edge> edges;
	List<Ring> rings;
}

public class GridBuilder : MonoBehaviour
{
	struct GridPoint
	{
		public GridPoint(bool a, Vector3 p)
		{
			active = a;
			pos = p;
			isEdge = false;
		}
		public GridPoint(bool a)
		{
			active = a;
			pos = Vector3.zero;
			isEdge = false;
		}
		public bool active;
		public Vector3 pos;
		public bool isEdge;
	}
	public LayerMask mask;
	public float legHeight = 0.6f;
	public float effectiveHeight = 0.4f;
	public float totalHeight => legHeight + effectiveHeight;

	const int radius = 6;
	const int width = (radius * 2);
	public float speed;

	GridPoint[,] grid = new GridPoint[width, width];
	Ring[,] rings = new Ring[width - 1, width - 1];
	float stepSize = 1;
	const int maxHits = 30;
	bool drawRays = false;

	void Awake()
	{

	}
	void Update()
	{
		BuildGrid(transform.position);
		Vector2Int input = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));


		Vector3 direction = new Vector3(input.x, 0, input.y);

		transform.position += direction * speed * Time.deltaTime;
		Move(input, direction);

		if (Input.GetKeyDown(KeyCode.P))
			drawRays = !drawRays;
	}

	void Move(Vector2Int direction, Vector3 desiredMove)
	{
		desiredMove.y = 0;
		rings[radius - 1, radius - 1].DrawOutline(Color.red);
		Vector2Int pos = new Vector2Int(radius - 1, radius - 1);
		DrawX(grid[radius, radius].pos, 1, Color.green);
		pos += direction;
		if (pos.x < 0 || pos.x > width - 2 || pos.y < 0 || pos.y > width - 2) return;
		if (rings[pos.x, pos.y].edges.Length == 0) return;

		transform.position = Vector3.MoveTowards(transform.position, rings[pos.x, pos.y].GetCenter(), speed);

		/*

			Ring[] nearRings = getNearbyRings(radius - 1, radius - 1);

			foreach (Ring r in nearRings)
			{
				r.DrawOutline(Color.green);
			}
		*/
		/*
		Vector3 startRound = new Vector3(Mathf.Floor(start.x), start.y, Mathf.Floor(start.z));

		Ring[] nearRings = getNearbyRings(radius, radius);
		
		Vector2? vector = null;
		foreach (Ring r in nearRings)
		{
			Vector2? v = r.GetGetExitIntersection(new Ray(startRound, desiredMove));
			if (vector == null || (v != null && v.Value.magnitude > vector.Value.magnitude))
			{
				vector = v;
			}
		}

		if (vector != null)
			transform.parent.transform.position = vector.Value;
		*/
	}

	Ring[] getNearbyRings(int x, int y)
	{
		Ring[] result = new Ring[4];
		Vector2Int[] offsets = {
				new Vector2Int(0, -1),
				new Vector2Int(1, 0),
				new Vector2Int(0, 1),
				new Vector2Int(-1, 0)
		};
		int size = 0;
		for (int i = 0; i < 4; i++)
		{
			Vector2Int pos = new Vector2Int(x, y) + offsets[i];
			if (pos.x < 0 || pos.x > width - 2 || pos.y < 0 || pos.y > width - 2) continue;
			if (rings[pos.x, pos.y].edges.Length == 0) continue;
			result[size] = rings[pos.x, pos.y];
			size++;
		}
		Array.Resize(ref result, size);
		return result;
	}


	void BuildGrid(Vector3 position)
	{
		Physics.queriesHitBackfaces = true;
		RaycastHit[] hits = new RaycastHit[maxHits];
		Vector3 positionRound = new Vector3(Mathf.Floor(position.x), position.y, Mathf.Floor(position.z));
		positionRound += new Vector3(1, 0, 1);
		for (int z = -radius; z < radius; z++)
		{
			for (int x = -radius; x < radius; x++)
			{
				Vector3 pos = positionRound + new Vector3(x * stepSize, 0, z * stepSize);

				int size = CollideAll(hits, pos + (Vector3.up * 10000f));

				RaycastHit? result = ProcessHits(hits, size, pos);
				grid[x + radius, z + radius].active = (result != null);
				if (result != null)
				{
					DrawX(result.Value.point, 0.1f, Color.cyan);
					grid[x + radius, z + radius].pos = result.Value.point;

				}
				else
				{
					for (int i = 0; i < size; i++)
					{
						DrawX(hits[i].point, 0.1f, Color.red);
					}

				}
				if (drawRays)
				{
					Debug.DrawLine(pos + (Vector3.up * 10000f), pos + (Vector3.down * 10f), Color.red);
				}

			}
		}
		findEdgePoints();
		buildRings();
		drawRings();
	}
	GridPoint[] getSquarePoints(int x, int y)
	{

		GridPoint[] result = new GridPoint[4];
		Vector2Int[] offsets = {
			new Vector2Int(0, 1),
			new Vector2Int(1, 1),
			new Vector2Int(1, 0),
			new Vector2Int(0, 0)
		};
		int size = 0;
		for (int i = 0; i < 4; i++)
		{
			Vector2Int pos = new Vector2Int(x, y) + offsets[i];
			if (pos.x < 0 || pos.x > width - 1 || pos.y < 0 || pos.y > width - 1) continue;
			if (!grid[pos.x, pos.y].active) continue;
			result[size] = grid[pos.x, pos.y];
			size++;
		}
		Array.Resize(ref result, size);
		return result;

	}
	void findEdgePoints()
	{
		Vector2Int[] offsets = {
					new Vector2Int(0, -1),
					new Vector2Int(1, 0),
					new Vector2Int(0, 1),
					new Vector2Int(-1, 0)
				};
		for (int z = 0; z < width; z++)
		{
			for (int x = 0; x < width; x++)
			{
				grid[x, z].isEdge = false;
			}
		}
		for (int z = 0; z < width; z++)
		{
			for (int x = 0; x < width; x++)
			{
				if (!grid[x, z].active)
				{
					for (int i = 0; i < 4; i++)
					{
						Vector2Int pos = new Vector2Int(x, z) + offsets[i];
						if (pos.x < 0 || pos.x > width - 1 || pos.y < 0 || pos.y > width - 1) continue;

						grid[pos.x, pos.y].isEdge = true;
					}
				}
			}
		}
	}
	void drawRings()
	{
		for (int z = 0; z < width - 1; z++)
		{
			for (int x = 0; x < width - 1; x++)
			{
				Ring ring = rings[x, z];
				foreach (Edge e in ring.edges)
				{
					Color c = e.isOutside ? Color.red : Color.white;
					Debug.DrawLine(e.a, e.b, c);
				}
			}
		}
	}
	void buildRings()
	{
		// group points 4 at a time
		// connect valid points from heighest to lowest
		for (int z = 0; z < width - 1; z++)
		{
			for (int x = 0; x < width - 1; x++)
			{

				GridPoint[] square = getSquarePoints(x, z);
				GridPoint[] nearby = getNearbyPoints(x, z);

				Edge[] edges = new Edge[square.Length];

				for (int i = 0; i < square.Length; i++)
				{
					GridPoint a = square[i],
							  b = square[(i + 1) % square.Length];
					Color c = a.isEdge && b.isEdge ? Color.red : Color.white;
					edges[i] = new Edge(a.pos, b.pos, a.isEdge && b.isEdge);
				}
				rings[x, z] = new Ring(edges);
			}
		}
	}
	GridPoint[] getNearbyPoints(int x, int y)
	{
		GridPoint[] result = new GridPoint[4];
		Vector2Int[] offsets = {
			new Vector2Int(0, -1),
			new Vector2Int(1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(-1, 0)
		};
		int size = 0;
		for (int i = 0; i < 4; i++)
		{
			Vector2Int pos = new Vector2Int(x, y) + offsets[i];
			if (pos.x < 0 || pos.x > width - 1 || pos.y < 0 || pos.y > width - 1) continue;
			if (!grid[pos.x, pos.y].active) continue;
			result[size] = grid[pos.x, pos.y];
			size++;
		}
		Array.Resize(ref result, size);
		return result;
	}


	RaycastHit? ProcessHits(RaycastHit[] hits, int size, Vector3 pos)
	{
		if (size == 0) return null;

		float maxHeight = pos.y + legHeight;
		float minHeight = pos.y - legHeight;
		float headHeight = pos.y + totalHeight;
		for (int i = 0; i < size; i++)
		{
			RaycastHit hit = hits[i];
			Vector3 p = hit.point;

			if (p.y > maxHeight || p.y < minHeight) //cannot step up or down to point
				continue;
			if (i > 0 && hits[i - 1].point.y < headHeight) // cannot fit in space
				continue;

			return hit;
		}
		return null;
	}
	int CollideAll(RaycastHit[] output, Vector3 pos)
	{

		Vector3 current = pos;
		int i = 0;
		Ray ray = new Ray(current, Vector3.down);
		for (; i < maxHits; i++)
		{
			RaycastHit hit;

			bool found = Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
			if (!found) break;
			output[i] = hit;
			current = hit.point + Vector3.down * 0.001f;
		}
		return i;
	}
	void DrawX(Vector3 position, float size, Color color, float duration = 0.0f, bool depthTest = true)
	{
		Vector3 a0 = new Vector3(size, 0, size),
				b0 = new Vector3(-size, 0, -size),
				a1 = new Vector3(size, 0, -size),
				b1 = new Vector3(-size, 0, size);

		Debug.DrawLine(position + a0, position + b0, color, duration, depthTest);
		Debug.DrawLine(position + a1, position + b1, color, duration, depthTest);
	}
}
