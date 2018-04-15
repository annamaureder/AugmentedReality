﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Atom : MonoBehaviour
{

	//properties of this atom
	public Text label;
	public string name = "Atom";
	public int numberElectrons = 0;
	public Vector3 lastPosition;
	public Vector3 mergeTarget;
	public Vector3 threshold;
	public bool isDragged = false;
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

	//other atoms
	private List<Atom> mergeAtoms;

	private ElementCreator elementCreator;
	float speed = 0.5f;
	float step;

	// Use this for initialization
	void Start ()
	{
		renderChilds = this.transform.Find ("Ring/Electrons").GetComponentsInChildren<Renderer> ();
		ring = this.transform.Find ("Ring");
		core = this.transform.Find ("Core");
		label.text = name;
		drawElectrons (numberElectrons);
		mergeAtoms = new List<Atom> ();
		elementCreator = gameObject.GetComponent<ElementCreator> ();
		lastPosition = transform.position;
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
			merge ();
		}

		if (split) {
			moveAway ();
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			elementCreator.splitElement ();
		}
		
	}

	public void setVariables (string name, int numberElectrons)
	{
		this.name = name;
		this.numberElectrons = numberElectrons;
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
				performMerge = true;
				setOtherColliders (colliderStatus = false);
				setColliderStatus (false);
				atom.lastPosition = new Vector3(transform.position.x + threshold.x, transform.position.y, transform.position.z + threshold.z);
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
		int electrons = numberElectrons;

		foreach (Atom atom in mergeAtoms) {
			electrons += atom.numberElectrons;
		}
			
		return electrons == 8 || electrons == 2;
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

		moveTowards (mergeAtoms);
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

		transform.position = Vector3.MoveTowards (transform.position, lastPosition, step);

		if (pointReached(transform.position, lastPosition)) {
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

	public void setMoveAway (bool active)
	{
		split = active;
	}

	private bool pointReached (Vector3 v1, Vector3 v2)
	{
		return Vector3.Distance (v1, v2) < 0.03f;
	}

	public void setRing(bool active){
		ring.gameObject.SetActive (active);
		drawElectrons (numberElectrons);
	}

	public void setIsDragged(bool active){
		isDragged = active;
	}

	public bool getIsDragged(){
		return isDragged;
	}

}
