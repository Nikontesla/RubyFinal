using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanwater : MonoBehaviour
{
    private GameObject rubyController;
    public AudioClip cleanClip;
    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
    }
    void OnTriggerStay2D(Collider2D other)
    {

        RubyController ruby = other.GetComponent<RubyController>();
        if (ruby != null)
        {
            ruby.ChangeSpeed(1f);
            ruby.PlaySound(cleanClip);
        }
    }
}
