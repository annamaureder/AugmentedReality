using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Atom : MonoBehaviour
{

	//properties of this atom
	public Text label;
	public Text numberLabel;
	public string name = "Atom";
	public int totalElectrons = 0;
	private int boundingElectrons = 0;
	public Color color;
	public Vector3 mergeVector;
	public Vector3 mergeTarget;
	public bool isDragged = false;
	public float t = 1.6f;
	public Vector3 oldPosition = new Vector3 (0.0f, 0.0f, 0.0f);

	//state variables
	public bool isMerged;
	private bool performMerge = false;
	private bool colliderStatus;
	private bool split;

	//components of this atom
	private List<Renderer> rings = new List<Renderer>();
	private Renderer core;
	Vector3 moveDirection;

	//other atoms
	private List<Atom> mergeAtoms;

	private ElementCreator elementCreator;
	float speed = 0.5f;
	float step;

	// Use this for initialization
	void Start ()
	{

		if (totalElectrons <= 2) {
			boundingElectrons = totalElectrons;
		} else {
			boundingElectrons = (totalElectrons - 2) % 8;
			Debug.Log (boundingElectrons);
		}
			

		foreach (var child in GetComponentsInChildren<Renderer>()) {
			if (child.gameObject.name == "Ring") {
				rings.Add (child);
			} else if (child.gameObject.name == "Core") {
				core = child;
				core.GetComponent<Renderer> ().material.color = color;
			}
		}

		label.text = name;
		numberLabel.text = totalElectrons.ToString();
		drawRings ();
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
			
		oldPosition = transform.position;

		if (performMerge) {
			moveTowards (mergeAtoms);
		}

		if (split) {
			moveAway ();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			if (elementCreator.getElement () != null) {
				elementCreator.getElement ().GetComponent<Element> ().splitElement ();
			}
		}
	}

	public void setVariables (string name, int numberElectrons, Color color)
	{
		this.name = name;
		this.totalElectrons = numberElectrons;
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

		int electrons = boundingElectrons;
		int total = totalElectrons;

		if (mergeAtoms.Count == 1 && mergeAtoms [0].totalElectrons == total && electrons >= 6) {
			return true;
		}

		foreach (Atom atom in mergeAtoms) {
			electrons += atom.boundingElectrons;
			total += atom.totalElectrons;
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
		setRings (false);
		isMerged = true;

		foreach (Atom atom in mergeAtoms) {
			mergeTarget += atom.transform.position;
			numberAtoms++;
			atom.isMerged = true;
			atom.setRings (false);
		}

		mergeTarget /= numberAtoms;
	}

	void drawRings ()
	{

		int lastRing = (boundingElectrons + 1) / 2;

		Debug.Log ("Childs: " + rings.Count);
		Debug.Log ("Number rings: " + lastRing);

		for (int i = 0; i < lastRing; i++) {
			rings [i].enabled = true;
			foreach (var electron in rings[i].GetComponentsInChildren<Renderer>()) {
				electron.enabled = true;
			}
		}

		if (boundingElectrons % 2 != 0) {
			rings [lastRing-1].GetComponentsInChildren<Renderer> () [1].enabled = false;
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

		if (pointReached (transform.position, moveDirection)) {
			split = false;
			setColliderStatus (true);
			isMerged = false;
			setRings (true);
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

		foreach (Collider c in GetComponentsInChildren<Collider>()) {
			c.enabled = active;
		}
	}
		
	public void setMoveAway (bool active, Vector3 elementPosition)
	{
		split = active;
		transform.position = elementPosition;
		mergeVector.Scale (new Vector3 (t, t, t));
		moveDirection = elementPosition - mergeVector;
		moveDirection.y = 0.15f;
		Debug.Log ("Merge vector: " + mergeVector);
		Debug.Log ("elementPosition: " + elementPosition);
		Debug.Log ("moveDirection: " + moveDirection);
	}

	private bool pointReached (Vector3 v1, Vector3 v2)
	{
		return Vector3.Distance (v1, v2) < 0.03f;
	}

	private void setRings (bool active)
	{
		if (active) {
			drawRings ();
			return;
		}

		foreach (var ring in rings) {
			foreach (var electron in ring.GetComponentsInChildren<Renderer>()) {
				electron.enabled = false;;
			}
			ring.enabled = false;
		}
	}

	private void setCore(bool active){
		core.enabled = active;
	}

	private void setLabel(bool active){
		label.enabled = active;
		numberLabel.enabled = active;
	}

	public void setVisible(bool active){
		setRings (active);
		setCore (active);
		setLabel (active);
	}
		
	public void setIsDragged (bool active)
	{
		isDragged = active;
	}

	public bool getIsDragged ()
	{
		return isDragged;
	}

	public List<Renderer> getRings(){
		return rings;
	}
}
