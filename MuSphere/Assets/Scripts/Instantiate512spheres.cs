using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512spheres : MonoBehaviour {

	/** Instantiates 512 spheres that dynamically respond to
	 * any given audio input. The spheres are broken down
	 * into eight groups, each representing a range of frequencies.
	 */

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

	// Scales the height of each sphere by this much.
	public float scale;
	public float shift = 1f;

	/* Spawns 512 instances of spherePrefab in a sphere
     * around the object this script is attached to.*/
	void Start () {

		float radius = 100; //radius of sphere

		for (int i = 0; i < 512; i++) {

			// Spawns a copy of spherePrefab
			GameObject sphere = Instantiate(spherePrefab);

			// Assigns this copy to it's proper position in cubes.
			spheres[i] = sphere;

			// Names it properly with its index.
			sphere.name = "Sphere" + i;

			// Set its parent to this object.
			sphere.transform.parent = this.transform;


			/* Cool stuff to make an interesting spherical pattern of spheres.
			 * Apparently you get cool patterns when you forget to convert degrees to radians.*/
			float angle = (360f/512f) * i;

			angles [i] = angle;

			float xPos = radius * Mathf.Cos(angle);

			float zPos = radius * Mathf.Sin(angle);

			transform.eulerAngles = new Vector3(angle, 0, 0);

			shift = shift * -1;

			sphere.transform.position = new Vector3(shift * xPos, 0,shift * zPos);

			Transform s = transform;

			//Set initial coordinate as a position anchor in case sphere moves
			initCoord [i] = s.TransformPoint(new Vector3(shift * xPos, 0 ,shift * zPos));

		}

	}

	/* Updates positions and scales of spheres based
	 * on the audio input.*/
	void Update () {
		for (int i = 0; i < 512; i++) {
			if (spheres != null) {
				GameObject sphere = spheres [i];
				float newScale;
				float band;

				Renderer rend = sphere.GetComponent<Renderer>();
				rend.enabled = true;

				int bandNum;

				//Determine which band sphere fits into based on its index.

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

				//Set band to scale spheres to their respective band sizes
				band = SpectrumAnalyzer.audioBandBuffers[bandNum];

				scale = Mathf.Min(maxScale, SpectrumAnalyzer.getAvgMaxFrequency() / 1.5f);
				//Set color
				rend.sharedMaterial = materials[bandNum];

				Transform s = sphere.transform;

				//scale the sphere
				if (i > 448) {
					newScale = 1.3f + (scale) * (Mathf.Abs(band));
				} else {
					newScale = 1 + scale * (Mathf.Abs(band));
				}


				//make sure spheres don't get too big
				if (newScale > maxScale){
					newScale = maxScale;
				}

				//update scale of sphere
				s.localScale = new Vector3 (newScale, newScale, newScale);

				//move the sphere based on the new scale
				Vector3 movementVector = Vector3.Normalize (s.position - transform.position);
				s.position = initCoord[i] + movementVector * newScale/2f; 

			}
		}
	}
}

