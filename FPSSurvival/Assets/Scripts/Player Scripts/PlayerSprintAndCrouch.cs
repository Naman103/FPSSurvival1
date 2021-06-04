using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playermovement;

    public float sprint_speed = 10f;
    public float move_speed = 5f;
    public float crouch_speed = 2f;

    private Transform look_Root;
    private float stand_height = 1.6f;
    private float crouch_height = 1f;

    private bool isCrouching;

    private PlayerFootsteps player_Footsteps;

    private float sprint_volume = 1f;
    private float crouch_volume = 0.1f;
    private float walk_volume_min = 0.2f, walk_volume_max = 0.6f;

    private float walk_step_Distance = 0.4f;
    private float sprint_Step_Distance = 0.25f;
    private float crouch_Step_Distance = 0.5f;

    private PlayerStats playerStats;

    private float sprint_Value = 100f;
    private float sprint_Threshold = 10f;

    private CharacterController controller;

    void Awake()
    {
        playermovement = GetComponent<PlayerMovement>();

        look_Root = transform.GetChild(0);

        player_Footsteps = GetComponentInChildren<PlayerFootsteps>();

        playerStats = GetComponent<PlayerStats>();

        controller = GetComponent<CharacterController>();
       
    }

    void Start()
    {
        player_Footsteps.minVolume = walk_volume_min;
        player_Footsteps.maxVolume = walk_volume_max;
    }
    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if (sprint_Value > 0 && controller.velocity.sqrMagnitude>0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching )
            {
                playermovement.speed = sprint_speed;

                player_Footsteps.step_Distance = sprint_Step_Distance;
                player_Footsteps.minVolume = sprint_volume;
                player_Footsteps.maxVolume = sprint_volume;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playermovement.speed = move_speed;

            player_Footsteps.step_Distance = walk_step_Distance;
            player_Footsteps.minVolume = walk_volume_min;
            player_Footsteps.maxVolume = walk_volume_max;
        }
        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching && controller.velocity.sqrMagnitude>0)
        {
            sprint_Value -= Time.deltaTime * sprint_Threshold;
            if(sprint_Value<=0f)
            {
                sprint_Value = 0f;
                playermovement.speed = move_speed;


                player_Footsteps.step_Distance = walk_step_Distance;
                player_Footsteps.minVolume = walk_volume_min;
                player_Footsteps.maxVolume = walk_volume_max;
            }
            playerStats.DisplayStamina_Stats(sprint_Value);
        }
        else if(sprint_Value!=100f)
        {
            sprint_Value += (sprint_Threshold / 2f) * Time.deltaTime;
            if(sprint_Value>100f)
            {
                sprint_Value = 100f;
            }
            playerStats.DisplayStamina_Stats(sprint_Value);
        }
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                look_Root.localPosition = new Vector3(0f, stand_height, 0f);
                playermovement.speed = move_speed;
                player_Footsteps.step_Distance = walk_step_Distance;
                player_Footsteps.minVolume = walk_volume_min;
                player_Footsteps.maxVolume = walk_volume_max;
                isCrouching = false;
            }
            else
            {
                look_Root.localPosition = new Vector3(0f, crouch_height, 0f);
                playermovement.speed = crouch_speed;
                player_Footsteps.minVolume = crouch_volume;
                player_Footsteps.maxVolume = crouch_volume;
                player_Footsteps.step_Distance = crouch_Step_Distance;

                isCrouching = true;
            }
        }
    }
}
