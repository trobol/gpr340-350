using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2DManager : MonoBehaviour
{
	public float width = 70;
	public float height = 60;
	public float time = 0.1f;
	float count = 0;
	GameObject particlePrefab;
	void Start()
	{
		particlePrefab = Resources.Load<GameObject>("Particle2D");
	}
	void Update()
	{
		count += Time.deltaTime;
		if (count >= time)
		{
			count = 0;
			Instantiate(particlePrefab, new Vector3(Random.Range(-width, width), Random.Range(0, height), 0), Quaternion.identity);
		}

		Particle2D[] particles = FindObjectsOfType<Particle2D>();
		foreach (Particle2D obj0 in particles)
		{
			foreach (Particle2D obj1 in particles)
			{
				if (obj0 == obj1) continue;
				if (CollitionDectector2D.DetectCollition(obj0, obj1))
				{
					Destroy(obj0.gameObject);
					Destroy(obj1.gameObject);
				}
			}
		}
	}
}
