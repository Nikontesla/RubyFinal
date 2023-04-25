using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metalcube : MonoBehaviour

{
    public AudioClip breakClip;
    public ParticleSystem openpar;
    private GameObject rubyController;

    void Start()
    {
        rubyController = GameObject.FindWithTag("RubyController");
    }

    public void Open()
    {
        RubyController ruby = rubyController.GetComponent<RubyController>(); ;

        if (ruby != null)
        {
            CreateOpenpar();
            ruby.PlaySound(breakClip);
            Destroy(gameObject);
        }

    }
    void CreateOpenpar()
    {
        openpar.Play();
    }
}