using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
	public static void integrate(ref Particle2DData particle, float dt)
	{
		Vector2 vel = particle.velocity * (float)dt;
		particle.position += new Vector2(vel.x, vel.y);

		Vector2 resultingAcc = particle.acceleration;
		//only accumulate forces if ignoreForces is false
		if (!particle.ignoreForces)//accumulate forces here
		{
			resultingAcc += particle.accumulatedForces * particle.mass;
		}

		particle.velocity += (resultingAcc * (float)dt);
		float damping = Mathf.Pow(particle.dampingConstant, (float)dt);
		particle.velocity *= damping;

		particle.accumulatedForces = Vector2.zero;
	}
}
