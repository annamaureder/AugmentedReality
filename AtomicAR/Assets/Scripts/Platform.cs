using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

	private static bool blocked;
	public Material green;
	public Material red;

	// Use this for initialization
	void Start ()
	{

		blocked = false;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider collider)
	{

		if (collider.transform.tag == "Atom") {
			Debug.Log ("Platform blocked!");
			blocked = true;
			this.GetComponent<Renderer> ().material = red;
		}
	}

	void OnTriggerExit (Collider collision)
	{
		if (collision.transform.tag == "Atom") {
			Debug.Log ("Platform free!");
			blocked = false;
			this.GetComponent<Renderer> ().material = green;
		}
	}

	public static bool isBlocked ()
	{
		return blocked;
	}
}
