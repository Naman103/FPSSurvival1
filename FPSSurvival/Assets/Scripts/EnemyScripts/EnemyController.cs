using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK  
}

public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent agent;

    private EnemyState enemy_state;

    public float walk_speed = 0.5f;
    public float run_speed = 4f;

    public float chase_Distance = 7f;
    private float current_chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemyAudio;

    void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag("Player").transform;
        enemyAudio = GetComponentInChildren<EnemyAudio>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy_state = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;

        attack_Timer = wait_Before_Attack;

        current_chase_Distance = chase_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy_state==EnemyState.PATROL)
        {
            Patrol();
        }
        if (enemy_state == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_state == EnemyState.ATTACK)
        {
            Attack();
        }
    }
    void Patrol()
    {
        agent.isStopped = false;
        agent.speed = walk_speed;

        patrol_Timer += Time.deltaTime;

        if(patrol_Timer>patrol_For_This_Time)
        {
            SetRandomNewDestination();

            patrol_Timer = 0f;
        }
        if(agent.velocity.sqrMagnitude>0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }
        if(Vector3.Distance(transform.position,target.position)<=chase_Distance)
        {
            enemy_Anim.Walk(false);

            enemy_state = EnemyState.CHASE;

            //play spotted Audio
            enemyAudio.Play_ScreamSound();
        }
    }
    void Chase()
    {
        agent.isStopped = false;
        agent.speed = run_speed;
        
        agent.SetDestination(target.position);

        if (agent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }
        if(Vector3.Distance(transform.position,target.position)<=attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);

            enemy_state = EnemyState.ATTACK;

            if(chase_Distance!=current_chase_Distance)
            {
                chase_Distance = current_chase_Distance;
            }
        }
        else if(Vector3.Distance(transform.position,target.position)>chase_Distance)
        {
            enemy_Anim.Run(false);
            enemy_state = EnemyState.PATROL;
            patrol_Timer = patrol_For_This_Time;
            if(chase_Distance!=current_chase_Distance)
            {
                chase_Distance = current_chase_Distance;
            }
        }
    }
    void Attack()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        attack_Timer += Time.deltaTime;
        if(attack_Timer>wait_Before_Attack)
        {
            enemy_Anim.Attack();

            attack_Timer = 0f;

            //play attack sound
            enemyAudio.Play_AttackSound();
        }
        if(Vector3.Distance(transform.position,target.position)>attack_Distance+chase_After_Attack_Distance)
        {
            enemy_state = EnemyState.CHASE;
        }
    }

    void SetRandomNewDestination()
    {
        float randRadius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * randRadius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, randRadius, -1);

        agent.SetDestination(navHit.position);
    }
    public void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }

    public void Turn_Off_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get;set;
    }
}

