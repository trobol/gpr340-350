using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
	int score = 0;
	Particle2D particle;
	public float radius;
	public Text text;
	void Start()
	{
		particle = GetComponent<Particle2D>();
		float volume = transform.localScale.sqrMagnitude;
		float maxDepth = transform.lossyScale.y;
		BouyancyForceGenerator2D floatForce = new BouyancyForceGenerator2D(particle, volume, maxDepth, 0, 30);
		ForceManager.instance.Add(floatForce);
		setRandomPos();
	}

	void Update()
	{
		Particle2D[] particles = FindObjectsOfType<Particle2D>();
		foreach (Particle2D p in particles)
		{
			if (p != particle)
			{
				float distance = (particle.data.position - p.data.position).magnitude;
				if (distance < radius)
				{
					addScore();
					setRandomPos();
					break;
				}
			}
		}
	}

	void setRandomPos()
	{
		particle.data.position.x = Random.Range(-10, 20);
		particle.data.position.y = 9;
	}

	void addScore()
	{
		score++;
		text.text = score.ToString();
	}
}
