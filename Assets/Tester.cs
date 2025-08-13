using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Unit testPlayer;
    public Unit_Enemy testEnemy;
    public GameOrganizer go;
    public AttackDatabase attackDB;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestButton()
    {
        StartCoroutine(go.Combat());
    }
    
    public void DisplayDamageTaken(string name, int value)
    {
        Debug.Log($"{testPlayer.unitAttributes.unitName} has taken ({value}) damage!");
    }

    public void DisplayHealthGained(string name, int value)
    {
       Debug.Log($"{testPlayer.unitAttributes.unitName} has gained ({value}) health!");
    }
}
