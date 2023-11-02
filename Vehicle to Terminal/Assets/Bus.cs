using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour
{
    public Color vehicleColor = Color.red;
    public string vehicleName = "Bus";
    public int vehicleCount = 3;
    public bool canBeMoved = true;

    public void DecrementCount()
    {
        vehicleCount--;
    }
}
