using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour {
	AudioSource audioSource;
	public static float[] samples = new float[512];

	public static float[] freqBand = new float[8];
	public static float[] bufferBand = new float[8];

	static float[] freqBandHighest = new float[8];

	public static float[] audioBands = new float[8];
	public static float[] audioBandBuffers = new float[8];
	public float[] bufferDecrease = new float[8];

	void CreateAudioBands() {
		for (int k = 0; k < 8; k++) {
			if (freqBand[k] > freqBandHighest[k]) {
				freqBandHighest[k] = freqBand[k];
			}
			audioBands [k] = freqBand [k] / freqBandHighest[k];
			audioBandBuffers [k] = bufferBand [k] / freqBandHighest[k];

		}
	}

	void BufferBands(){
		for (int k = 0; k < 8; k++) {
			if (freqBand[k] > bufferBand[k]){
				bufferBand [k] = freqBand [k];
				bufferDecrease [k] = 0.005f;
			}
			if (freqBand[k] < bufferBand[k] && (bufferBand[k] - bufferDecrease [k]) > 0){
				bufferBand [k] -= bufferDecrease [k];
				bufferDecrease [k] *= 1.4f;
			}
		}
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}

	void GetSpectrumAudioSource() {
		audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
	}

	void MakeFrequencyBands() {
		int count = 0;

		// Iterate through the 8 bins.
		for (int i = 0; i < 8; i++)  {
			float average = 0;
			int sampleCount = (int)Mathf.Pow (2, i + 1);

			// Adding the remaining two samples into the last bin.
			if (i == 7) {
				sampleCount += 2;
			}

			// Go through the number of samples for each bin, add the data to the average
			for (int j = 0; j < sampleCount; j++) {
				average += samples [count];
				count++;
			}

			// Divide to create the average, and scale it appropriately.
			average /= count;
			freqBand[i] = (i+1) * 100 * average;
		}
	}

	public static float getAvgMaxFrequency() {
		float avg = 0;

		for (int k = 0; k < 8; k++) {
			avg += freqBandHighest[k];
		}

		avg /= 8f;
		return avg;
	}

	public static float getAvgFrequency() {
		float avg = 0;

		for (int k = 0; k < 8; k++) {
			avg += freqBand[k];
		}

		avg /= 8f;
		return avg;
	}

	// Update is called once per frame
	void Update () {
		GetSpectrumAudioSource();
		MakeFrequencyBands();
		CreateAudioBands();
		BufferBands ();
	}
}
