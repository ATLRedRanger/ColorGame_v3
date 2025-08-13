using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationNode : MonoBehaviour
{
    public LocationColors locationColors => currentLocationColors;

    private LocationColors currentLocationColors;
    [SerializeField]
    private LocationColors baseLocationColors;

    public string iD;
    public string nodeName;
    public LocationType locationType;
    public List<string> connectedNodeLocations = new List<string>();
    public Unit_Enemy[] locationEnemies;

    // Start is called before the first frame update
    void Start()
    {
        currentLocationColors = baseLocationColors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsConnectedTo(string targetNodeId)
    {
        return connectedNodeLocations.Contains(targetNodeId);
    }

    public Unit_Enemy GenerateEnemy()
    {
        int roll = Random.Range(1, locationEnemies.Length);

        return locationEnemies[roll];
    }

    public void AddRed(int amount)
    {
        currentLocationColors.currentRed += amount;
    }
    public void LoseRed(int amount)
    {
        currentLocationColors.currentRed -= amount;
    }
    public void AddGreen(int amount)
    {
        currentLocationColors.currentGreen += amount;
    }
    public void LoseGreen(int amount)
    {
        currentLocationColors.currentGreen -= amount;
    }
    public void AddBlue(int amount)
    {
        currentLocationColors.currentBlue += amount;
    }
    public void LoseBlue(int amount)
    {
        currentLocationColors.currentBlue -= amount;
    }
}
