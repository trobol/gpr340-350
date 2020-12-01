using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Particle2DLink : MonoBehaviour
{
	[HideInInspector]
	public Particle2D mObj0;
	public Particle2D mObj1;
	void Start()
	{
		mObj0 = GetComponent<Particle2D>();
	}
	public abstract void createContacts(List<Particle2DContact> contacts);

	protected float getCurrentLength()
	{
		float distance = (mObj0.data.position - mObj1.data.position).magnitude;
		return distance;
	}
}
