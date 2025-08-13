using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public string attackName;
    public Hue atttackColor;
    public WeaponType attackWeaponRequirement;
    public int attackCost;
    public int attackAccuracy;
    public int attackPower;
    public int healAmount;
    public AttackTarget attackTarget;


    //Attack unlock Requirements
    public int requiredColorLevel;
    public int requiredWeaponLevel;

    public Attack(string attackName, Hue atttackColor, WeaponType attackWeaponRequirement, int attackCost, int attackAccuracy, int attackPower, int healAmount, int requiredColorLevel, int requiredWeaponLevel, AttackTarget attackTarget)
    {
        this.attackName = attackName;
        this.atttackColor = atttackColor;
        this.attackWeaponRequirement = attackWeaponRequirement;
        this.attackCost = attackCost;
        this.attackAccuracy = attackAccuracy;
        this.attackPower = attackPower;
        this.healAmount = healAmount;
        this.requiredColorLevel = requiredColorLevel;
        this.requiredWeaponLevel = requiredWeaponLevel;
        this.attackTarget = attackTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
