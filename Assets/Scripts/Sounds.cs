using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Playrandom",30,30);
    }

void Playrandom(){

    source = GetComponent<AudioSource>();

    source.clip = sounds[Random.Range(0,sounds.Length)];
    source.PlayOneShot(source.clip);
}

}
