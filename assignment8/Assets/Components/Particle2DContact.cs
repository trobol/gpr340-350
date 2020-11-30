using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2DContact
{
	Particle2D mObj0, mObj1;
	float mRestitutionCoefficient;
	Vector2 mContactNormal;
	float mPenetration;
	Vector2 mMove0, mMove1;

	public Particle2DContact(Particle2D obj0, Particle2D obj1, float restitution, Vector2 normal, float pen, Vector2 move0, Vector2 move1)
	{
		mObj0 = obj0;
		mObj1 = obj1;
		mRestitutionCoefficient = restitution;
		mContactNormal = normal;
		mPenetration = pen;
		mMove0 = move0;
		mMove1 = move1;
	}
	public void resolve(float dt)
	{
		resolveVelocity(dt);
		resolveInterpenetration(dt);
	}

	float calculateSeparatingVelocity()
	{
		Vector2 relativeVel = mObj0.data.velocity;
		if (mObj1)
		{
			relativeVel -= mObj1.data.velocity;
		}
		return Vector2.Dot(relativeVel, mContactNormal);
	}
	void resolveVelocity(float dt)
	{
		float separatingVel = calculateSeparatingVelocity();
		if (separatingVel > 0.0f)//already separating so need to resolve
			return;

		float newSepVel = -separatingVel * mRestitutionCoefficient;

		Vector2 velFromAcc = mObj0.data.acceleration;
		if (mObj1)
			velFromAcc -= mObj1.data.acceleration;
		float accCausedSepVelocity = Vector2.Dot(velFromAcc, mContactNormal) * dt;

		if (accCausedSepVelocity < 0.0f)
		{
			newSepVel += mRestitutionCoefficient * accCausedSepVelocity;
			if (newSepVel < 0.0f)
				newSepVel = 0.0f;
		}

		float deltaVel = newSepVel - separatingVel;

		float totalInverseMass = mObj0.data.inverseMass;
		if (mObj1)
			totalInverseMass += mObj1.data.inverseMass;

		if (totalInverseMass <= 0)//all infinite massed objects
			return;

		float impulse = deltaVel / totalInverseMass;
		Vector2 impulsePerIMass = mContactNormal * impulse;

		Vector2 newVelocity0 = mObj0.data.velocity + impulsePerIMass * mObj0.data.inverseMass;
		mObj0.data.velocity = newVelocity0;
		if (mObj1)
		{
			Vector2 newVelocity1 = mObj1.data.velocity + impulsePerIMass * -mObj1.data.inverseMass;
			mObj1.data.velocity = newVelocity1;
		}
	}
	void resolveInterpenetration(double dt)
	{
		if (mPenetration <= 0.0f)
			return;

		float totalInverseMass = mObj0.data.inverseMass;
		if (mObj1)
			totalInverseMass += mObj1.data.inverseMass;

		if (totalInverseMass <= 0)//all infinite massed objects
			return;

		Vector2 movePerIMass = mContactNormal * (mPenetration / totalInverseMass);

		mMove1 = movePerIMass * mObj0.data.inverseMass;
		if (mObj1)
			mMove1 = movePerIMass * -mObj1.data.inverseMass;
		else
			mMove1 = Vector2.zero;

		Vector2 newPosition0 = mObj0.data.position + mMove0;
		mObj0.data.position = newPosition0;
		if (mObj1)
		{
			Vector2 newPosition1 = mObj1.data.position + mMove1;
			mObj1.data.position = newPosition1;
		}
	}
}
