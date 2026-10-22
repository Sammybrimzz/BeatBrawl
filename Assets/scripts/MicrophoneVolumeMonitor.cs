using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MicrophoneVolumeMonitor : MonoBehaviour
{
    // ====================
    // UI
    // ====================

    [Header("UI")]
    public TMP_Text volumeText;
    public Image volumeBar;

    // ====================
    // Microphone
    // ====================

    public AudioClip microphoneClip;
    public string microphoneDevice;

    private const int SampleRate = 44100;
    private const int SampleWindow = 128;

    private readonly float[] volumeSamples =
        new float[SampleWindow];

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        microphoneDevice = Microphone.devices[0];

        microphoneClip = Microphone.Start(
            microphoneDevice,
            true,
            10,
            SampleRate
        );

        Debug.Log($"Using microphone: {microphoneDevice}");
    }

    void Update()
    {
        float volume = GetMicrophoneVolume();

        if (volumeText != null)
        {
            volumeText.text =
                $"Volume: {volume:F3}";
        }

        if (volumeBar != null)
        {
            volumeBar.fillAmount =
                Mathf.Clamp01(volume * 20f);
        }
    }

    float GetMicrophoneVolume()
    {
        if (microphoneClip == null)
            return 0f;

        int micPosition =
            Microphone.GetPosition(microphoneDevice);

        if (micPosition < SampleWindow)
            return 0f;

        microphoneClip.GetData(
            volumeSamples,
            micPosition - SampleWindow
        );

        float sum = 0f;

        for (int i = 0; i < volumeSamples.Length; i++)
        {
            sum += Mathf.Abs(volumeSamples[i]);
        }

        return sum / volumeSamples.Length;
    }
}