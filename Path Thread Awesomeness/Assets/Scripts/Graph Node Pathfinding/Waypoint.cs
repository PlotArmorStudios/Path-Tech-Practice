using UnityEngine;

public class Waypoint : MonoBehaviour
{
    #region Fields

    [SerializeField] int _ID;
    [SerializeField] private float _gizmoRadius = 1;

    #endregion

    #region Unity Methods

    private void OnValidate()
    {
        int index = transform.GetSiblingIndex() + 1;
        _ID = index;
        gameObject.name = $"Waypoint {index}";
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the unique id for the waypoint
    /// 
    /// </summary>
    /// <value>unique id</value>
    public int ID => _ID;

    #endregion

    #region Private Methods

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _gizmoRadius);
    }

    #endregion
}