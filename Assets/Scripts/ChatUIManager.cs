using UnityEngine;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour
{
    public Text chatText;  
    public InputField inputField;   
    public Button sendButton;  
    public Button voiceButton;  
    public ChatGPTManager chatGPTManager;
    public AzureSTT azureSTT;

    void Start()
    {
        sendButton.onClick.AddListener(OnSendMessage);
        voiceButton.onClick.AddListener(OnVoiceInput);
    }

    /// <summary>
    /// Send message
    /// </summary>
    public void OnSendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            chatGPTManager.SendMessageToChatGPT();
        }
    }

    /// <summary>
    /// Voice recognition
    /// </summary>
    public void OnVoiceInput()
    {
        azureSTT.StartRecognition();
    }

    /// <summary>
    /// Display user message in the chat window
    /// </summary>
    public void DisplayUserMessage(string message)
    {
        chatText.text += "\n👤 " + message;
    }

    /// <summary>
    /// Display AI message in the chat window
    /// </summary>
    public void DisplayAIMessage(string message)
    {
        chatText.text += "\n🤖 " + message;
    }

    /// <summary>
    /// Clear chat history
    /// </summary>
    public void ClearChat()
    {
        chatText.text = "";
    }
}
