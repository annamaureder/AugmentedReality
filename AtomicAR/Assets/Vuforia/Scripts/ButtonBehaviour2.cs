using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ButtonBehaviour2 : MonoBehaviour, IVirtualButtonEventHandler {

	// Use this for initialization
	void Start () {
		GameObject virtualButton = GameObject.Find ("action2");
		virtualButton.GetComponent<VirtualButtonBehaviour> ().RegisterEventHandler (this);
	}

	public void OnButtonPressed(VirtualButtonAbstractBehaviour vb){

		MeshRenderer[] cubes = GameObject.Find("Cubes").GetComponentsInChildren<MeshRenderer>();
		Debug.Log ("number of cubes: " + cubes.Length);


		foreach(MeshRenderer cube in cubes){
			cube.material.color = new Color (1.0f/Random.Range(1,255), 1.0f/Random.Range(1,255), 1.0f/Random.Range(1,255));
		}

		Debug.Log ("Button 2 pressed");
	}

	public void OnButtonReleased(VirtualButtonAbstractBehaviour vb){
		Debug.Log ("Button 2 released");
	}
}
