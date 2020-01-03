using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;
 
    void Start()
    {
        this.vcam = GetComponent<CinemachineVirtualCamera>();
        this.noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
    }

    public void Begin(float amplitude, float gain, float length)
    {
        this.StartCoroutine(ProcessShake(amplitude, gain, length));
    }

    private IEnumerator ProcessShake(float amplitude, float gain, float length)
    {
        Noise(amplitude, gain);
        yield return new WaitForSeconds(length);
        Noise(0, 0);
    }
 
    private void Noise(float amplitude, float gain)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = gain;        
 
    }
}
