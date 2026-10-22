using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PitchDetector : MonoBehaviour
{
    public TMP_Text frequencyText;
    public TMP_Text noteText;
    public TMP_Text statusText;
    public TMP_Text centsText;
    public RectTransform needle;
    public TMP_Text confidenceText;


    private MicrophoneVolumeMonitor micMonitor;

    private const int SampleRate = 44100;
    private const int SampleSize = 4096;
    private const float SignalThreshold = 0.001f;
    private float displayedCents = 0f;
    private float needleSmoothSpeed = 8f;
    private string displayedNote = "---";
    private string candidateNote = "---";
    private int candidateFrames = 0;
    private float smoothedFrequency = 0f;
    private const float FrequencySmoothSpeed = 10f;

    private const int NoteLockFrames = 3;

    private float[] samples = new float[SampleSize];

    private readonly string[] noteNames =
    {
        "C",
        "C#",
        "D",
        "D#",
        "E",
        "F",
        "F#",
        "G",
        "G#",
        "A",
        "A#",
        "B"
    };

    string GetLockedNote(string detectedNote)
    {
        if (detectedNote == displayedNote)
        {
            candidateNote = detectedNote;
            candidateFrames = 0;
            return displayedNote;
        }

        if (detectedNote == candidateNote)
        {
            candidateFrames++;

            if (candidateFrames >= NoteLockFrames)
            {
                displayedNote = detectedNote;
                candidateFrames = 0;
            }
        }
        else
        {
            candidateNote = detectedNote;
            candidateFrames = 1;
        }

        return displayedNote;
    }

    float GetConfidence(float signalLevel)
    {
        float confidence =
            Mathf.InverseLerp(
                SignalThreshold,
                0.01f,
                signalLevel
            );

        return Mathf.Clamp01(confidence);
    }

    float GetCentsOff(float frequency)
    {
        float targetFrequency =
            GetNearestNoteFrequency(frequency);

        return 1200f *
               Mathf.Log(
                   frequency / targetFrequency,
                   2f
               );
    }

    string GetTuningStatusFromCents(float cents)
    {
        if (Mathf.Abs(cents) <= 5f)
            return "In Tune";

        if (cents > 0f)
            return "Sharp";

        return "Flat";
    }

    void UpdateNeedle(float cents)
    {
        if (needle == null)
            return;

        float clampedCents =
            Mathf.Clamp(cents, -50f, 50f);

        float xPosition =
            (clampedCents / 50f) * 200f;

        needle.anchoredPosition =
            new Vector2(
                xPosition,
                needle.anchoredPosition.y
            );
    }

    void Start()
    {
        micMonitor = GetComponent<MicrophoneVolumeMonitor>();

        if (frequencyText != null)
            frequencyText.text = "Frequency: ---";

        if (noteText != null)
            noteText.text = "Note: ---";

        if (statusText != null)
            statusText.text = "Status: ---";
    }

    void Update()
    {
        if (micMonitor == null)
            return;

        if (micMonitor.microphoneClip == null)
            return;

        int micPosition =
            Microphone.GetPosition(
                micMonitor.microphoneDevice
            );

        if (micPosition < SampleSize)
            return;

        micMonitor.microphoneClip.GetData(
            samples,
            micPosition - SampleSize
        );

        float signalLevel = GetSignalLevel();
        float confidence =
    GetConfidence(signalLevel);

        if (signalLevel < SignalThreshold)
        {
            ShowNoPitch();
            return;
        }

        float frequency = DetectPitch();

        if (frequency <= 0f)
        {
            ShowNoPitch();
            return;
        }

        if (smoothedFrequency <= 0f)
        {
            smoothedFrequency = frequency;
        }
        else
        {
            smoothedFrequency =
                Mathf.Lerp(
                    smoothedFrequency,
                    frequency,
                    Time.deltaTime * FrequencySmoothSpeed
                );
        }

        frequency = smoothedFrequency;

        float cents =
    GetCentsOff(frequency);

        frequencyText.text =
            $"Frequency: {frequency:F1} Hz";

        string detectedNote =
    GetNoteName(frequency);

        string lockedNote =
            GetLockedNote(detectedNote);

        noteText.text =
            $"Note: {lockedNote}";

        statusText.text =
            $"Status: {GetTuningStatusFromCents(cents)}";

        centsText.text =
            $"Cents: {cents:F0}";

        confidenceText.text =
    $"Confidence: {(confidence * 100f):F0}%";

        displayedCents =
    Mathf.Lerp(
        displayedCents,
        cents,
        Time.deltaTime * needleSmoothSpeed
    );

        UpdateNeedle(displayedCents);
    }

    void ShowNoPitch()
    {
        if (frequencyText != null)
            frequencyText.text = "Frequency: ---";

        if (noteText != null)
            noteText.text = "Note: ---";

        if (statusText != null)
            statusText.text = "Status: ---";

        if (confidenceText != null)
        {
            confidenceText.text =
                "Confidence: 0%";
        }

        displayedNote = "---";
        candidateNote = "---";
        candidateFrames = 0;
        smoothedFrequency = 0f;

        if (centsText != null)
        {
            centsText.text = "Cents: ---";
        }

        if (needle != null)
        {
            needle.anchoredPosition =
                new Vector2(
                    0f,
                    needle.anchoredPosition.y
                );
        }
    }

    float GetSignalLevel()
    {
        float sum = 0f;

        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        return sum / samples.Length;
    }

    float DetectPitch()
    {
        int minLag = SampleRate / 400;
        int maxLag = SampleRate / 70;

        float bestCorrelation = 0f;
        int bestLag = -1;

        for (int lag = minLag; lag <= maxLag; lag++)
        {
            float correlation = 0f;

            for (int i = 0; i < SampleSize - lag; i++)
            {
                correlation +=
                    samples[i] *
                    samples[i + lag];
            }

            if (correlation > bestCorrelation)
            {
                bestCorrelation = correlation;
                bestLag = lag;
            }
        }

        if (bestLag <= 0)
            return 0f;

        return (float)SampleRate / bestLag;
    }

    string GetNoteName(float frequency)
    {
        if (frequency <= 0f)
            return "---";

        float noteNumber =
            69f +
            12f *
            Mathf.Log(frequency / 440f, 2f);

        int roundedNote =
            Mathf.RoundToInt(noteNumber);

        int noteIndex =
            ((roundedNote % 12) + 12) % 12;

        int octave =
            (roundedNote / 12) - 1;

        return noteNames[noteIndex] + octave;
    }

    float GetNearestNoteFrequency(float frequency)
    {
        float noteNumber =
            69f +
            12f *
            Mathf.Log(frequency / 440f, 2f);

        int roundedNote =
            Mathf.RoundToInt(noteNumber);

        return 440f *
               Mathf.Pow(
                   2f,
                   (roundedNote - 69f) / 12f
               );
    }

    string GetTuningStatus(float frequency)
    {
        float targetFrequency =
            GetNearestNoteFrequency(frequency);

        float difference =
            frequency - targetFrequency;

        if (Mathf.Abs(difference) < 1f)
            return "In Tune";

        if (difference > 0f)
            return "Sharp";

        return "Flat";
    }
}