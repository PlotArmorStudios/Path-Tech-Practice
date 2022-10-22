using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAllUnits : MonoBehaviour
{
    [SerializeField] private List<Traveler> _units;

    private void Awake()
    {
        CollectUnitsIntoList();
    }

    public void Move()
    {
        CollectUnitsIntoList();

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].GeneratePathAsync();
        }
    }

    private void CollectUnitsIntoList()
    {
        _units = FindObjectsOfType<Traveler>().ToList();
    }
}