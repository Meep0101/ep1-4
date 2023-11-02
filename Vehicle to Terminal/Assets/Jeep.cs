using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeep : MonoBehaviour
{
    public Color vehicleColor = Color.green;
    public string vehicleName = "Jeep";
    public int vehicleCount = 3;
    public bool canBeMoved = true;

    public void DecrementCount()
    {
        vehicleCount--;
    }
}
