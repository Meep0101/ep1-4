using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorcycle : MonoBehaviour
{
    public Color vehicleColor = Color.yellow;
    public string vehicleName = "Motorcycle";
    public int vehicleCount = 3;
    public bool canBeMoved = true;

    public void DecrementCount()
    {
        vehicleCount--;
    }
}
