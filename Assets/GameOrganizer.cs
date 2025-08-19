using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GameOrganizer : MonoBehaviour
{

    private AttackDatabase attackDB;
    private StatusEffectDatabase statusEffectDB;

    public CombatState state;
    private PlayerChoice playerChoice;


    [SerializeField]
    private List<Unit> combatants = new List<Unit>();
    [SerializeField]
    private Transform[] enemyPositions;
    [SerializeField]
    private Transform[] playerPositions;

    public Unit_Player activePlayerUnit;

    public Unit enemyA { get; private set; } = null;
    public Unit enemyB { get; private set; } = null;
    public Unit playerA { get; private set; } = null;
    public Unit playerB { get; private set; } = null;


    //Combat Variables
    [SerializeField]
    private string chosenTarget;
    [SerializeField]
    private string chosenAttack;


    //Events
    public static event Action onNameSet;
    public static event Action<Unit_Player> onPlayerTurn;
    public static event Action onPlayerTurnEnd;
    public static event Action onHealthUpdate;
    public static event Action onColorUpdate;
    //public static event Action onCombatStart;

    //Environment Stuff
    public Dictionary<Hue, int> envColros = new Dictionary<Hue, int>();



    // Start is called before the first frame update
    void Start()
    {
        attackDB = FindObjectOfType<AttackDatabase>();
        statusEffectDB = FindObjectOfType<StatusEffectDatabase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        UI.onAttackTargetSelected += AttackTargetSelected;
        UI.onAttackSelected += AttackSelected;
        UI.onDefendSelected += PlayerIsDefending;
    }

    public void OnDisable()
    {
        UI.onAttackTargetSelected -= AttackTargetSelected;
        UI.onAttackSelected -= AttackSelected;
        UI.onDefendSelected -= PlayerIsDefending;
    }

    private void GenerateCombatants()
    {

        enemyA = null;
        enemyB = null;
        playerA = null;
        playerB = null;

        

        int enemyCount = 0;
        int playerCount = 0;

        foreach (Unit unit in combatants)
        {
            if (unit is Unit_Enemy enemy)
            {
                if (enemyCount == 0)
                {
                    enemyA = enemy;
                }
                else if (enemyCount == 1)
                {
                    enemyB = enemy;
                }
                enemyCount++;
            }
            else if (unit is Unit_Player player)
            {
                if (playerCount == 0)
                {
                    playerA = player;
                }
                else if (playerCount == 1)
                {
                    playerB = player;
                }
                playerCount++;
            }
        }

        enemyA.gameObject.SetActive(true);
        enemyB.gameObject.SetActive(true);
        playerA.gameObject.SetActive(true);
        playerB.gameObject.SetActive(true);

        onNameSet?.Invoke();

    }

    public void SortCombatants()
    {
        //Sort all combatants by speed in descending order
        combatants.Sort((y, x) => x.unitAttributes.speed.CompareTo(y.unitAttributes.speed));

    }

    public void PlaceEnemyUnits()
    {
        List<Unit_Enemy> enemyUnits = combatants.OfType<Unit_Enemy>().ToList();
        

        for(int i = 0; i < enemyUnits.Count; i++)
        {
            enemyUnits[i].transform.position = enemyPositions[i].transform.position;
        }
        
    }

    public void PlacePlayerUnits()
    {
        List<Unit_Player> playerUnits = combatants.OfType<Unit_Player>().ToList();


        for (int i = 0; i < playerUnits.Count; i++)
        {
            playerUnits[i].transform.position = playerPositions[i].transform.position;
        }

    }

    private Unit TargetConversion(string target)
    {
        switch (target)
        {
            case "Enemy_A":
                return enemyA;
            case "Enemy_B":
                return enemyB;
            case "Player_A":
                return playerA;
            case "Player_B":
                return playerB;
        }

        return null;
    }

    private bool DoesAttackHit(Attack attack)
    {
        int roll = UnityEngine.Random.Range(1, 101);
        Debug.Log($"Roll: {roll}, Attack Accuracy: {attack.attackAccuracy}");

        if(roll <= attack.attackAccuracy)
        {
            Debug.Log($"The {attack.attackName} hits!");
            return true;
        }

        Debug.Log($"The {attack.attackName} misses!");
        return false;
    }

    private int GetAttackDamage(Attack attack, Unit attacker)
    {
        int damage = 0;


        if (attack.attackPower != 0)
        {
            damage = attack.attackPower + attacker.unitAttributes.attack;
        }

        Debug.Log($"Damage = Attack Power: {attack.attackPower} + Unit's Attack: {attacker.unitAttributes.attack}");


        if(attacker is Unit_Player)
        {
            var playerUnit = (Unit_Player)attacker;
            switch(attack.attackColor)
            {
                case Hue.Red:
                    Debug.Log($"This Red Attack's Damage = Damage: {damage} + Player's Red Level: ({playerUnit.redLevel} * .5)");
                    damage = damage + (int)(playerUnit.redLevel * .5); 
                    break;
            }
        }

        Debug.Log($"Returned Damage: {damage}");
        return damage;

    }

    private int GetHealAmount(Attack attack, Unit attacker)
    {
        int healAmount = attack.healAmount;
        Debug.Log($"Heal Amount = Heal Amount: {attack.healAmount}");


        if (attacker is Unit_Player)
        {
            var playerUnit = (Unit_Player)attacker;
            switch (attack.attackColor)
            {
                case Hue.Red:
                    Debug.Log($"This Red Attack's Heal Amount = Heal Amount: {healAmount} + Player's Red Level: ({playerUnit.redLevel} * .5)");
                    healAmount = healAmount + (int)(playerUnit.redLevel * .5);
                    break;
            }
        }

        Debug.Log($"Returned Heal Amount: {healAmount}");
        return healAmount;
    }

    private void AttackToTarget(Attack attack, int damageAmount, int healAmount, Unit target)
    {
        //TODO: Figure out how to factor in the targets resistance when taking damage and gaining health
        //Solution_1: Add in the attack to the function
        //This seems to be the easiest solution

        float resistance = 0;
        int defendingDamage = 0;

        switch (attack.attackColor)
        {
            case Hue.Red:
                resistance += target.unitAttributes.redResistance;
                break;
            case Hue.Green:
                resistance += target.unitAttributes.greenResistance;
                break;
            case Hue.Blue:
                resistance += target.unitAttributes.blueResistance;
                break;
        }
        Debug.Log($"Resistance: {resistance}");
        Debug.Log($"DamageAmount: {damageAmount} - ({damageAmount * resistance})");
        Debug.Log($"HealAmount: {healAmount} + ({healAmount * resistance})");

        //Mathf.RoundToInt rounds to the nearest even number if .5
        damageAmount = damageAmount - Mathf.RoundToInt(damageAmount * resistance);
        healAmount = healAmount + Mathf.RoundToInt(healAmount * resistance);
        
        if (target != null)
        {
            
            if (healAmount != 0)
            {
                target.GainHealth(target.unitAttributes.unitName, healAmount);
            }

            if (damageAmount != 0)
            {
                if (target.isDefending == true)
                {
                    //Debug.Log("CHERRY");
                    defendingDamage = defendingDamage + Mathf.RoundToInt(damageAmount * .5f);
                    target.LoseHealth(target.unitAttributes.unitName, defendingDamage);
                }
                else
                {
                    //Debug.Log("PEACH");
                    target.LoseHealth(target.unitAttributes.unitName, damageAmount);
                }

                if (!target.unitStatusEffects.Contains(statusEffectDB.effects["Future Sight"]))
                {
                    target.AddStatus(statusEffectDB.effects["Future Sight"].DeepCopy());
                }
                
            }
        }
    }

    public void Target_Single(Attack attack, int damageAmount, int healAmount, Unit targetedUnit)
    {
        AttackToTarget(attack, damageAmount, healAmount, targetedUnit);
    }

    public void Target_All(Attack attack, int damageAmount, int healAmount)
    {
        foreach(Unit unit in combatants)
        {
            if(unit != null)
            {
                AttackToTarget(attack, damageAmount, healAmount, unit);
            }
        }
        /*
        AttackToTarget(attack, damageAmount, healAmount, enemyB);
        AttackToTarget(attack, damageAmount, healAmount, playerA);
        AttackToTarget(attack, damageAmount, healAmount, playerB);
        */
    }

    public void Target_Enemies(Attack attack, int damageAmount, int healAmount)
    {
        foreach(Unit unit in combatants)
        {
            if(unit != null && unit is Unit_Enemy)
            {
                AttackToTarget(attack, damageAmount, healAmount, unit);
            }
        }
        /*
        AttackToTarget(attack, damageAmount, healAmount, enemyA);
        AttackToTarget(attack, damageAmount, healAmount, enemyB);
        */
    }

    public void Target_Players(Attack attack, int damageAmount, int healAmount)
    {
        foreach (Unit unit in combatants)
        {
            if (unit != null && unit is Unit_Player)
            {
                AttackToTarget(attack, damageAmount, healAmount, unit);
            }
        }
        /*
        AttackToTarget(attack, damageAmount, healAmount, playerA);
        AttackToTarget(attack, damageAmount, healAmount, playerB);
        */
    }

    private void CheckUnitStatusEffects()
    {
        foreach (Unit unit in combatants)
        {
            // Create a temporary set to hold effects to remove
            HashSet<StatusEffect> effectsToRemove = new HashSet<StatusEffect>();

            foreach (StatusEffect status in unit.unitStatusEffects)
            {
                status.ApplyEffect(unit);

                if (status.timeActive > status.duration)
                {
                    effectsToRemove.Add(status);
                }
            }

            // Remove the expired effects from the main set
            unit.unitStatusEffects.ExceptWith(effectsToRemove);
        }
    }

    private void AttackTargetSelected(string target)
    {

        chosenTarget = target;
        Debug.Log($"The chosen target is {chosenTarget}");
        PlayerIsAttacking();
    }

    private void AttackSelected(string attack)
    {

        chosenAttack = attack;
        Debug.Log($"The chosen attack is {chosenAttack}");
        PlayerIsAttacking();
    }

    private void PlayerIsAttacking()
    {
        if(activePlayerUnit != null && (chosenAttack != "" && chosenTarget != ""))
        {
            playerChoice = PlayerChoice.Attacking;
            Debug.Log("Player is Attacking");
        }
 
    }

    private void PlayerIsDefending()
    {
        activePlayerUnit.SetDefendingTrue();
        playerChoice = PlayerChoice.Defending;
         
    }

    private bool PlayerChoiceIsMade()
    {
        return playerChoice != PlayerChoice.Deciding;
    }

    public IEnumerator Combat()
    {
        BeginningOfCombat();
        yield return new WaitForSeconds(.2f);
        int round = 0;
        List<Unit> dead = new List<Unit>();

        CombatState combatState = CombatState.Battling;
        while(combatState == CombatState.Battling)
        {
            //Sort the Units that are battling
            SortCombatants();
            RoundBegin();
            round++;
            Debug.Log($"Round: {round}");
            //Go through each Unit
            foreach (Unit unit in combatants)
            {
                if (unit.currentHealth > 0)
                {
                    chosenTarget = "";
                    chosenAttack = "";
                    //If the Unit is PC, PlayerTurn(Unit)
                    if (unit is Unit_Player)
                    {
                        PlayerTurn((Unit_Player)unit);
                        Debug.Log($"It's Player: {unit.unitAttributes.unitName}'s turn!");
                        activePlayerUnit = (Unit_Player)unit;
                        playerChoice = PlayerChoice.Deciding;
                        //Wait until the player's choice is made
                        yield return new WaitUntil(PlayerChoiceIsMade);
                        PlayerTurn((Unit_Player)unit);
                        if (playerChoice == PlayerChoice.Attacking)
                        {
                            Attack attack = attackDB.attackDictionary[chosenAttack];
                            yield return new WaitForSeconds(1f);
                            if (DoesAttackHit(attack))
                            {
                                int damage = GetAttackDamage(attack, unit);
                                int healAmount = GetHealAmount(attack, unit);
                                switch (attack.attackTarget)
                                {
                                    case AttackTarget.All:
                                        Target_All(attack, damage, healAmount);
                                        break;
                                    case AttackTarget.All_Enemies:
                                        Target_Enemies(attack, damage, healAmount);
                                        break;
                                    case AttackTarget.All_Players:
                                        Target_Players(attack, damage, healAmount);
                                        break;
                                    case AttackTarget.Single_Opp:
                                        Target_Single(attack, damage, healAmount, TargetConversion(chosenTarget));
                                        break;
                                    case AttackTarget.Single_Ally:
                                        Target_Single(attack, damage, healAmount, TargetConversion(chosenTarget));
                                        break;
                                }
                                
                            }
                            SubtractColorFromENV(attack.attackColor, attack.attackCost);
                        }
                        if (playerChoice == PlayerChoice.Defending)
                        {
                            Debug.Log("Player is Defending");
                            unit.SetDefendingTrue();

                        }

                        PlayerTurnEnd();
                    }
                    //Else EnemyTurn(Unit)
                    else
                    {
                        Debug.Log($"It's Enemy: {unit.unitAttributes.unitName}'s turn!");
                        yield return new WaitForSeconds (1f);
                        //Target_Single(attackDB.attackDictionary["Red Attack"], attackDB.attackDictionary["Red Attack"].attackPower, attackDB.attackDictionary["Red Attack"].healAmount, playerA);
                        EnemyTurnEnd();
                        
                    }

                    
                }

                CombatHealthUpdate();

                if (playerA.currentHealth == 0 && playerB.currentHealth == 0)
                {
                    Debug.Log("Game Over");
                    combatState = CombatState.Lost;
                    break;
                }
                else if (enemyA.currentHealth == 0 && enemyB.currentHealth == 0)
                {
                    Debug.Log("You've Won!");
                    combatState = CombatState.Won;
                    break;
                }
            }

            yield return new WaitForSeconds (1f);
            CheckUnitStatusEffects();
            AddColorToENV(round, 3);
            RoundEnd();
            //Check Units health
            //If Unit is dead, remove from combatants
            //Check Combatants
            //If there are no PC Units, combatState = CombatState.Lost
            //If there are no Enemy Units, combatState = CombatState.Won
            /*
            if(enemyA.currentHealth == 0 && enemyB.currentHealth == 0)
            {
                combatState = CombatState.Won;
            }*/

            if (playerA.currentHealth == 0 && playerB.currentHealth == 0)
            {
                Debug.Log("Game Over");
                combatState = CombatState.Lost;
                break;
            }
            else if (enemyA.currentHealth == 0 && enemyB.currentHealth == 0)
            {
                Debug.Log("You've Won!");
                combatState = CombatState.Won;
                break;
            }
        }

    }

    private void BeginningOfCombat()
    {
        GenerateCombatants();
        PlaceEnemyUnits();
        PlacePlayerUnits();
        CombatHealthUpdate();

    }

    private void RoundBegin()
    {
        onColorUpdate?.Invoke();
    }

    private void RoundEnd()
    {

        foreach(Unit unit in combatants)
        {
            if (unit.isDefending)
            {
                unit.SetDefendingFalse();
            }
        }
        CombatHealthUpdate();
        onColorUpdate?.Invoke();
    }

    private void CombatHealthUpdate()
    {
        onHealthUpdate?.Invoke();
    }

    private void PlayerTurn(Unit_Player unit)
    {
        onPlayerTurn?.Invoke(unit);

    }

    private void PlayerTurnEnd()
    {
        //CombatHealthUpdate();
        onPlayerTurnEnd?.Invoke();
    }

    private void EnemyTurnEnd()
    {
        
    }

    public bool isAttackUseable(Attack attack)
    {
        if (envColros[attack.attackColor] >= attack.attackCost)
        {
            return true;
        }
        return false;
    }
    
    private void SubtractColorFromENV(Hue color, int amount)
    {
        envColros[color] -= amount;
        onColorUpdate?.Invoke();
    }

    private void AddColorToENV(int round, int amount)
    {
        if(round % 3 == 0)
        {
            envColros[Hue.Red] += amount;
            envColros[Hue.Green] += amount;
            envColros[Hue.Blue] += amount;
        }

        
    }

}
