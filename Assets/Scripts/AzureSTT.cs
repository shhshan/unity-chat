using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

public class AzureSTT : MonoBehaviour
{
    [Header("Azure Speech Settings")]
    public string subscriptionKey = "Your Azure API Key";
    public string region = "Your Azure Region";  // e.g., "eastus"

    private SpeechConfig speechConfig;

    void Start()
    {
        speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
    }

    public async void StartRecognition()
    {
        // 🟢 Automatically detect `zh-CN` (Chinese), `ja-JP` (Japanese), `en-US` (English)
        var autoDetectConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "zh-CN", "ja-JP", "en-US" });

        using (var recognizer = new SpeechRecognizer(speechConfig, autoDetectConfig))
        {
            Debug.Log("🎤 Starting speech recognition (Supported: Chinese / Japanese / English)...");
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                // Get the detected language
                var detectedLanguage = AutoDetectSourceLanguageResult.FromResult(result);
                Debug.Log($"🌍 Detected language: {detectedLanguage.Language}");

                // Recognized text
                Debug.Log("✅ Speech recognition successful: " + result.Text);
                FindObjectOfType<ChatGPTManager>().inputField.text = result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                Debug.LogError("❌ Speech recognition failed: No speech detected");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                Debug.LogError("❌ Speech recognition canceled");
            }
        }
    }
}
