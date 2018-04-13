using UnityEngine;
using System.Collections;

public class DragAtoms : MonoBehaviour {
	
	private float dist;
	private bool dragging = false;
	private Transform atom;
	private Vector3 pos3D;

	void Update() {

		if (Input.touchCount != 1) {
			dragging = false; 
			return;
		}

		Touch touch = Input.touches[0];
		Vector2 pos = touch.position;

		if(touch.phase == TouchPhase.Began) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(pos); 
			if(Physics.Raycast(ray, out hit) && (hit.collider.tag == "Atom"))
			{
				Debug.Log ("Drag Atom");
				atom = hit.transform;
				dist = hit.transform.position.z - Camera.main.transform.position.z;
				pos3D = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, dist));
				dragging = true;
			}
		}
		else if (dragging && touch.phase == TouchPhase.Moved) {
			pos3D = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, dist));
			atom.position = pos3D;
		}
		else if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
			dragging = false;
		}
	}
}