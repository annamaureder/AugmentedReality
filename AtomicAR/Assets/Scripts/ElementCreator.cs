using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCreator : MonoBehaviour
{
	private GameObject element;
	public GameObject H2Oprefab;
	public GameObject H2prefab;
	public GameObject NaClprefab;
	public GameObject HClprefab;

	Vector3 targetPosition;
	List<Atom> atoms;

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
		this.targetPosition = targetPosition;
		this.atoms = atoms;

		foreach (Atom atom in atoms) {
			atom.setVisible (false);
		}
			
		GameObject prefab = getPrefab ();	

		if (prefab != null) {
			element = Instantiate (prefab, targetPosition, new Quaternion ());

			Element e = element.GetComponent<Element> ();
			e.label.text = prefab.name;
			e.atoms = atoms;
		}

	}

	GameObject getPrefab ()
	{

		if(containsExactly(new string[] {"O", "H", "H"})){
			return H2Oprefab;
		}
		else if(containsExactly(new string[] {"H", "Cl"})){
			return HClprefab;
		}
		else if(containsExactly(new string[] {"Na", "Cl"})){
			return NaClprefab;
		}
		else if(containsExactly(new string[] {"H", "H"})){
			return H2prefab;
		}
			
		return null;	
	}

	private bool containsExactly(string[] atomLetters){

		if (atomLetters.Length != atoms.Count) {
			return false;
		}

		List<Atom> copyAtoms = new List <Atom>(atoms);

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

	public GameObject getElement(){
		return element;
	}

}


