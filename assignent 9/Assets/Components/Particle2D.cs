using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Particle2DData
{
	public float inverseMass;
	public Vector2 velocity;
	public Vector2 accumulatedForces;
	public Vector2 acceleration;
	public float dampingConstant;
	public bool ignoreForces;
	public Vector2 position;
	public float mass
	{
		get => inverseMass > 0 ? 1.0f / inverseMass : float.MaxValue;
		set
		{
			inverseMass = 1.0f / value;
		}
	}
}
public class Particle2D : MonoBehaviour
{
	public Particle2DData data;

	void Start()
	{
		data.position = transform.position;
	}

	void Update()
	{
		Integrator.integrate(ref data, Time.deltaTime);
		transform.position = data.position;
	}
}


