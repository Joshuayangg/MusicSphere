using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip audioClip;
    public string selectedDevice;
    public bool useMicrophone;

    public AudioMixerGroup mixerGroupMicrophone, mixerGroupMaster;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (useMicrophone){
            if (Microphone.devices.Length > 0) {
                selectedDevice = Microphone.devices[0].ToString();
                audioSource.outputAudioMixerGroup = mixerGroupMicrophone;
                audioSource.clip = Microphone.Start(selectedDevice, true, 10, AudioSettings.outputSampleRate);
                while (!(Microphone.GetPosition(selectedDevice) > 0)){}
            } else {
                useMicrophone = false;
            }
        } 
        if (!useMicrophone) {
            audioSource.outputAudioMixerGroup = mixerGroupMaster;
            audioSource.clip = audioClip;
        }

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMic(int micNum) {
        audioSource.Stop();
        Microphone.End(selectedDevice);

        if (useMicrophone){
            if (Microphone.devices.Length > 0) {
                selectedDevice = Microphone.devices[micNum].ToString();
                audioSource.outputAudioMixerGroup = mixerGroupMicrophone;
                audioSource.clip = Microphone.Start(selectedDevice, true, 10, AudioSettings.outputSampleRate);
                while (!(Microphone.GetPosition(selectedDevice) > 0)){}
            } else {
                useMicrophone = false;
            }
        } 
        if (!useMicrophone) {
            audioSource.outputAudioMixerGroup = mixerGroupMaster;
            audioSource.clip = audioClip;
        }

        audioSource.Play();

        Debug.Log("Changed Mic to: " + selectedDevice);
    }

    public string[] getMics() {
        int numMics = Microphone.devices.Length;
        string[] mics = new string[numMics];
        
        for (int i = 0; i < numMics; i++) {
            mics[i] = Microphone.devices[i].ToString();
        }

        return mics;
    }
}
