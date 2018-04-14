using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCreator : MonoBehaviour
{


	private GameObject element;
	public GameObject H2Oprefab;
	public GameObject H2prefab;
	public GameObject NaClprefab;
	private int electrons = 0;

	List<Atom> atoms;
	Vector3 targetPosition;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void createSymbol (List<Atom> atoms, Vector3 targetPosition)
	{

		this.atoms = atoms;
		this.targetPosition = targetPosition;

		setActive (false);

		GameObject prefab = getPrefab ();	

		if (prefab != null) {
			Debug.Log ("Electrons in list: " + atoms.Count);
			element = Instantiate (prefab, targetPosition, new Quaternion ());
			element.GetComponent<Element> ().label.text = prefab.name;
		}

	}

	public void splitElement ()
	{

		element.SetActive (false);
		setActive (true);

		foreach (Atom atom in atoms) {
			atom.setMoveAway (true);
			atom.setRing (true);
		}
	}

	void setActive (bool active)
	{
		foreach (Atom atom in atoms) {
			electrons += atom.numberElectrons;
			atom.setRenderer (active);
			atom.label.enabled = active;
		}
	}

	GameObject getPrefab ()
	{
		if(containsExactly(new string[] {"H", "H"})){
			return H2prefab;
		}
		else if(containsExactly(new string[] {"O", "H", "H"})){
			return H2Oprefab;
		}
		else if(containsExactly(new string[] {"Na", "Cl"})){
			return NaClprefab;
		}
			
		return null;	
	}

	private bool containsExactly(string[] atomLetters){

		if (atomLetters.Length != atoms.Count) {
			return false;
		}

		List<Atom> copyAtoms = new List <Atom>(atoms);

		Debug.Log ("Number of atoms: " + copyAtoms.Count);
		foreach (Atom atom in copyAtoms) {
			Debug.Log ("List includes: " + atom.label.text);
		}

		int found = 0;

		foreach (string letter in atomLetters) {
			foreach(Atom atom in atoms){
				if (letter.Equals (atom.label.text)) {
					Debug.Log ("Letter found for " + letter + ": " + atom.label.text);
					copyAtoms.Remove (atom);
					found++;
					Debug.Log ("Number of atoms: " + copyAtoms.Count);
					break;
				} else {
					Debug.Log (letter + " could not be matched to " + atom.label.text);
				}
			}
		}

		if (atoms.Count!=found) {
			Debug.Log ("Number of items in list left: " + copyAtoms.Count);
			return false;
		}

		return true;
		
	}

}


