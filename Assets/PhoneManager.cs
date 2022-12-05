using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public bool readyToSwitchPhone = false;

    [Header("App Panels")]
    public GameObject HomePanel;
    public GameObject AmazonPanel, LCBO_Panel, BankPanel, NotesPanel, GalleryPanel, EmailPanel;
    public GameObject VoicemailPanel, MessengerPanel, PhonePanel, CameraPanel, SettingsPanel;

    public void ReturnToHome() 
    {
        HomePanel.SetActive(true);

        AmazonPanel.SetActive(false);
        LCBO_Panel.SetActive(false);
        BankPanel.SetActive(false);
        NotesPanel.SetActive(false);
        GalleryPanel.SetActive(false);
        EmailPanel.SetActive(false);
        MessengerPanel.SetActive(false);
        PhonePanel.SetActive(false);
        CameraPanel.SetActive(false);
        VoicemailPanel.SetActive(false);
        SettingsPanel.SetActive(false);

        if (readyToSwitchPhone) {
            if (!GameManager.GetInstance().IsPresent) 
            {
                GameManager.GetInstance().EndGame();
            } 
            
            else 
            {
                GameManager.GetInstance().TogglePresentDay(!GameManager.GetInstance().IsPresent);
            }
            
        }
    }
}
