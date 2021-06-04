using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScript : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 30f;

    public float deactivateTimer = 3f;

    public float damage = 15f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }
    public void Launch(Camera mainCamera)
    {
        rb.velocity = mainCamera.transform.forward * speed;

        transform.LookAt(transform.position + rb.velocity);
    }
    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            other.GetComponent<Health>().ApplyDamage(damage);
        }
    }
}
