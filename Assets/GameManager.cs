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
    
    public GameObject PastPhone;
    public GameObject PresentPhone;    

    [Header("Clips")]
    public AudioSource EffectsSource;
    public AudioClip VmBeepClip;
    public AudioClip TextSent;
    public AudioClip TextSent2;

    [Header("Game State")]
    public bool IsPresent = true;

    [Header("End")]
    public GameObject endPanel;
    public bool GameOver = false;

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

    private void Update() 
    {
        if (GameOver && Input.GetKeyDown(KeyCode.Q)) 
        {
            Quit();
        }
    }

    public void TogglePresentDay(bool state) 
    {
        IsPresent = state; 

        PastPhone.SetActive(!IsPresent);
        PresentPhone.SetActive(IsPresent);
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

    public void EndGame() 
    {
        endPanel.SetActive(true);
        GameOver = true;
    }

    public void Quit() 
    {    
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
