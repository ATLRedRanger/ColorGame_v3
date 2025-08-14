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
        if(currentLocationColors.currentRed > currentLocationColors.maxRed)
        {
            currentLocationColors.currentRed = currentLocationColors.maxRed;
        }
    }
    public void LoseRed(int amount)
    {
        currentLocationColors.currentRed -= amount;
        if (currentLocationColors.currentRed < 0)
        {
            currentLocationColors.currentRed = 0;
        }
    }
    public void AddGreen(int amount)
    {
        currentLocationColors.currentGreen += amount;
        if (currentLocationColors.currentGreen > currentLocationColors.maxGreen)
        {
            currentLocationColors.currentGreen = currentLocationColors.maxGreen;
        }
    }
    public void LoseGreen(int amount)
    {
        currentLocationColors.currentGreen -= amount;
        if (currentLocationColors.currentGreen < 0)
        {
            currentLocationColors.currentGreen = 0;
        }
    }
    public void AddBlue(int amount)
    {
        currentLocationColors.currentBlue += amount;
        if (currentLocationColors.currentBlue > currentLocationColors.maxBlue)
        {
            currentLocationColors.currentBlue = currentLocationColors.maxBlue;
        }
    }
    public void LoseBlue(int amount)
    {
        currentLocationColors.currentBlue -= amount;
        if (currentLocationColors.currentBlue < 0)
        {
            currentLocationColors.currentBlue = 0;
        }
    }
}
