using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512spheres : MonoBehaviour {
	//smooth movement
	//public float smoothTime = 0.1F;
	//private Vector3 velocity = Vector3.zero;

	//holds 8 differnent materials with different colors
	public Material[] materials;

	// Prefab sphere that gets spawned in.
	public GameObject spherePrefab;

	// Array that holds the 512 spheres we're spawning in.
	GameObject[] spheres = new GameObject[512];

	//Array that holds initial positions of all the spheres
	Vector3[] initCoord = new Vector3[512];

	public float[] angles = new float[512];

	//set max size of spheres
	public float maxScale;

	// Scales the height of each cube by this much.
	public float scale;
	public float shift = 1f;
	/* Spawns 512 instances of spherePrefab in a "circle" of radius 100
     * around the object this script is attached to.*/
	void Start () {

		float radius = 100; //radius of "circle"

		for (int i = 0; i < 512; i++) {
			// Spawns a copy of spherePrefab
			GameObject sphere = Instantiate(spherePrefab);

			// Assigns this copy to it's proper position in cubes.
			spheres[i] = sphere;

			// Names it properly.
			sphere.name = "Sphere" + i;

			// Set its parent to this object.
			sphere.transform.parent = this.transform;

			/* Rotate and position the cube properly. Some attributes that
             * might come in handy include Transform.eulerAngles, Transform.forward,
             * and the Transform class in general. Make sure you're using floats
             */
			float angle = (360f/512f) * i;

			angles [i] = angle;

			//Apparently you get cool patterns when you forget to convert degrees to radians
			float xPos = radius * Mathf.Cos(angle);

			float zPos = radius * Mathf.Sin(angle);

			transform.eulerAngles = new Vector3(angle, 0, 0);

			shift = shift * -1;

			sphere.transform.position = new Vector3(shift * xPos, 0,shift * zPos);

			//Set variables for sphere translation (code below)
			Transform s = transform;

			initCoord [i] = s.TransformPoint(new Vector3(shift * xPos, 0 ,shift * zPos));

		}
			
	}
		

	void Update () {
		for (int i = 0; i < 512; i++) {
			if (spheres != null) {
				GameObject sphere = spheres [i];
				float newScale;
				float band;

				Renderer rend = sphere.GetComponent<Renderer>();
				rend.enabled = true;

				int bandNum;

				if (i <= 64) {
					bandNum = 0;
				} else if (i <= 128) {
					bandNum = 1;
				} else if (i <= 192) {
					bandNum = 2;
				} else if (i <= 256) {
					bandNum = 3;
				} else if (i <= 320) {
					bandNum = 4;
				} else if (i <= 384) {
					bandNum = 5;
				} else if (i <= 448) {
					bandNum = 6;
				} else {
					bandNum = 7;
				}

				band = SpectrumAnalyzer.audioBandBuffers [bandNum];
				rend.sharedMaterial = materials[bandNum];

				Transform s = sphere.transform;

				//scale the sphere
				newScale = 1 + scale * (Mathf.Abs(band));


				//make sure spheres don't get too big
				if (newScale > maxScale){
					newScale = maxScale;
				}

				//update scale of sphere
				s.localScale = new Vector3 (newScale, newScale, newScale);

				//move the sphere 
				Vector3 movementVector = Vector3.Normalize (s.position - transform.position);
				s.position = initCoord[i] + movementVector * newScale/3f; 

			}
		}
	}
}
