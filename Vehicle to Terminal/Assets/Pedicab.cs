using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedicab : MonoBehaviour
{
    public Color vehicleColor = Color.blue;
    public string vehicleName = "Pedicab";
    public int vehicleCount = 3;
    public bool canBeMoved = true;

    public void DecrementCount()
    {
        vehicleCount--;
    }
}
