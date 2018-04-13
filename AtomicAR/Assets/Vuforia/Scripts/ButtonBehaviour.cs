using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ButtonBehaviour : MonoBehaviour, IVirtualButtonEventHandler {

	private float move = 0.1f;

	// Use this for initialization
	void Start () {
		GameObject virtualButton = GameObject.Find ("action");
		virtualButton.GetComponent<VirtualButtonBehaviour> ().RegisterEventHandler (this);
	}
	
	public void OnButtonPressed(VirtualButtonAbstractBehaviour vb){

		Transform[] cubes = GameObject.Find("Cubes").GetComponentsInChildren<Transform>();

		foreach(Transform cube in cubes){
			cube.transform.position = new Vector3 (cube.transform.position.x, cube.transform.position.y + move, cube.transform.position.z);
		}

		Debug.Log ("Button pressed");
	}

	public void OnButtonReleased(VirtualButtonAbstractBehaviour vb){
		Debug.Log ("Button released");
		move = -move;
	}
}
