using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

[System.Serializable]
public class ChatMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatRequest
{
    public string model = "gpt-3.5-turbo";
    public List<ChatMessage> messages = new List<ChatMessage>();
}

[System.Serializable]
public class ChatResponse
{
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public ChatMessage message;
}

public class ChatGPTManager : MonoBehaviour
{
    [Header("OpenAI API Key")]
    public string openAIApiKey = "Your ChatGPT API Key";
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    public InputField inputField;  // Compatible with standard InputField
    public Text chatText;  // Compatible with standard Text
    public AzureTTS azureTTS;
    public AzureSTT azureSTT;

    private List<ChatMessage> messages = new List<ChatMessage>();

    public void SendMessageToChatGPT()
    {
        string userText = inputField.text;
        if (string.IsNullOrEmpty(userText)) return;

        messages.Add(new ChatMessage { role = "user", content = userText });
        chatText.text += "\n👤 " + userText;

        StartCoroutine(SendRequest(userText));
        inputField.text = "";
    }

    private IEnumerator SendRequest(string userText)
    {
        ChatRequest requestData = new ChatRequest { messages = messages };
        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + openAIApiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(request.downloadHandler.text);
                string aiText = chatResponse.choices[0].message.content;

                messages.Add(new ChatMessage { role = "assistant", content = aiText });
                chatText.text += "\n🤖 " + aiText;

                azureTTS.SpeakText(aiText);
            }
            else
            {
                Debug.LogError("❌ ChatGPT request failed: " + request.error);
            }
        }
    }
}
