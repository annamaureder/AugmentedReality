using UnityEngine;
using System.Collections;

public class DragAtoms : MonoBehaviour
{
	
	private float dist;
	private bool dragging = false;
	private Transform atom;
	private Vector3 pos3D;
	private float planeY = 0.15f;
	private float height = 0.15f;

	void Update ()
	{
		if (Input.touchCount != 1) {
			dragging = false; 
			return;
		}

		Touch touch = Input.touches [0];
		Vector2 pos = touch.position;

		if (touch.phase == TouchPhase.Began) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (pos); 
			if (Physics.Raycast (ray, out hit) && (hit.collider.tag == "Atom")) {
				atom = hit.transform;
				atom.gameObject.GetComponent<Atom> ().setIsDragged (true);
				dragging = true;
			}
		} else if (dragging && touch.phase == TouchPhase.Moved) {
			Ray ray = Camera.main.ScreenPointToRay (pos);
			Plane plane = new Plane (Vector3.up, Vector3.up * planeY);
			float distance;
			if (plane.Raycast (ray, out distance)) {
				atom.position = ray.GetPoint (distance);
				atom.position = new Vector3 (atom.position.x, atom.position.y, atom.position.z);
			}
		} else {
			atom.gameObject.GetComponent<Atom> ().setIsDragged (false);
		}
	}
}