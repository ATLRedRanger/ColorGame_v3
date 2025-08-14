using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class GameOrganizer : MonoBehaviour
{

    private AttackDatabase attackDB;

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
    public static event Action onHealthUpdate;
    public static event Action onColorUpdate;
    //public static event Action onCombatStart;

    //Environment Stuff
    public Dictionary<Hue, int> envColros = new Dictionary<Hue, int>();



    // Start is called before the first frame update
    void Start()
    {
        attackDB = FindObjectOfType<AttackDatabase>();
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
                    Debug.Log($"EnemyA: {enemyA.unitAttributes.unitName}");
                }
                else if (enemyCount == 1)
                {
                    enemyB = enemy;
                    Debug.Log($"EnemyB: {enemyB.unitAttributes.unitName}");
                }
                enemyCount++;
            }
            else if (unit is Unit_Player player)
            {
                if (playerCount == 0)
                {
                    playerA = player;
                    Debug.Log($"PlayerA: {playerA.unitAttributes.unitName}");
                }
                else if (playerCount == 1)
                {
                    playerB = player;
                    Debug.Log($"PlayerB: {playerB.unitAttributes.unitName}");
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

    private void AttackToTarget(Attack attack, Unit target)
    {
        int damageAmount = attack.attackPower;
        int healAmount = attack.healAmount;

        if (target != null)
        {
            if (attack.healAmount != 0)
            {
                target.GainHealth(target.unitAttributes.unitName, healAmount);
            }

            if (attack.attackPower != 0)
            {
                target.LoseHealth(target.unitAttributes.unitName, damageAmount);
            }
        }
    }

    public void Target_Single(Attack attack, Unit targetedUnit)
    {
        AttackToTarget(attack, targetedUnit);
    }

    public void Target_All(Attack attack)
    {
        AttackToTarget(attack, enemyA);
        AttackToTarget(attack, enemyB);
        AttackToTarget(attack, playerA);
        AttackToTarget(attack, playerB);
    }

    public void Target_Enemies(Attack attack)
    {
        AttackToTarget(attack, enemyA);
        AttackToTarget(attack, enemyB);
    }

    public void Target_Players(Attack attack)
    {
        AttackToTarget(attack, playerA);
        AttackToTarget(attack, playerB);
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
            TurnBegin();
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
                            switch (attack.attackTarget)
                            {
                                case AttackTarget.All:
                                    Target_All(attack);
                                    break;
                                case AttackTarget.All_Enemies:
                                    Target_Enemies(attack);
                                    break;
                                case AttackTarget.All_Players:
                                    Target_Players(attack);
                                    break;
                                case AttackTarget.Single_Opp:
                                    Target_Single(attack, TargetConversion(chosenTarget));
                                    break;
                                case AttackTarget.Single_Ally:
                                    Target_Single(attack, TargetConversion(chosenTarget));
                                    break;
                            }
                            SubtractColorFromENV(attack.attackColor, attack.attackCost);
                        }
                        if (playerChoice == PlayerChoice.Defending)
                        {
                            Debug.Log("Player is Defending");
                        }
                        
                    }
                    //Else EnemyTurn(Unit)
                    else
                    {
                        yield return new WaitForSeconds (1f);
                        Debug.Log($"It's Enemy: {unit.unitAttributes.unitName}'s turn!");
                    }
                }
                TurnEnd();
            }

            yield return new WaitForSeconds (1f);

            if(playerA.currentHealth == 0 && playerB.currentHealth == 0)
            {
                Debug.Log("Game Over");
                combatState = CombatState.Lost;
            }

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
        }

    }

    private void BeginningOfCombat()
    {
        GenerateCombatants();
        PlaceEnemyUnits();
        PlacePlayerUnits();
        CombatHealthUpdate();

    }

    private void TurnBegin()
    {
        onColorUpdate?.Invoke();
    }

    private void TurnEnd()
    {
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

}
