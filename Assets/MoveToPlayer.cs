using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
	public GameObject scene;
	public GameObject player; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("a"))
		{
			Debug.Log("realign scene");
			Vector3 tempPos;
			Quaternion tempRot; 
			tempPos.x = player.transform.position.x;
			tempPos.z = player.transform.position.z;
			//tempRot.x = player.transform.rotation.x;
			//tempRot.z = player.transform.rotation.z;
			scene.transform.position = new Vector3 (tempPos.x, 0, tempPos.z);
		//	scene.transform.rotation = new Quaternion(tempRot.x, 0, tempRot.z, 0);
		}
    }
}
