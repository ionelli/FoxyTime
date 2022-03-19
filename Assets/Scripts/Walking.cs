using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource animationSoundPlayer;

    // Update is called once per frame
    private void Foxwalking()
    {
        animationSoundPlayer.Play();
    }
    private void FoxNotwalking(){
        animationSoundPlayer.Stop();
    }
}
