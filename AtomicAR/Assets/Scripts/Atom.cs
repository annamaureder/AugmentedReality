using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Atom : MonoBehaviour
{

	//properties of this atom
	public Text label;
	public string name = "Atom";
	public int numberElectrons = 0;
	private int outherElectrons = 0;
	public Color color;
	public Vector3 mergeVector;
	public Vector3 mergeTarget;
	public Vector3 threshold;
	public bool isDragged = false;
	public float t = 1.05f;
	public Vector3 oldPosition = new Vector3(0.0f, 0.0f, 0.0f);

	//state variables
	public bool isMerged;
	private bool performMerge = false;
	private bool colliderStatus;
	private bool split;

	//components of this atom
	private Renderer[] renderChilds;
	private Transform ring;
	private Transform core;
	Vector3 moveDirection; 

	//other atoms
	private List<Atom> mergeAtoms;

	private ElementCreator elementCreator;
	float speed = 0.5f;
	float step;

	// Use this for initialization
	void Start ()
	{

		if (numberElectrons <= 2) {
			outherElectrons = numberElectrons;
		} else {
			outherElectrons = (numberElectrons -2) % 8;
			Debug.Log (outherElectrons);
		}

		renderChilds = this.transform.Find ("Ring/Electrons").GetComponentsInChildren<Renderer> ();
		ring = this.transform.Find ("Ring");
		core = this.transform.Find ("Core");
		core.GetComponent<Renderer> ().material.color = color;
		label.text = name;
		drawElectrons (outherElectrons);
		mergeAtoms = new List<Atom> ();
		elementCreator = gameObject.GetComponent<ElementCreator> ();
		mergeVector = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		isDragged = false;

		if (oldPosition != transform.position) {
			isDragged = true;
		}
			
		threshold = (oldPosition - transform.position) * 5;
		oldPosition = transform.position;

		if (performMerge) {
			moveTowards (mergeAtoms);
		}

		if (split) {
			moveAway ();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			if (elementCreator.getElement() != null) {
				elementCreator.getElement ().GetComponent<Element> ().splitElement ();
			}
		}
	}

	public void setVariables (string name, int numberElectrons, Color color)
	{
		this.name = name;
		this.numberElectrons = numberElectrons;
		this.color = color;
	}


	void OnTriggerEnter (Collider collider)
	{

		if (collider.transform.tag == "Atom" && isDragged) {
			Atom atom = collider.gameObject.GetComponent<Atom> ();
			if (!mergeAtoms.Contains (atom)) {
				mergeAtoms.Add (atom);
				Debug.Log ("Atom added to list: " + mergeAtoms.Count);
			}
			if (electronsMatch () && !isMerged) {
				merge ();
				performMerge = true;
				setOtherColliders (colliderStatus = false);
				setColliderStatus (false);
				atom.mergeVector = mergeTarget - atom.transform.position;
				mergeVector = mergeTarget - transform.position;
			}
		}
	}

	void OnTriggerExit (Collider collision)
	{
		if (collision.transform.tag == "Atom" && isDragged) {
			Atom atom = (Atom)collision.gameObject.GetComponent<Atom> ();
			if (mergeAtoms.Contains (atom)) {
				mergeAtoms.Remove (atom);
				Debug.Log ("Removed from list: " + mergeAtoms.Count);
			}
		}
	}

	void OnCollisionEnter (Collision collider)
	{
		if (collider.transform.tag == "Atom") {
		}
	}

	bool electronsMatch ()
	{
		int electrons = outherElectrons;
		int total = numberElectrons;

		foreach (Atom atom in mergeAtoms) {
			electrons += atom.outherElectrons;
			total += atom.numberElectrons;
		}

		if (electrons == 2 && total == 2) {
			return true;
		}
			
		return electrons == 8;
	}

	void merge ()
	{
		mergeTarget = transform.position;
		int numberAtoms = 1;
		setRing (false);
		isMerged = true;

		foreach (Atom atom in mergeAtoms) {
			mergeTarget += atom.transform.position;
			numberAtoms++;
			atom.isMerged = true;
			atom.setRing (false);
		}

		mergeTarget /= numberAtoms;
	}

	void drawElectrons (int numberElectrons)
	{

		foreach (Renderer r in renderChilds) {
			r.enabled = false;
		}

		for (int i = 1; i < numberElectrons + 1; i++) {
			renderChilds [i].enabled = true;
		}
	}

	public void moveTowards (List<Atom> otherAtoms)
	{
		step = speed * Time.deltaTime;

		foreach (Atom atom in otherAtoms) {
			atom.transform.position = Vector3.MoveTowards (atom.transform.position, mergeTarget, step);
		}

		transform.position = Vector3.MoveTowards (transform.position, mergeTarget, step);

		if (pointReached (transform.position, mergeTarget)) {
			Debug.Log ("Point reached!");
			performMerge = false;
			mergeAtoms.Add (this);

			if (isDragged) {
				elementCreator.createSymbol (mergeAtoms, mergeTarget);
			}
		}
	}

	public void moveAway ()
	{
		step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, moveDirection, step);

		if (pointReached(transform.position, moveDirection)) {
			split = false;
			setColliderStatus (true);
			isMerged = false;
			setRing (true);
			mergeAtoms = new List<Atom> ();
		}
	}


	void setOtherColliders (bool active)
	{			
		foreach (Atom atom in mergeAtoms) {
			foreach (Collider c in atom.GetComponents<Collider> ()) {
				c.enabled = active;
			}
			foreach (Collider c in atom.GetComponentsInChildren<Collider> ()) {
				c.enabled = active;
			}
		}
	}

	void setColliderStatus (bool active)
	{

		foreach (Collider c in GetComponents<Collider> ()) {
			c.enabled = active;
		}

		foreach(Collider c in GetComponentsInChildren<Collider>()){
			c.enabled = active;
		}
	}



	public void setRenderer (bool active)
	{

		foreach (Renderer r in GetComponents<Renderer> ()) {
			r.enabled = active;
		}
		
		foreach (Renderer r in GetComponentsInChildren<Renderer> ()) {
			r.enabled = active;
		}
	}

	public void setMoveAway (bool active, Vector3 elementPosition)
	{
		split = active;
		transform.position = elementPosition;
		moveDirection = elementPosition - mergeVector;
		Debug.Log("Merge vector: " + mergeVector);
		moveDirection.Scale (new Vector3 (t, 1, t));
	}

	private bool pointReached (Vector3 v1, Vector3 v2)
	{
		return Vector3.Distance (v1, v2) < 0.03f;
	}

	public void setRing(bool active){
		ring.gameObject.SetActive (active);
		drawElectrons (outherElectrons);
	}

	public void setVisible (bool active)
	{
		setRenderer (active);
		label.enabled = active;
	}

	public void setIsDragged(bool active){
		isDragged = active;
	}

	public bool getIsDragged(){
		return isDragged;
	}

}
