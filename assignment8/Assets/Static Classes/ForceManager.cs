using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManager : MonoBehaviour
{
	public static ForceManager _instance;
	public static ForceManager instance => _instance;
	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}

	}
	List<ForceGenerator2D> mGenerators = new List<ForceGenerator2D>();
	public void Add(ForceGenerator2D generator)
	{
		mGenerators.Add(generator);
	}
	public void Delete(ForceGenerator2D generator)
	{
		mGenerators.Remove(generator);
	}

	void Update()
	{
		float dt = Time.deltaTime;
		Particle2D[] particles = GameObject.FindObjectsOfType<Particle2D>();

		foreach (ForceGenerator2D generator in mGenerators)
		{
			if (generator.shouldEffectAll)
			{
				foreach (Particle2D particle in particles)
				{
					if (!particle.data.ignoreForces)
						generator.applyForce(particle, dt);
				}
			}
			else
			{
				generator.applyForce(null, dt);
			}

		}
	}

}
