using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim
{
    NONE,
    SELF_AIM,
    AIM
}
public enum WeaponFireType
{
    SINGLE,
    MULTIPLE
}
public enum WeaponBulletType
{
    BULLET,
    ARROW,
    SPEAR,
    NONE
}

public class WeaponHandler : MonoBehaviour
{
    private Animator anim;

    public WeaponAim weapon_Aim;

    [SerializeField]
    private GameObject muzzleFlash;

    [SerializeField]
    private AudioSource shoot_Sound, reload_Sound;

    public WeaponFireType fireType;

    public WeaponBulletType bulletType;

    public GameObject attack_Point;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ShootAnimation()
    {
        anim.SetTrigger("Shoot");
    }

    public void Aim(bool canAim)
    {
        anim.SetBool("Aim", canAim);
    }

    public void TurnOnMuzzleFlash()
    {
        muzzleFlash.SetActive(true); 
    }

    public void TurnOffMuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }

    public void ShootSound()
    {
        shoot_Sound.Play();
    }

    public void ReloadSound()
    {
        reload_Sound.Play();
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
}
