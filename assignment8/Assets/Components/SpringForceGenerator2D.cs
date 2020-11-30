using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringForceGenerator2D : ForceGenerator2D
{
	Particle2D obj0, obj1;
	float springConstant;
	float restLength;

	public SpringForceGenerator2D(Particle2D obj0, Particle2D obj1, float springConstant, float restLength)
	{
		this.obj0 = obj0;
		this.obj1 = obj1;
		this.springConstant = springConstant;
		this.restLength = restLength;
	}
	public override void applyForce(Particle2D particle, float dt)
	{


		if (obj0 == null || obj1 == null)//either object no longer exists
			return;

		Vector2 pos0 = obj0.data.position;
		Vector2 pos1 = obj1.data.position;

		Vector2 diff = pos0 - pos1;
		float dist = diff.magnitude;

		float magnitude = dist - restLength;
		//if (magnitude < 0.0f)
		//magnitude = -magnitude;
		magnitude *= springConstant;

		diff.Normalize();
		diff *= -magnitude;

		obj0.data.accumulatedForces += diff;
		obj1.data.accumulatedForces += -diff;

	}
}
