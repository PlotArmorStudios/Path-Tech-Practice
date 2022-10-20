using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private Traveler _travelerObject;
    
    public void AddUnit()
    {
        var traveler = Instantiate(_travelerObject, transform.position, Quaternion.identity);
        traveler.transform.parent = transform;
    }
}
