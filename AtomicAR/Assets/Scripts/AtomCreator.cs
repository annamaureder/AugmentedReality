using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomCreator : MonoBehaviour
{
	public GameObject atomPrefab;
	private bool create;
	private string tag;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Platform.isBlocked()) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.H)) {
			createAtom ("H", 1, new Color(0,0,0));
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			createAtom ("O", 8, new Color(0,0,0));
		}

		if (Input.GetKeyDown (KeyCode.N)) {
			createAtom ("Na", 11, new Color(0,0,0));
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			createAtom ("Cl", 17, new Color(0,0,0));
		}
			
		if (Input.touchCount != 1) {
			create = false; 
			return;
		}

		Touch touch = Input.touches [0];
		Vector2 pos = touch.position;

		if (touch.phase == TouchPhase.Began) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (pos); 
			if (Physics.Raycast (ray, out hit) && !Platform.isBlocked ()) {

				tag = hit.collider.tag;
				Color color = hit.collider.gameObject.GetComponent<Renderer> ().material.color;
				create = true;

				if (tag == "H") {
					createAtom ("H", 1, color);
				} else if (tag == "O") {
					createAtom ("O", 8, color);
				} else if (tag == "Na") {
					createAtom ("Na", 11, color);
				} else if (tag == "Cl") {
					createAtom ("Cl", 17, color);
				}
			} 

		} else if (create && touch.phase == TouchPhase.Ended) {
			create = false;
		}
	}

	void createAtom (string name, int electrons, Color color)
	{
		GameObject atom = Instantiate (atomPrefab);
		Atom myAtom = atom.GetComponent<Atom> ();
		myAtom.setVariables (name, electrons, color);
	}
}
