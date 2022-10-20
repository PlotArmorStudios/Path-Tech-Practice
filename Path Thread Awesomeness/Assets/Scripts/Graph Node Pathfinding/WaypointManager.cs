using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Waypoint> _waypoints;
    [SerializeField] private Waypoint _waypointObject;

    [SerializeField] private int _xSpace = 2;
    [SerializeField] private int _zSpace = 2;
    [SerializeField] private Vector3 _offset;

    private int _numberOfColumns = 3;
    private int _xStart = 2;
    private int _zStart = 2;

    #endregion

    #region Unity Methods

#if UNITY_EDITOR

    private void OnValidate()
    {
        _waypoints = GetComponentsInChildren<Waypoint>().ToList();
    }
#endif

    #endregion

    #region Private Methods
    
    private async void ReorganizeWaypoints()
    {
        int divisor = GetHighestDivisor(_numberOfColumns);
        _numberOfColumns = _waypoints.Count / divisor;
        _offset = new Vector3(-_waypoints.Count / divisor, 0, -_waypoints.Count / divisor);

        for (int i = 0; i < _waypoints.Count; i++)
        {
            _waypoints[i].transform.position = new Vector3(_xStart * (_xSpace * (i % _numberOfColumns)),
                .5f,
                _zStart * (_zSpace * (i / _numberOfColumns))) + _offset + transform.position;
        }

        await Task.Yield();
    }

    private int GetHighestDivisor(int number)
    {
        for (int i = 10; i >= 1; i--)
        {
            if (number % i == 0)
            {
                return i;
            }
        }

        return 1;
    }

    #endregion
    
    #region Public Methods

    [ContextMenu("Regrid Waypoints")]
    public void RegridWaypoints()
    {
        _waypoints = GetComponentsInChildren<Waypoint>().ToList();

        ReorganizeWaypoints();

        GraphRebuilder.RebuildGraph();
    }
    
    public void AddWayPoint()
    {
        var waypoint = Instantiate(_waypointObject, transform.position, Quaternion.identity);
        waypoint.transform.parent = transform;
        GraphRebuilder.RebuildGraph();
    }
    
    #endregion
}