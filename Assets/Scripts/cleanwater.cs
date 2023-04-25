using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanwater : MonoBehaviour
{
    private GameObject rubyController;
   public AudioClip cleanClip;
    AudioSource audioSource;
    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        RubyController ruby = other.GetComponent<RubyController>();
        if (ruby != null)
        {
            ruby.ChangeSpeed(1f);
            audioSource.PlayOneShot(cleanClip);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
