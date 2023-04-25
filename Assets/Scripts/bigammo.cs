using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigammo : MonoBehaviour
{
    public AudioClip collectedClip;
    private GameObject rubyController;

    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController ruby = rubyController.GetComponent<RubyController>(); ;

        if (ruby != null)
        {
            ruby.PickupCog(3);
            Destroy(gameObject);
            ruby.PlaySound(collectedClip);
        }

    }
}