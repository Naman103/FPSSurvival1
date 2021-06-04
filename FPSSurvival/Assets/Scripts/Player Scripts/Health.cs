using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    private EnemyAnimator enemy_anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_controller;

    private float health = 100f;

    public bool is_Player, is_Boar, is_Cannibal;

    private bool is_Dead;

    private EnemyAudio enemyAudio;

    private PlayerStats playerStats;
   
    void Awake()
    {
     if(is_Boar || is_Cannibal)
        {
            enemy_anim = GetComponent<EnemyAnimator>();
            enemy_controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemyAudio = GetComponentInChildren<EnemyAudio>();
            //get enemy audio
        }
      
     if(is_Player)
        {
            playerStats = GetComponent<PlayerStats>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ApplyDamage(float damage)
    {
        if (is_Dead)
            return;

        health -= damage;

        if(is_Player)
        {
            //Show the stats in UI
            playerStats.DisplayHealth_Stats(health);
        }

        if(is_Boar || is_Cannibal)
        {
            if(enemy_controller.Enemy_State==EnemyState.PATROL)
            {
                enemy_controller.chase_Distance = 50f;
            }
        }
        if(health<=0)
        {
            PlayerDied();

            is_Dead = true;
        }
    }
    void PlayerDied()
    {
        if(is_Cannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 50f);

            enemy_controller.enabled = false;
            navAgent.enabled = false;
            enemy_anim.enabled = false;

            //Start Coroutine
            StartCoroutine(DeadSound());
            //EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied(true);
        }
        if(is_Boar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_controller.enabled = false;

            enemy_anim.Dead();

            //Start Coroutine
            StartCoroutine(DeadSound());
            // Spawn more enemies
            EnemyManager.instance.EnemyDied(false);
        }

        if(is_Player)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for(int i=0;i<enemies.Length;i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);

            EnemyManager.instance.StopSpawning();
        }

        if(tag=="Player")
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }//Player Died
    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.Play_DieSound();
    }
}
