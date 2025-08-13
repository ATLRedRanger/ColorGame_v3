using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{
    public UnitAttributes unitAttributes => _currentAttributes;

    [SerializeField]
    private UnitAttributes _baseAttributes;
    private UnitAttributes _currentAttributes;

    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _currentAttributes = _baseAttributes;
        currentHealth = _baseAttributes.maxHealth;
    }

    public void Test()
    {
        Debug.Log($"Speed: {_baseAttributes.speed}");
        _currentAttributes.speed += 5;
        Debug.Log($"Speed: {_currentAttributes.speed}");
        Debug.Log($"Attack: {_currentAttributes.attack}");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainHealth(int amount)
    {
        currentHealth =+ amount;
    }
}
