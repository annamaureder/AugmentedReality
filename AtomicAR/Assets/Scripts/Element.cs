using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {

	public Text label;
	public List<Atom> atoms;
	AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void splitElement ()
	{
		foreach (Atom atom in atoms) {
			atom.setVisible (true);
			atom.setMoveAway (true, this.gameObject.transform.position);
		}
			
		Destroy (this.gameObject);
	}
		
	public string toString(){
		return name;
	}
}
