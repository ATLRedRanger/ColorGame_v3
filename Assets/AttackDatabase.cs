using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackDatabase : MonoBehaviour
{

    public Dictionary<string, Attack> attackDictionary = new Dictionary<string, Attack>();

    // Start is called before the first frame update
    void Awake()
    {
                                //attackName, atttackColor, attackWeaponRequirement, attackCost, attackAccuracy, attackPower, healAmount, numHits, requiredColorLevel, requiredWeaponLevel
        attackDictionary["Red Attack"] = CreateAttack("Red Attack", Hue.Red, WeaponType.None, 1, 50, 5, 0, 3, 0, 0, AttackTarget.All);
        attackDictionary["Green Attack"] = CreateAttack("Green Attack", Hue.Green, WeaponType.None, 2, 100, 5, 0, 1, 0, 0, AttackTarget.All);
        attackDictionary["Blue Attack"] = CreateAttack("Blue Attack", Hue.Blue, WeaponType.None, 2, 100, 0, 5, 1, 0, 0, AttackTarget.Single_Opp);
        attackDictionary["Basic Attack"] = CreateAttack("Basic Attack", Hue.White, WeaponType.None, 2, 100, 5, 0, 1, 0, 0, AttackTarget.Single_Opp);
    }




    private Attack CreateAttack(string attackName, Hue atttackColor, WeaponType attackWeaponRequirement, int attackCost, int attackAccuracy, int attackPower, int healAmount, int numHits, int requiredColorLevel, int requiredWeaponLevel, AttackTarget attackTarget)
    {
        var attack = new Attack(attackName, atttackColor, attackWeaponRequirement, attackCost, attackAccuracy, attackPower, healAmount, numHits, requiredColorLevel, requiredWeaponLevel, attackTarget);
        
        return attack;
    }

}
