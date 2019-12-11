using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

	public GameObject objectTrigger;

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("entered trigger " + this.gameObject.name);
		if (objectTrigger != null && other.tag == "Player")
		{
			objectTrigger.SetActive(true);
			Destroy(this.gameObject, 1);
		}
		else
		{
			Debug.Log("You have not assigned a game object in the inspector!");
		}
	}

	public void OnTriggerExit(Collider other)
	{
		Debug.Log("exit trigger" + this.gameObject.name);
		if (objectTrigger != null)
		{
			//objectTrigger.SetActive(false);
		}
	}
}