using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public AudioClip bombClip;
    private GameObject rubyController;
    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController ruby = rubyController.GetComponent<RubyController>();

        if (ruby != null)
        {
            ruby.PlaySound(bombClip);
            ruby.ChangeHealth(-3);
            Destroy(gameObject);
        }

    }
}
