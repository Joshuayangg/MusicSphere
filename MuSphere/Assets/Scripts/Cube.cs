using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public int band;
	public float startScale;
	public float scaleMultiplier;
	public float maxScale;
	public bool buffered;



	void Start () {
	}


	void Update () {
		for (int i = 0; i < 8; i++) {
			if (i == band) {
				float scale = startScale + scaleMultiplier * (Mathf.Abs(SpectrumAnalyzer.audioBandBuffers [i]));
				if (scale > maxScale) {
					scale = maxScale;
				}
				transform.localScale = new Vector3 (10, scale, 10);
				
			}
		}
	}
}
