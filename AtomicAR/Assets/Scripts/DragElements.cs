using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragElements : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private float dist;
	private bool dragging = false;
	private Transform element;
	private Vector3 pos3D;
	private float planeY = 0;
	private float height = 0.15f;

	float counter = 1.0f;

	void Update ()
	{
		if (Input.touchCount != 1) {
			dragging = false; 
			return;
		}

		Touch touch = Input.touches [0];
		Vector2 pos = touch.position;

		if (touch.phase == TouchPhase.Began) {

			if (counter < 0.5f) {
				element.GetComponent<Element> ().splitElement ();
			}

			counter = 0.0f;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (pos); 
			if (Physics.Raycast (ray, out hit) && (hit.collider.tag == "Element")) {
				element = hit.transform;
				dragging = true;
			}
		} else if (dragging && touch.phase == TouchPhase.Moved) {
			Ray ray = Camera.main.ScreenPointToRay (pos);
			Plane plane = new Plane (Vector3.up, Vector3.up * planeY);
			float distance;
			if (plane.Raycast (ray, out distance)) {
				element.position = ray.GetPoint (distance);
				element.position = new Vector3 (element.position.x, element.position.y + height, element.position.z);
			}
		} else if(touch.phase == TouchPhase.Ended){
			
			//atom.gameObject.GetComponent<Atom> ().setIsDragged (false);
		}

		counter += Time.deltaTime;
	}
}
