using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator animator;
    public AudioSource EffectsSource;

    [Header("Clips")]
    public AudioClip VmBeepClip;

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

    public void MessengerForwardAnimation() 
    {
        animator.SetTrigger("MessengerForward");
    }

    public void MessengerBackAnimation() 
    {
        animator.SetTrigger("MessengerBackward");
    }

    public void Quit() 
    {    
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
