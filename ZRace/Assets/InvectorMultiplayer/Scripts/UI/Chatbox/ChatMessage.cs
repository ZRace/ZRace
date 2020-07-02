using UnityEngine;
using UnityEngine.UI;
using CBGames.Core;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private Text playerName = null;
    [SerializeField] private Text message = null;
    private string userId = null;
    public void SetMessage(SentChatMessage incoming)
    {
        userId = incoming.playerName;
        playerName.text = incoming.playerName.Split(':')[0];
        message.text = incoming.message;
    }
}
