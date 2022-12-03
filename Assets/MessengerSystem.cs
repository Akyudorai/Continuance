using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class MessengerSystem : MonoBehaviour
{
    public GameObject messagePrefab;
    public GameObject messageBoxParent;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SendText("Potatoes are fascinating creatures.  You should respect them.", false);
        }

        if (Input.GetKeyDown(KeyCode.T)) 
        {
            SendText("I disagree.  Potatoes are disgusting.", true);
        }
    }

    public void SendText(string message, bool isPlayer) 
    {  
        GameObject newMessage = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity);
        newMessage.transform.SetParent(messageBoxParent.transform);

        messageBoxParent.GetComponent<VerticalLayoutGroup>().enabled = false;
        messageBoxParent.GetComponent<VerticalLayoutGroup>().enabled = true;
        
        RectTransform rect = newMessage.GetComponent<RectTransform>();
        rect.localPosition += new Vector3((isPlayer) ? 30 : -30, 0, 0);
        rect.localScale = Vector3.one;

        Image img = newMessage.GetComponent<Image>();
        img.color = (isPlayer) ? Color.blue : Color.grey;

        TMP_Text txt = newMessage.GetComponentInChildren<TMP_Text>();
        txt.text = message;
        
    }
}
