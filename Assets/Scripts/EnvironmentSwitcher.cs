using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSwitcher : MonoBehaviour
{

	public GameObject[] vignettes;
	int currentVignetteNum;
	public string key; 
	
    void Update()
    {
		if (Input.GetKeyDown(key)){
			Debug.Log("keyDown");
			NextVignette();
		}
        
    }
	
	public void NextVignette()
	{
		Debug.Log("switched to" + currentVignetteNum);
		vignettes[currentVignetteNum].SetActive(false);
		currentVignetteNum++; 
		if (currentVignetteNum == vignettes.Length)
		{
			currentVignetteNum = 0;
		}
		vignettes[currentVignetteNum].SetActive(true);
	}
}
