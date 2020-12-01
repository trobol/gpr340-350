using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForceGenerator2D
{
	public bool shouldEffectAll = true;
	public abstract void applyForce(Particle2D particle, float dt);
}
