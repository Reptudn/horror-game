using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatItem : MonoBehaviour, ICombatItem
{

    [Header("Basic")]
    public int damage;
    public int cooldown;
    public Animator itemAnimator;
    public KeyCode key = KeyCode.R;

    [Header("Ammo")]
    public bool hasAmmo = false;
    public int ammo = 0;

    [Header("Melee")]
    public int hitDistance = 3;


    private bool hasRun = false;

    public void Attack()
    {

        if (hasRun) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {

            Debug.DrawLine(transform.position, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z), Color.magenta, Time.deltaTime);

            if (hasAmmo)
            {
                if (ammo <= 0) return;
                ammo -= 1;
            }

            itemAnimator.SetTrigger("AttackTrigger");

            if (hit.distance > hitDistance) return;

            hit.transform.gameObject.BroadcastMessage("Damage", damage);
            //transform.gameObject.BroadcastMessage("AttackAnimation"); //or just call the animator idk whats best yet 

        }

        hasRun = true;
        WaitForCooldown();
        hasRun = false;
    }

    IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) Attack();

    }

}
