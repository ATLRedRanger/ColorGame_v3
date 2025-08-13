using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitAttributes unitAttributes => _currentAttributes;

    [SerializeField]
    private UnitAttributes _baseAttributes;
    private UnitAttributes _currentAttributes;

    [SerializeField]
    public int currentHealth = 0;

    public List<string> unitAttackList { get; private set; } = new List<string>();


    //Trying Events
    public static event Action<string, int> onDamageTaken;
    public static event Action<string, int> onHealthGained;

    public bool isDefending { get; private set; } = false;
    public bool hadTurn = false;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        _currentAttributes = _baseAttributes;
        currentHealth = unitAttributes.maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetCurrentAttributes()
    {
        _currentAttributes = _baseAttributes;
    }

    public void LoseHealth(string name, int value)
    {
        currentHealth -= value;

        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        onDamageTaken?.Invoke(unitAttributes.unitName, value);

    }
    public void GainHealth(string name, int value)
    {
        currentHealth += value;

        if (currentHealth > unitAttributes.maxHealth)
        {
            currentHealth = unitAttributes.maxHealth;
        }

        onHealthGained?.Invoke(unitAttributes.unitName, value);

    }

    public void SetDefendingTrue()
    {
        isDefending = true;
    }
    public void SetDefendingFalse()
    {
        isDefending = false;
    }
}
