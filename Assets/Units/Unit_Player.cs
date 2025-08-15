using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Player : Unit
{

    public int redLevel { get; private set; } = 0;
    public int greenLevel { get; private set; } = 0;
    public int blueLevel { get; private set; } = 0;
    public int axeLevel { get; private set; } = 0;
    public int hammerLevel { get; private set; } = 0;
    public int staffLevel { get; private set; } = 0;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

        
    }

    public override void Start()
    {
        base.Start();
        if(unitAttributes.unitName == "Julie")
        {
            
        }

        unitAttackList.Add("Blue Attack");
        unitAttackList.Add("Red Attack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
