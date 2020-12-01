using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	public float mRotation = 0;
	float mSensitivity = 1.0f;
	GameObject mRodPrefab;
	GameObject mSpringPrefab;
	bool projType = false;
	void Start()
	{
		mRodPrefab = Resources.Load<GameObject>("RodProjectile");
		mSpringPrefab = Resources.Load<GameObject>("SpringProjectile");
	}

	// Update is called once per frame
	void Update()
	{
		float amt = Input.GetAxis("Rotation");
		mRotation += amt * mSensitivity;
		mRotation = Mathf.Clamp(mRotation, -90, 90);
		transform.rotation = Quaternion.AngleAxis(mRotation, Vector3.forward);

		float sin = Mathf.Sin(mRotation * Mathf.Deg2Rad);
		float cos = Mathf.Cos(mRotation * Mathf.Deg2Rad);
		Vector2 fireDirection = new Vector2(cos, sin);

		if (Input.GetKeyDown(KeyCode.Return))
		{

			createProjectile(fireDirection);
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			projType = !projType;
		}

	}

	void createProjectile(Vector2 direction)
	{


		GameObject obj = Instantiate(projType ? mRodPrefab : mSpringPrefab, transform.position + (Vector3)direction, Quaternion.identity);
		Particle2D particle0 = obj.transform.GetChild(0).GetComponent<Particle2D>();
		Particle2D particle1 = obj.transform.GetChild(1).GetComponent<Particle2D>();
		ref Particle2DData data = ref particle0.data;
		data.velocity = direction * 30;

		ref Particle2DData data1 = ref particle1.data;
		data1.velocity = direction * 32;

		if (!projType)
		{
			SpringForceGenerator2D springForce = new SpringForceGenerator2D(particle0, particle1, 3, 2);
			ForceManager.instance.Add(springForce);
		}

		float volume = obj.transform.localScale.sqrMagnitude;
		float maxDepth = obj.transform.lossyScale.y;

		BouyancyForceGenerator2D floatForce0 = new BouyancyForceGenerator2D(particle0, volume, maxDepth, 0, 30);
		ForceManager.instance.Add(floatForce0);
		BouyancyForceGenerator2D floatForce1 = new BouyancyForceGenerator2D(particle1, volume, maxDepth, 0, 30);
		ForceManager.instance.Add(floatForce1);



	}
}
