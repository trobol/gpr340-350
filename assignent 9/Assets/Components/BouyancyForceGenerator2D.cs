using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyancyForceGenerator2D : ForceGenerator2D
{
	float mVolume;
	float mMaxDepth;
	float mLiquidHeight;
	float mLiquidDensity;
	Particle2D mParticle;
	public BouyancyForceGenerator2D(Particle2D particle, float volume, float maxDepth, float liquidHeight, float liquidDensity)
	{
		mParticle = particle;
		mVolume = volume;
		mMaxDepth = maxDepth;
		mLiquidHeight = liquidHeight;
		mLiquidDensity = liquidDensity;
		shouldEffectAll = false;
	}
	public override void applyForce(Particle2D particle, float dt)
	{

		if (particle != null) return;

		if (mParticle == null)
		{
			return;
		}
		ref Particle2DData data = ref mParticle.data;

		Vector2 pos = data.position;

		float depth = pos.y;

		if (depth >= mLiquidHeight + mMaxDepth)
		{
			return;
		}
		float force;
		// Check if we’re at maximum depth.
		if (depth <= mLiquidHeight - mMaxDepth)
		{
			force = mLiquidDensity * mVolume;
		}
		else
		{
			// Otherwise we are partly submerged.
			force = mLiquidDensity * mVolume *
				(depth - mMaxDepth - mLiquidHeight) / 2 * mMaxDepth;
		}
		data.accumulatedForces += new Vector2(0, Mathf.Abs(force));
	}

}
