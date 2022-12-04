using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance() 
    {
        return instance;
    }
    
    public AudioSource EffectsSource;

    [Header("Clips")]
    public AudioClip VmBeepClip;

    [Header("Game State")]
    public bool IsPresent = true;

    [Header("Past-Day Elements")]
    public GameObject PastDayAppsObject;

    [Header("Present-Day Elements")]
    public GameObject CrackedScreenObject;
    public GameObject PresentDayAppsObject;

    private void Awake() 
    {
        if (instance != null) 
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void Start() 
    {
        TogglePresentDay(true);
    }

    public void TogglePresentDay(bool state) 
    {
        IsPresent = state; 

        // Present Elements
        CrackedScreenObject.SetActive(IsPresent);
        PresentDayAppsObject.SetActive(IsPresent);
        
        // Past Elements
        PastDayAppsObject.SetActive(!IsPresent);  
    }

    public void Play(AudioClip clip) 
    {
        EffectsSource.clip = clip;            
        EffectsSource.Play();                
    }

    private IEnumerator VoiceMailBeepDelay(AudioClip clip, float lengthOfDelay) 
    {
        Play(VmBeepClip);
        yield return new WaitForSeconds(lengthOfDelay);
        Play(clip);
    }

    public void PlayVoiceMail(AudioClip clip) 
    {
        float lengthOfDelay = VmBeepClip.length;
        StartCoroutine(VoiceMailBeepDelay(clip, lengthOfDelay));
              
    }

    public void Quit() 
    {    
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
