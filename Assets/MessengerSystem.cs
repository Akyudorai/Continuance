using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class MessengerSystem : MonoBehaviour
{
    public PhoneManager phone;

    public Animator animator;
    public GameObject messagePrefab;
    public GameObject messageBoxParent;
    public Scrollbar messageScrollbar;

    public GameObject contactListPanel = null;
    public GameObject conversationPanel = null;
    public GameObject conversationButtonPrefab = null;
    public GameObject conversationButtonParent;
    public Scrollbar conversationButtonScrollbar;

    [Header("Conversations")]
    public List<Conversation> conversations = new List<Conversation>();
    public Conversation currentConversation;
    public Dictionary<string, GameObject> conversationButtons = new Dictionary<string, GameObject>();    

    [Header("Messenger Options")]
    public GameObject messageOption1;
    public GameObject messageOption2;

    public bool isFirstTimeOpening = true;    

    private void OnEnable() 
    {
        if (isFirstTimeOpening)
        {            
            //GameManager.GetInstance().StartCoroutine(SendMessageSequence(currentConversation.Responses[0].OptionOne, false));
            InitiateConversation(conversations[0]);
            isFirstTimeOpening = false;
        }

        if (conversationPanel.activeInHierarchy && currentConversation.waitingForPlayerInput) {
            ToggleMessageOptions(true);
        }

        MessengerForwardAnimation();
    }

    private void OnDisable() 
    {
        if (currentConversation.waitingForPlayerInput) {
            ToggleMessageOptions(false);
        }
        
        MessengerBackAnimation();
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            InitiateConversation(conversations[1]);
        }
    }

    public void InitiateConversation(Conversation c) 
    {
        currentConversation = c;
        GameManager.GetInstance().StartCoroutine(SendMessageSequence(c, c.Responses[0].OptionOne, false));
        
        // If the current conversation has not been started, create a new entry in conversations list
        if (!conversationButtons.ContainsKey(c.Name)) 
        {
            // Instantiate and set transform of the new button
            GameObject b = Instantiate(conversationButtonPrefab, Vector3.zero, Quaternion.identity);
            b.transform.SetParent(conversationButtonParent.transform);
            b.transform.localScale = Vector3.one;

            // Refresh the layout settings
            conversationButtonParent.GetComponent<VerticalLayoutGroup>().enabled = false;
            conversationButtonParent.GetComponent<VerticalLayoutGroup>().enabled = true;
            
            // Change the color to indicate a new conversation has started
            Image img = b.GetComponent<Image>();
            img.color = Color.yellow;

            // Set the button event
            b.GetComponent<Button>().onClick.AddListener(delegate{OpenConversation(c);}); 

            // Set the text of the message prefab. 
            TMP_Text txt = b.GetComponentInChildren<TMP_Text>();
            txt.text = c.Name;  

            // Add button to the dictionary
            conversationButtons.Add(c.Name, b);
        } 
        
        // The conversation already exists
        else 
        {
            // Change the color to indicate a new message was sent in this conversation
            Image img = conversationButtons[c.Name].GetComponent<Image>();
            img.color = Color.yellow;
        }
    }

    public void OpenConversation(Conversation c)
    {
        // Change the color to indicate the conversation has been read.
        Image img = conversationButtons[c.Name].GetComponent<Image>();
        img.color = Color.gray;

        LoadConversation(c);

        // Switch to Conversation Panel
        conversationPanel.SetActive(true);
        contactListPanel.SetActive(false);
        
        if (c.waitingForPlayerInput) 
        {
            ToggleMessageOptions(true);
            LoadMessageOptions(c.messageIndex);
        }
        
    }

    public void LoadConversation(Conversation c) 
    {
        currentConversation = c;

        // Clear all existing conversation prefabs
        foreach (Transform child in messageBoxParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        
        // Load in number of prefabs based on historyBool and historyString of conversation class
        int numHistory = c.HistoryString.Count;
        for (int i = 0; i < numHistory; i++) 
        {
            SendText(c, c.HistoryString[i], c.HistoryBool[i], true);
        }

    }

    public void BackButton() 
    {
        if (contactListPanel.activeInHierarchy) 
        {
            gameObject.SetActive(false);
            phone.ReturnToHome();
        } 
        
        else 
        {
            conversationPanel.SetActive(false);
            contactListPanel.SetActive(true);

            if (currentConversation.waitingForPlayerInput && contactListPanel.activeInHierarchy) 
            {
                ToggleMessageOptions(false);
            }
             
        }
        
    }

    public void SendText(Conversation c, string message, bool isPlayer, bool isLoaded = false) 
    {  
        if (currentConversation != c) return;

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

        // Update current conversation's history
        if (!isLoaded) {
            currentConversation.HistoryBool.Add(isPlayer);
            currentConversation.HistoryString.Add(message);
        }
            
    }

    public void SendMessageOption(int index) 
    {           
        // Send the first option sequence.
        if (index == 1) {
            currentConversation.optionPath = 1;
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(currentConversation, currentConversation.Options[currentConversation.messageIndex].OptionOne, true));
        } 

        // Send the second option sequence.
        else if (index == 2) {
            currentConversation.optionPath = 2;
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(currentConversation, currentConversation.Options[currentConversation.messageIndex].OptionTwo, true));
        }

        // Disable the player options while sequence is active.
        ToggleMessageOptions(false);
    }

    public void LoadMessageOptions(int index) 
    {
        // Set the text for both options available to the player to the first option in each sequence.
        TMP_Text displayOne = messageOption1.GetComponentInChildren<TMP_Text>();
        displayOne.text = currentConversation.Options[index].OptionOne.Messages[0];        
        TMP_Text displayTwo = messageOption2.GetComponentInChildren<TMP_Text>();
        displayTwo.text = currentConversation.Options[index].OptionTwo.Messages[0];

        // Turn on the message options for the player to use.
        currentConversation.waitingForPlayerInput = true;
        
        if (conversationPanel.activeInHierarchy) {
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

    public IEnumerator SendMessageSequence(Conversation c, MessageSequence sequence, bool isPlayer) 
    {   
        // Send each message in the sequence, one by one with a 2 second delay after each message.  (can randomize this later).
        for (int i = 0; i < sequence.Messages.Count; i++) 
        {               
            SendText(c, sequence.Messages[i], isPlayer);            
            GameManager.GetInstance().StartCoroutine(SetScrollbarZero());
            
            if (i+1 < sequence.Messages.Count) { 
                int messageLength = sequence.Messages[i+1].Length;
                int wpm = 72;               
                yield return new WaitForSeconds(messageLength * 6/wpm);
            } 

            else {
                yield return new WaitForSeconds(2.0f);
            }
            
        }
        
        // If the the current sequence is the NPCs
        if (!isPlayer) 
        {   
            // Change the color to indicate a new message was sent in this conversation
            Image img = conversationButtons[c.Name].GetComponent<Image>();
            img.color = Color.yellow;

            // Load the player response options   
            if (c.messageIndex < c.Options.Count) {
                LoadMessageOptions(c.messageIndex);
            }
            
            else {
                Debug.Log("End of Conversation");
                phone.readyToSwitchPhone = true;
            }
        } 
        
        // If the current sequence is the Player
        else 
        {
            // Begin a response sequence by the NPC
            c.messageIndex++;
            MessageSequence resultSequence = (c.optionPath == 1) ? c.Responses[c.messageIndex].OptionOne : c.Responses[c.messageIndex].OptionTwo;
            GameManager.GetInstance().StartCoroutine(SendMessageSequence(c, resultSequence, false));
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
