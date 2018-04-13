using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomCreator : MonoBehaviour {

	public GameObject atomPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (KeyCode.H)) {
			createAtom ("H", 1);
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			createAtom ("O", 6);
		}
	}

	void createAtom(string name, int electrons){
		GameObject atom = Instantiate (atomPrefab);
		Atom myAtom = atom.GetComponent<Atom> ();
		myAtom.setVariables (name, electrons);
	}
}
