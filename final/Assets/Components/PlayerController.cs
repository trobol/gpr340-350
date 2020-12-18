using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float legHeight;
	public float effectiveHeight;
	public float totalHeight => legHeight + effectiveHeight;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		transform.position += new Vector3(input.x, 0, input.y) * speed * Time.deltaTime;
	}

}
