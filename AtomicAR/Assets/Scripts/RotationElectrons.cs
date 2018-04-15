using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationElectrons : MonoBehaviour {

	private List<Renderer> rings;
	private float angle = 2.0f;
	private float electronSpeed = 5.0f;
	private Vector3[] rotations;

	// Use this for initialization
	void Start () {
		
		rings = gameObject.GetComponent<Atom> ().getRings ();
		rotations = new Vector3[rings.Count];

		for(int i = 0; i < rotations.Length; i++){
			rotations[i] = new Vector3(Random.Range(0,1.0f),Random.Range(0,1.0f),Random.Range(0,1.0f));
		}
	}
	
	// Update is called once per frame
	void Update () {



		for(int i = 0; i < rings.Count; i++){
			rings [i].gameObject.transform.RotateAround (rings [i].gameObject.transform.position, rotations [i], angle);
			rings [i].gameObject.transform.RotateAround (rings [i].gameObject.transform.position, rings[i].gameObject.transform.up, electronSpeed);
		}
		
	}
}
