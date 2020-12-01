using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollitionDectector2D
{
	public static bool DetectCollition(Particle2D obj0, Particle2D obj1)
	{
		float distance = (obj0.data.position - obj1.data.position).magnitude;
		return distance <= 1;
	}
}
