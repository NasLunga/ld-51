using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class DoorsController : MonoBehaviour
{
    public bool open {get; private set;} = false;
    public AudioClip openingSound;
    public AudioClip footstepsSound;
    public AudioClip closingSound;
    private Animator animator;
    
    private AudioSource audioSource;
    


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    

    public void Open()
    {
        open = true;
        audioSource.clip = openingSound;
        audioSource.Play();
        animator.SetTrigger("Open");
    }
    
    void OnCollisionEnter2D(Collision2D coll) {
        if (open && coll.gameObject.CompareTag("Player")) {
            coll.gameObject.SendMessage("Stun", 10);
            StartCoroutine(PlayClosingSound());
            StartCoroutine(GameManager.instance.LoadNextLevel());
        }
    }

    IEnumerator PlayClosingSound() {
        audioSource.clip = closingSound;
        audioSource.Play();

        while (audioSource.isPlaying) {
            yield return null;
        }

        audioSource.clip = footstepsSound;
        audioSource.Play();
    }
}
