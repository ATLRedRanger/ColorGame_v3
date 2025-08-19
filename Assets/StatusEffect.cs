using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public string statusName { get; private set; } = "";
    public int duration { get; private set; } = 0;
    public int timeActive { get; private set; } = 0;

    private int damage;


    public StatusEffect(string statusName = null, int duration = 0, int timeActive = 0, int damage = 0)
    {
        this.statusName = statusName;
        this.duration = duration;
        this.timeActive = timeActive;
        this.damage = damage;
    }

    public StatusEffect DeepCopy()
    {
        StatusEffect status = new StatusEffect();
        status.statusName = this.statusName;
        status.duration = this.duration;
        status.timeActive = this.timeActive;
        status.damage = this.damage;


        return status;
    }

    public void ApplyEffect(Unit unit)
    {
        int dmg = damage;
        Debug.Log($"Time Active: {timeActive}, Duration: {duration}");
        Debug.Log($"");
        if (timeActive < duration)
        {
            switch (this.statusName)
            {
                case "Burn":
                    dmg += Mathf.RoundToInt(unit.unitAttributes.maxHealth * .05f);
                    Debug.Log($"Unit Max Health: {unit.unitAttributes.maxHealth}");
                    unit.LoseHealth(unit.unitAttributes.unitName, dmg);
                    timeActive++;
                    break;

            }
        }
        
         
        
    }
}
