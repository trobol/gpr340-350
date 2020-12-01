using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2DRod : Particle2DLink
{
	public float mLength = 0;
	public override void createContacts(List<Particle2DContact> contacts)
	{
		// Find the length of the rod.
		float currentLen = getCurrentLength();
		// Check whether we’re overextended.
		if (currentLen == mLength)
		{
			return;
		}

		// Calculate the normal.
		Vector2 normal = mObj1.data.position - mObj0.data.position;
		normal.Normalize();
		float pen = 0;
		// The contact normal depends on whether we’re extending
		// or compressing.
		if (currentLen > mLength)
		{
			pen = currentLen - mLength;
		}
		else
		{
			normal *= -1;
			pen = mLength - currentLen;
		}

		pen /= 1000.0f;
		Particle2DContact contact = new Particle2DContact(mObj0, mObj1, 0, normal, pen, Vector2.zero, Vector2.zero);

		contacts.Add(contact);
	}
}
