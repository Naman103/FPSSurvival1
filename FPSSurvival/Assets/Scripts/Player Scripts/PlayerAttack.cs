using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    public float nextTimetoFire;
    public float Damage = 50f;

    private Animator zoominAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;
    private bool isAiming;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform arrow_Bow_Start_Position;

    void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();
        zoominAnim = transform.Find("LookRoot").transform.Find("FP Camera").GetComponent<Animator>();
        crosshair = GameObject.FindWithTag("CrossHair");
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInandOut();
    }

    void WeaponShoot()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimetoFire)
            {
                nextTimetoFire = Time.time + 1 / fireRate;
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(weapon_Manager.GetCurrentSelectedWeapon().tag=="Axe")
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    if(isAiming)
                    {
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArroworSpear(true);
                            //shoot arrow
                        }
                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            //shoot spear
                            ThrowArroworSpear(false);
                        }
                    }
                   
                }
            }
        }
    }

    void ZoomInandOut()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if(Input.GetMouseButtonDown(1))
            {
                zoominAnim.Play("ZoomIn");
                crosshair.SetActive(false);
            }
            if (Input.GetMouseButtonDown(1))
            {
                zoominAnim.Play("ZoomOut");
                crosshair.SetActive(true);
            }
        }
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim==WeaponAim.SELF_AIM)
        {
            if(Input.GetMouseButtonDown(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                isAiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                isAiming = false;
            }
        }
    }
    void ThrowArroworSpear(bool throwArrow)
    {
        if(throwArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrow_Bow_Start_Position.position;
            arrow.GetComponent<ArrowBowScript>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrow_Bow_Start_Position.position;
            spear.GetComponent<ArrowBowScript>().Launch(mainCam);
        }
    }
    void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position,mainCam.transform.forward,out hit))
        {
            if(hit.transform.tag=="Enemy")
            {
                hit.transform.GetComponent<Health>().ApplyDamage(Damage);
            }
        }
    }
}
