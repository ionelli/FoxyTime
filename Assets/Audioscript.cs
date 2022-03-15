using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioscript : MonoBehaviour
{
    public audiClip[] sounds;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>
    }

    // Update is called once per frame
    void Update()
    {
        source.clip(Random.Range[0,sounds.Length)]);
        source.PlayOneShot(source.clip);
    }
}
