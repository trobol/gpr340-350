using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactResolver : MonoBehaviour
{
	public static ContactResolver instance;
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Debug.LogError("More than one ContactResolver");
		}
	}
	void Update()
	{
		Particle2DLink[] links = FindObjectsOfType<Particle2DLink>();
		List<Particle2DContact> contacts = new List<Particle2DContact>();
		foreach (Particle2DLink link in links)
		{
			link.createContacts(contacts);
		}

		foreach (Particle2DContact contact in contacts)
		{
			contact.resolve(Time.deltaTime);
		}
	}
}
