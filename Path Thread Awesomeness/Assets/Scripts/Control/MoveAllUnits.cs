using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAllUnits : MonoBehaviour
{
    [SerializeField] private List<Traveler> _units;

    private void Awake()
    {
        _units = FindObjectsOfType<Traveler>().ToList();
    }

    public void Move()
    {
        _units = FindObjectsOfType<Traveler>().ToList();
        
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].GeneratePathAsync();
        }
    }
}