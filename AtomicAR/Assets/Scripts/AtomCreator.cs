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

		if (Input.GetKeyDown (KeyCode.H)) {
			createAtom ("H", 1);
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			createAtom ("O", 6);
		}

		if (Input.GetKeyDown (KeyCode.N)) {
			createAtom ("Na", 1);
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			createAtom ("Cl", 7);
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
			if (Physics.Raycast (ray, out hit) && !Platform.isBlocked()) {

				tag = hit.collider.tag;
				create = true;

				if (tag == "H") {
					createAtom ("H", 1);
				}

				else if (tag == "O") {
					createAtom ("O", 6);
				}

				else if (tag == "Na") {
					createAtom ("Na", 1);
				}

				else if (tag == "Cl") {
					createAtom ("Cl", 7);
				}
			}

		} else if (create && touch.phase == TouchPhase.Ended) {
			create = false;
		}
	}

	void createAtom (string name, int electrons)
	{
		GameObject atom = Instantiate (atomPrefab);
		Atom myAtom = atom.GetComponent<Atom> ();
		myAtom.setVariables (name, electrons);
	}
}
