using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageOptions
{
    public MessageSequence OptionOne;
    public MessageSequence OptionTwo;
}

[System.Serializable]
public class MessageSequence 
{
    public List<string> Messages = new List<string>();
}

[System.Serializable]
public class Conversation
{   
    [Header("Identifier")]
    public string Name;

    [Header("Dialogue")]
    // Player Options
    public List<MessageOptions> Options = new List<MessageOptions>();
    
    // NPC Options
    public List<MessageOptions> Responses = new List<MessageOptions>();

    public List<bool> HistoryBool = new List<bool>();
    public List<string> HistoryString = new List<string>();
    
    [Header("System Settings")]
    public bool isPlayerStarted = false;
    public int messageIndex = 0;    // Used to determine how far into the conversation we have gotten.
    public int optionPath = 0;      // Used to determine which option the NPC should use in response to the player.
    public bool waitingForPlayerInput = false; // Used to determine if player options should be available to the player
}
