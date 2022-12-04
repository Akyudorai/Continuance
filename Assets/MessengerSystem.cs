using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

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

public class MessengerSystem : MonoBehaviour
{
    public Animator animator;
    public GameObject messagePrefab;
    public GameObject messageBoxParent;
    public Scrollbar messageScrollbar;

    [Header("Message Options")]
    public List<MessageOptions> Options = new List<MessageOptions>();
    public List<MessageOptions> Responses = new List<MessageOptions>();
    public int messageIndex = 0;
    public int optionPath = 0;

    [Header("Messenger Options")]
    public GameObject messageOption1;
    public GameObject messageOption2;

    public bool isFirstTimeOpening = true;
    public bool waitingForPlayerInput = false;

    private void OnEnable() 
    {
        if (isFirstTimeOpening)
        {            
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(Responses[0].OptionOne, false));
            isFirstTimeOpening = false;
        }

        if (waitingForPlayerInput) {
            ToggleMessageOptions(true);
        }

        MessengerForwardAnimation();
    }

    private void OnDisable() 
    {
        if (waitingForPlayerInput) {
            ToggleMessageOptions(false);
        }
        
        MessengerBackAnimation();
    }

    public void SendText(string message, bool isPlayer) 
    {   
        // Generate the message prefab and set prefab to the message panel.
        GameObject newMessage = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity);
        newMessage.transform.SetParent(messageBoxParent.transform);

        // Toggle on and off to readjust the layout settings?
        messageBoxParent.GetComponent<VerticalLayoutGroup>().enabled = false;
        messageBoxParent.GetComponent<VerticalLayoutGroup>().enabled = true;
        
        // Trying to reposition the message left or right depending on if it's an NPC or Player (not working)
        RectTransform rect = newMessage.GetComponent<RectTransform>();
        rect.localPosition += new Vector3((isPlayer) ? 30 : -30, 0, 0);
        rect.localScale = Vector3.one;

        // Change the color of the message based on if it's an NPC or Player
        Image img = newMessage.GetComponent<Image>();
        img.color = (isPlayer) ? Color.blue : Color.grey;

        // Set the text of the message prefab. 
        TMP_Text txt = newMessage.GetComponentInChildren<TMP_Text>();
        txt.text = message;        
    }

    public void SendMessageOption(int index) 
    {           
        // Send the first option sequence.
        if (index == 1) {
            optionPath = 1;
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(Options[messageIndex].OptionOne, true));
        } 

        // Send the second option sequence.
        else if (index == 2) {
            optionPath = 2;
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(Options[messageIndex].OptionTwo, true));
        }

        // Disable the player options while sequence is active.
        ToggleMessageOptions(false);
    }

    public void LoadMessageOptions(int index) 
    {
        // Set the text for both options available to the player to the first option in each sequence.
        TMP_Text displayOne = messageOption1.GetComponentInChildren<TMP_Text>();
        displayOne.text = Options[index].OptionOne.Messages[0];        
        TMP_Text displayTwo = messageOption2.GetComponentInChildren<TMP_Text>();
        displayTwo.text = Options[index].OptionTwo.Messages[0];

        // Turn on the message options for the player to use.
        waitingForPlayerInput = true;
        
        if (gameObject.activeInHierarchy) {
            ToggleMessageOptions(true);
        }
        
        

    }

    private IEnumerator SetScrollbarZero() 
    {
        // Weird way we have to go through to make scrollbar auto-adjust every time a message is sent
        yield return null;
        yield return null;
        messageScrollbar.value = 0;
    }

    public IEnumerator SendMessageSequence(MessageSequence sequence, bool isPlayer) 
    {   
        // Send each message in the sequence, one by one with a 2 second delay after each message.  (can randomize this later).
        for (int i = 0; i < sequence.Messages.Count; i++) 
        {               
            SendText(sequence.Messages[i], isPlayer);            
            GameManager.GetInstance().StartCoroutine(SetScrollbarZero());
            
            if (i < sequence.Messages.Count) {
                yield return new WaitForSeconds(2.0f);
            } 

            else {
                yield return new WaitForSeconds(6.0f);
            }
            
        }
        
        // If the the current sequence is the NPCs
        if (!isPlayer) 
        {   
            // Load the player response options   
            if (messageIndex < Options.Count) {
                LoadMessageOptions(messageIndex);
            }
            
            else {
                Debug.Log("End of Conversation");
            }
        } 
        
        // If the current sequence is the Player
        else 
        {
            // Begin a response sequence by the NPC
            messageIndex++;
            MessageSequence resultSequence = (optionPath == 1) ? Responses[messageIndex].OptionOne : Responses[messageIndex].OptionTwo;
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(resultSequence, false));
        }
    }

    public void ToggleMessageOptions(bool state) 
    {
        messageOption1.SetActive(state);
        messageOption2.SetActive(state);
    }

    public void MessengerForwardAnimation() 
    {
        animator.SetTrigger("MessengerForward");
    }

    public void MessengerBackAnimation() 
    {
        animator.SetTrigger("MessengerBack");
    }
}
