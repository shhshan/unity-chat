using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

public class AzureTTS : MonoBehaviour
{
    [Header("Azure Speech Settings")]
    public string subscriptionKey = "Your Azure API Key";
    public string region = "Your Azure Region";  // e.g., "eastus"

    private SpeechConfig speechConfig;
    private AudioSource audioSource;

    void Start()
    {
        speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
        audioSource = GetComponent<AudioSource>();
    }

    public async void SpeakText(string text)
    {
        using (var synthesizer = new SpeechSynthesizer(speechConfig))
        {
            var result = await synthesizer.SpeakTextAsync(text);
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Debug.Log("✅ Speech synthesis successful: " + text);
            }
            else
            {
                Debug.LogError("❌ Speech synthesis failed: " + result.Reason);
            }
        }
    }
}
