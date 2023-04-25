using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oilspill : MonoBehaviour
{
    private GameObject rubyController;
    public AudioClip oilClip;
    AudioSource audioSource;
    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerStay2D(Collider2D other)
    {

        RubyController ruby = other.GetComponent<RubyController>();
        if (ruby != null)
        {
            ruby.ChangeSpeed(0.5f);
            audioSource.PlayOneShot(oilClip);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}