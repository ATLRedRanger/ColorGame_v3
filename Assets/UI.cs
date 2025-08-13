using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    private GameOrganizer go;
    private AttackDatabase attackDB;

    public TMP_Text combatPanelUnitName;
    public GameObject UI_EnemyA;
    public TMP_Text UI_EnemyA_Health;
    public GameObject UI_EnemyB;
    public TMP_Text UI_EnemyB_Health;
    public GameObject UI_PlayerA;
    public TMP_Text UI_PlayerA_Health;
    public GameObject UI_PlayerB;
    public TMP_Text UI_PlayerB_Health;




    public GameObject combatPanel;
    public GameObject spellsPanel;
    public GameObject abilitiesPanel;
    public GameObject itemsPanel;
    public GameObject enemiesPanel;
    public GameObject playersPanel;


    public GameObject attackButton;
    public GameObject spellsButton;
    public GameObject defendButton;
    public GameObject abilitiesButton;
    public GameObject itemsButton;

    
    public GameObject enemyA_Button;
    public GameObject enemyB_Button;

    //Event -> Events I want other scripts to know about
    public static event Action<string> onAttackTargetSelected;
    public static event Action<string> onAttackSelected;
    public static event Action onDefendSelected;

    // Start is called before the first frame update
    void Start()
    {
        go = FindObjectOfType<GameOrganizer>();
        attackDB = FindObjectOfType<AttackDatabase>();
    }

    private void OnEnable()
    {
        GameOrganizer.onNameSet += DisplayEnemyButtonNames;
        GameOrganizer.onNameSet += DisplayCombatantsNames;
        GameOrganizer.onPlayerTurn += DisplayCombatPanels;
        GameOrganizer.onHealthUpdate += DisplayCombatantsHealth;
        Unit.onDamageTaken += DisplayDamageTaken;
        Unit.onHealthGained += DisplayHealthGained;
    }

    private void OnDisable()
    {
        GameOrganizer.onNameSet -= DisplayEnemyButtonNames;
        GameOrganizer.onNameSet -= DisplayCombatantsNames;
        GameOrganizer.onPlayerTurn -= DisplayCombatPanels;
        GameOrganizer.onHealthUpdate -= DisplayCombatantsHealth;
        Unit.onDamageTaken -= DisplayDamageTaken;
        Unit.onHealthGained -= DisplayHealthGained;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayCombatPanels(string unitName)
    {
        ToggleCombatPanel();
        combatPanelUnitName.text = unitName;
    }

    public void ToggleCombatPanel()
    {
        bool isActive = combatPanel.activeSelf;

        if (combatPanel != null)
        {
            combatPanel.SetActive(!isActive);
        }
        
    }

    public void ToggelSpellsPanel()
    {
        if(enemiesPanel.activeSelf != true)
        {
            if (abilitiesPanel.activeSelf == true)
            {
                abilitiesPanel.SetActive(false);
            }

            bool isActive = spellsPanel.activeSelf;

            if (spellsPanel != null)
            {
                spellsPanel.SetActive(!isActive);
            }
        }
        else
        {
            enemiesPanel.SetActive(false);
        }
        

    }

    public void ToggleEnemiesPanel()
    {
        bool isActive = enemiesPanel.activeSelf;

        if (enemiesPanel != null)
        {
            enemiesPanel.SetActive(!isActive);
        }
    }

    public void TogglePlayersPanel()
    {
        bool isActive = playersPanel.activeSelf;

        if (playersPanel != null)
        {
            playersPanel.SetActive(!isActive);
        }
    }

    public void ToggleAbilitiesPanel()
    {
        if(spellsPanel.activeSelf == true)
        {
            spellsPanel.SetActive(false);
        }
        bool isActive = abilitiesPanel.activeSelf;

        if (abilitiesPanel != null)
        {
            abilitiesPanel.SetActive(!isActive);
        }

    }

    public void OnEnemyButtonClick(GameObject button)
    {
        onAttackTargetSelected?.Invoke(button.name);
        ToggleEnemiesPanel();
    }

    public void DisplayEnemyButtonNames()
    {
        
        if(go.enemyA != null)
        {       
            enemyA_Button.GetComponentInChildren<TMP_Text>().text = go.enemyA.unitAttributes.unitName;
        }
        if (go.enemyB != null)
        {
            enemyB_Button.GetComponentInChildren<TMP_Text>().text = go.enemyB.unitAttributes.unitName;
        }
    }

    public void OnSpellButtonClick(GameObject button)
    {

        

        if (attackDB.attackDictionary.ContainsKey(button.name))
        {
            switch (attackDB.attackDictionary[button.name].attackTarget)
            {
                case AttackTarget.All:
                    Debug.Log("TARGETING ALL COMBATANTS");
                    onAttackTargetSelected?.Invoke("All");
                    ToggelSpellsPanel();
                    break;
                case AttackTarget.All_Enemies:
                    Debug.Log("TARGETING ALL ENEMIES");
                    onAttackTargetSelected?.Invoke(button.name);
                    ToggelSpellsPanel();
                    break;
                case AttackTarget.All_Players:
                    Debug.Log("TARGETING ALL PLAYERS");
                    onAttackTargetSelected?.Invoke(button.name);
                    ToggelSpellsPanel();
                    break;
                case AttackTarget.Single_Opp:
                    ToggelSpellsPanel();
                    ToggleEnemiesPanel();
                    break;
                case AttackTarget.Single_Ally:
                    ToggelSpellsPanel();
                    TogglePlayersPanel();
                    break;
            }
            
            onAttackSelected?.Invoke(button.name);
        }
        else
        {
            Debug.Log($"Attack Database doesn't contain an attack for {button.name}");
        }



    }

    public void OnDefendButtonClick()
    {
        onDefendSelected?.Invoke();
        //ToggleCombatPanel();
    }






    private void DisplayCombatantsHealth()
    {
        if (go.enemyA != null)
        {
            UI_EnemyA_Health.text = go.enemyA.currentHealth + " / " + go.enemyA.unitAttributes.maxHealth;
        }
        if (go.enemyB != null)
        {
            UI_EnemyB_Health.text = go.enemyB.currentHealth + " / " + go.enemyB.unitAttributes.maxHealth;
        }
        if (go.playerA != null)
        {
            UI_PlayerA_Health.text = go.playerA.currentHealth + " / " + go.playerA.unitAttributes.maxHealth;
        }
        if (go.playerB != null)
        {
            UI_PlayerB_Health.text = go.playerA.currentHealth + " / " + go.playerA.unitAttributes.maxHealth;
        }
    }

    private void DisplayCombatantsNames()
    {
        if(go.enemyA != null)
        {
            UI_EnemyA.GetComponentInChildren<TMP_Text>().text = go.enemyA.unitAttributes.unitName;
        }
        if (go.enemyB != null)
        {
            UI_EnemyB.GetComponentInChildren<TMP_Text>().text = go.enemyB.unitAttributes.unitName;
        }
        if (go.playerA != null)
        {
            UI_PlayerA.GetComponentInChildren<TMP_Text>().text = go.playerA.unitAttributes.unitName;
        }
        if (go.playerB != null)
        {
            UI_PlayerB.GetComponentInChildren<TMP_Text>().text = go.playerB.unitAttributes.unitName;
        }
    }

    public void DisplayDamageTaken(string name, int value)
    {
        Debug.Log($"{name} has taken ({value}) damage!");
    }

    public void DisplayHealthGained(string name, int value)
    {
        Debug.Log($"{name} has gained ({value}) health!");
    }
}
