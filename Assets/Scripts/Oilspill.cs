using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oilspill : MonoBehaviour
{
    private GameObject rubyController;
    public AudioClip oilClip;
    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
    }
    void OnTriggerStay2D(Collider2D other)
    {

        RubyController ruby = other.GetComponent<RubyController>();
        if (ruby != null)
        {
            ruby.ChangeSpeed(0.5f);
            ruby.PlaySound(oilClip);
        }
    }
}