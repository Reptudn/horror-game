using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{

    [Header("Base Parameters")]

    public int health = 100;
    public int maxHealth = 100;
    public int armor = 0;
    public int maxArmor = 50;


    [Header("HUD")]
    public GameObject healthBar;
    public GameObject armorBar;
    public Text healthValue;
    public Text armorValue;

    private Slider healthSlider;
    private Slider armorSlider;

    void Start(){
        healthSlider = healthBar.GetComponent<Slider>();
        armorSlider  = armorBar.GetComponent<Slider>();
    }

    void Update(){

        if(armorSlider.value <= 0) {
            armorBar.SetActive(false);
        } else {
            armorBar.SetActive(true);
        }

        armorSlider.value  = armor;
        healthSlider.value = health;

        healthValue.text = health + "";
        armorValue.text  = armor + "";

        if(health <= 50) healthValue.color = Color.yellow;
        if(health <= 15) healthValue.color = Color.red;
        if(health > 50) healthValue.color = Color.green;
  
    }

    public void AddHealth(int health){
        int newHealth = this.health + health;
        this.health = newHealth > 100 ? newHealth : 100;
    }

    public void AddArmor(int armor){
        int newArmor = this.armor + armor;
        this.health = newArmor > 100 ? newArmor : 100;
    }

    void Die(){
        //die stuff
    }

    public void GotAttacked(int incomingDamage){

        health -= incomingDamage;
        health = health < 0 ? 0 : health;

        if(health <= 0) Die();

    }
}
