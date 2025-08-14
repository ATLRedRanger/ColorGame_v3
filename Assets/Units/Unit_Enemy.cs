using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Enemy : Unit
{
    public Hue colorWeakness { get; private set; } = Hue.Red;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColorWeakness(Hue color)
    {
        colorWeakness = color;
    }
}
