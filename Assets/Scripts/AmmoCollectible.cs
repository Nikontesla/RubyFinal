using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour
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
            ruby.PickupCog(1);
            Destroy(gameObject);
            ruby.PlaySound(collectedClip);
        }

    }
}
