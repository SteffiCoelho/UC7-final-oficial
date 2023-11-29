using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    private AudioSource player;
    [SerializeField] 
    private List<AudioClip> audios = new List<AudioClip>();
    [SerializeField]
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        StartCoroutine(PlaySounds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator PlaySounds()
    {
        player.PlayOneShot(audios[Random.Range(0, audios.Count)]);
        yield return new WaitForSeconds(time);

        StartCoroutine(PlaySounds());
    }
}
