using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    static public bool goalMet = false;
    public AudioSource goalHit;

    private bool soundPlayed = false;

    // Start is called before the first frame update
   void OnTriggerEnter(Collider other)
   {
    Projectile proj = other.GetComponent<Projectile>();
    if(proj != null)
    {
        if(soundPlayed == false && other.CompareTag("CurrentProjectile"))
        {
            goalHit.Play();
            print("DEBUG: goal entered");
        }
        Goal.goalMet = true;
        Material mat = GetComponent<Renderer>().material;
        Color c = mat.color;
        c.a = 0.75f;
        mat.color = c;
    }
   }
}
