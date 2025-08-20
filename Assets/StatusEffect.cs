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
        switch (statusName)
        {
            case "Burn":
                if(timeActive < duration)
                {
                    dmg += Mathf.RoundToInt(unit.unitAttributes.maxHealth * .05f);
                    unit.LoseHealth(unit.unitAttributes.unitName, dmg);
                }
                break;
            case "Future Sight":
                if (timeActive >= duration)
                {
                    dmg += Mathf.RoundToInt(unit.unitAttributes.maxHealth * .20f);
                    unit.LoseHealth(unit.unitAttributes.unitName, dmg);
                }
                break;
        }

        timeActive++;
    }

    //Override the Equals method to check for value equality
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        StatusEffect other = (StatusEffect)obj;

        //Use a unique identifier, like a status ID or name, for comparison
        return this.statusName == other.statusName;
    }

    //Override the GetHashCode method to match the Equals logic
    public override int GetHashCode()
    {
        //Use the same property for hashing that you use for equality
        return (this.statusName != null) ? this.statusName.GetHashCode() : 0;
    }
}
