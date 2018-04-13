using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour {

	public GameObject H2O;
	public GameObject NaCl;

	List<Atom> atoms;
	Vector3 targetPosition;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void createSymbol(List<Atom> atoms, Vector3 targetPosition){

		this.atoms = atoms;
		this.targetPosition = targetPosition;

		setActive (false);

		H2O = Instantiate (H2O, targetPosition,new Quaternion());
	}

	public void splitElement(){

		H2O.SetActive(false);
		setActive (true);

		foreach(Atom atom in atoms){
			atom.setMoveAway (true);
			atom.setRing (true);
		}
	}

	void setActive(bool active){
		foreach(Atom atom in atoms){
			atom.setRenderer (active);
		}
	}
}
