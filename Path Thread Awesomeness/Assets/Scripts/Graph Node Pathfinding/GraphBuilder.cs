using System;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphBuilder : MonoBehaviour
{
    #region Evemts

    public static event Action OnRebuildGraph;
    public static event Action OnRebuildEdges;

    #endregion

    #region Fields

    [SerializeField] private float _pathDetectionRange = 6;
    static Graph<Waypoint> graph;

    #endregion

    #region Properties

    public static Graph<Waypoint> Graph
    {
        get { return graph; }
        set { graph = value; }
    }

    #endregion

    #region Unity Methods

    public void Awake()
    {
        BuildGraph();
    }

    private void OnEnable()
    {
        OnRebuildGraph += BuildGraph;
    }

    private void OnDisable()
    {
        OnRebuildGraph -= BuildGraph;
    }

    #endregion

    #region Private Methods

    private void Build(Waypoint[] waypoints)
    {
        foreach (var waypoint in waypoints)
        {
            graph.AddNode(waypoint);
        }

        // add neighbors for each node in graph
        for (int i = 0; i < graph.Nodes.Count; i++)
        {
            for (int j = 0; j < waypoints.Length; j++)
            {
                var nodeDistance = Vector3.Distance(graph.Nodes[i].Value.transform.position,
                    graph.Nodes[j].Value.transform.position);

                bool correctNodeDistance = nodeDistance <= _pathDetectionRange;

                if (correctNodeDistance)
                {
                    graph.Nodes[i].AddNeighbor(graph.Nodes[j], nodeDistance);
                }
            }
        }
    }

    private static void TriggerEdgeRebuild()
    {
        OnRebuildEdges?.Invoke();
    }

    private static void TriggerRebuildEvent()
    {
        OnRebuildGraph?.Invoke();
    }

    #endregion

    #region Public Methods

    [ContextMenu("Build Graph")]
    public void BuildGraph()
    {
        // add nodes (all waypoints, including start and end) to graph
        var waypoints = FindObjectsOfType<Waypoint>();
        graph = new Graph<Waypoint>();

        Build(waypoints);
    }

    public static void RebuildGraph()
    {
        TriggerRebuildEvent();
        TriggerEdgeRebuild();
    }

    #endregion
}