using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	public GameObject palette;
	private static bool blocked;
	public Material green;
	public Material red;

	Renderer[] childs;
	Color[] colors;

	// Use this for initialization
	void Start ()
	{
		blocked = false;
		childs = palette.GetComponentsInChildren<Renderer> ();
		colors = new Color[childs.Length];

		for (int i = 0; i < childs.Length; i++) {
			colors [i] = childs [i].material.color;
		}
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

			foreach (Renderer renderer in childs) {
				renderer.material.color = Color.gray;
			}
		}
	}

	void OnTriggerExit (Collider collision)
	{
		if (collision.transform.tag == "Atom") {
			Debug.Log ("Platform free!");
			blocked = false;
			this.GetComponent<Renderer> ().material = green;

			for (int i = 0; i < childs.Length; i++) {
				childs[i].material.color = colors[i];
			}
		}
	}

	public static bool isBlocked ()
	{
		return blocked;
	}
}
