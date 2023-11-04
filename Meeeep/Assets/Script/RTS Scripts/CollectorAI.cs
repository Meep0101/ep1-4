using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorAI : MonoBehaviour
{
    public enum Resource
    {
        Wood, //station
        Minerals, //terminal
    }

    [SerializeField] private Resource _resource;
}
