using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footsteps_Sound;

    [SerializeField]
    private AudioClip[] footsteps_Clip;

    private CharacterController character_controller;

    [HideInInspector]
    public float minVolume, maxVolume;

    private float accumulated_Distance;

    [HideInInspector]
    public float step_Distance;
    // Start is called before the first frame update
    void Awake()
    {
        footsteps_Sound = GetComponent<AudioSource>();

        character_controller = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlayFootstepSound();
    }

    void CheckToPlayFootstepSound()
    {
        if (!character_controller.isGrounded)
            return;

        if(character_controller.velocity.sqrMagnitude>0)
        {
            accumulated_Distance += Time.deltaTime;

            if(accumulated_Distance>step_Distance)
            {
                footsteps_Sound.volume = Random.Range(minVolume, maxVolume);
                footsteps_Sound.clip = footsteps_Clip[Random.Range(0, footsteps_Clip.Length)];
                footsteps_Sound.Play();

                accumulated_Distance = 0f;
            }
            
        }
        else
        {
            accumulated_Distance = 0f;
        }
    }
}
