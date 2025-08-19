using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDatabase : MonoBehaviour
{
    public Dictionary<string, StatusEffect> effects = new Dictionary<string, StatusEffect>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private StatusEffect CreateStatus(string name, int duration, int timeActive, int damage)
    {
        StatusEffect statusEffect = new StatusEffect(name, duration, timeActive, damage);

        return statusEffect;
    }
}
