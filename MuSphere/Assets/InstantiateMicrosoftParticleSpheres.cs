using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateMicrosoftParticleSpheres : MonoBehaviour {

	/** Instantiates ~512 spheres in the shape of the microsoft logo
     * that dynamically respond to
	 * any given audio input. The spheres are broken down
	 * into four groups, each representing a range of frequencies.
	 */

	//holds 4 differnent materials with different colors
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

	// Set incremental x and y shifts for the spheres
	float xShift = 10f;
	float yShift = 10f;

	// Set starting coordinates for each of the four squares
	float xStart_1 = -145f;
	float yStart_1 = 125f;

	// Set limits for each row of spheres
	float x_lim = 1000f;

	// Set Current x coordinates
	float x_curr;
	float y_curr;

	/* Spawns 512 instances of spherePrefab in a sphere
     * around the object this script is attached to.*/
	void Start () {

		float radius = 100; //radius of sphere

		x_curr = xStart_1;
		y_curr = yStart_1;

		for (int i = 0; i < 512; i++) {

			// Spawns a copy of spherePrefab
			GameObject sphere = Instantiate(spherePrefab);

			// Assigns this copy to it's proper position in cubes.
			spheres[i] = sphere;

			// Names it properly with its index.
			sphere.name = "Sphere" + i;

			// Set its parent to this object.
			sphere.transform.parent = this.transform;

			transform.eulerAngles = new Vector3(0, 0, 0);
			sphere.transform.position = new Vector3(x_curr, y_curr, 0);

			x_curr += 15f;

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

				scale = Mathf.Min(maxScale, SpectrumAnalyzer.getAvgMaxFrequency());
				//Set color
				sphere.GetComponent<ParticleSystemRenderer>().material = materials[bandNum/2];

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

				//update scale of particle sphere
				ParticleSystem ps = sphere.GetComponent<ParticleSystem>();
                var sh = ps.shape;
                var mn = ps.main;

                mn.simulationSpeed = newScale * 2f;
				mn.startSpeed = newScale * 0.1f;
                sh.radius = newScale;

			}
		}
	}
}


