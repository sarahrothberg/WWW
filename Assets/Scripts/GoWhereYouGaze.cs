using UnityEngine;
using System.Collections;

public class GoWhereYouGaze : MonoBehaviour
{
	//note: if you are using a VR camera, the cameras position and rotation will be controlled
	//by the user's position and head rotation, so you must nest your camera under a "player" game object

	public GameObject player;
	public float thrust;

	void Update()
	{

		//player.transform.position += Camera.main.transform.forward * thrust;

		//every time user holds down trigger, move forward by thrust amount
		//if (Input.anyKey)
		//{
		//	player.transform.position += Camera.main.transform.forward * thrust;

		//	to only move on the x/ z axis(no flying) : 
		//	Vector3 playerPosition = player.transform.position;
		//	playerPosition.x += Camera.main.transform.forward.x * thrust;
		//	playerPosition.z += Camera.main.transform.forward.z * thrust;
		//	player.transform.position = playerPosition;

		//}

		//replace the above code with the following if you'd like your player to also use the physics engine
		//your "player" game object will need to have a rigidbody component and a collider for this to work, with gravity ticked "off" and with the x/y/z rotation constrained


		if (Input.anyKey)
		{
			player.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * thrust);
		}
		else
		{
			player.GetComponent<Rigidbody>().velocity = (new Vector3(0, 0, 0));

		}

	}
}