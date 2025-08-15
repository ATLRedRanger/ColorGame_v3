using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UnitAttributes
{
    public string unitName;
    public int maxHealth;
    public int attack;
    public int speed;
    public int defense;
    public float redResistance;
    public float greenResistance;
    public float blueResistance;
    public float axeResistance;
    public float hammerResistance;
    public float staffResistance;
}

[Serializable]
public struct LocationColors
{
    public int maxRed;
    public int currentRed;
    public int maxGreen;
    public int currentGreen;
    public int maxBlue;
    public int currentBlue;
}

public enum Hue
{
    Red, 
    Green,
    Blue,
    White,
    Black

}

public enum WeaponType
{
    Axe,
    Hammer,
    Staff,
    None
}

public enum CombatState
{
    Won,
    Lost,
    Battling
}

public enum AttackTarget
{
    All,
    All_Enemies,
    All_Players,
    Single_Opp,
    Single_Ally
}

public enum LocationType
{
    Combat,
    Shop
}

public enum PlayerChoice
{
    Attacking,
    Defending,
    UsingItem,
    Running,
    Deciding
}