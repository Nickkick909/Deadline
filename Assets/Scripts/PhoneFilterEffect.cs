using UnityEngine;

/// <summary>
/// Simple runtime Bitcrusher effect – simulates low sample rate and bit depth.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PhoneFilterEffect : MonoBehaviour
{
    [Range(1000, 44100)] public int sampleRate = 8000; // Typical phone = 8000 Hz
    [Range(1, 16)] public int bitDepth = 8; // 8-bit telephone/retro feel
    private float step;
    private float lastSample;

    public bool enableFilter = false;
    private float outputSampleRate;

    void Awake()
    {
        // Cache main-thread-only values here
        outputSampleRate = AudioSettings.outputSampleRate;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!enableFilter)
            return;


        step = Mathf.Pow(0.5f, bitDepth);
        float rateReducer = (float)sampleRate / outputSampleRate;
        float counter = 0;

        for (int i = 0; i < data.Length; i += channels)
        {
            counter += rateReducer;
            if (counter >= 1.0f)
            {
                counter -= 1.0f;
                lastSample = Mathf.Round(data[i] / step) * step; // Quantize amplitude
            }

            for (int c = 0; c < channels; c++)
                data[i + c] = lastSample;
        }
    }
}
 