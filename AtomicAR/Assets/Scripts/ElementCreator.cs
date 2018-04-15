using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCreator : MonoBehaviour
{
	private GameObject element;
	public GameObject H2Oprefab;
	public GameObject H2prefab;
	public GameObject NaClprefab;
	public GameObject NaOHprefab;
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

		if(containsExactly(new List<string>{"O", "H", "H"})){
			return H2Oprefab;
		}
		else if(containsExactly(new List<string>{"H", "Cl"})){
			return HClprefab;
		}
		else if(containsExactly(new List<string>{"H", "Na", "O"})){
			return NaOHprefab;
		}
			else if(containsExactly(new List<string>{"Na", "Cl"})){
			return NaClprefab;
		}
			else if(containsExactly(new List<string>{"H", "H"})){
			return H2prefab;
		}
			
		return null;	
	}

	private bool containsExactly(List<string> atomLetters){

		if (atomLetters.Count != atoms.Count) {
			return false;
		}
			
		foreach (var atom in atoms) {
			for (int i = 0; i < atomLetters.Count; ++i) {
				if (atomLetters[i].Equals (atom.label.text)) {
					atomLetters.RemoveAt (i);
				}
			}
		}
		if (atomLetters.Count == 0) {
			return true;
		}

		return false;

	}

	public GameObject getElement(){
		return element;
	}

}


