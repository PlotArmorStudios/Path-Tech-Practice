using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphBuilder : MonoBehaviour
{
    [SerializeField] private float _pathDetectionRange = 6;
    static Graph<Waypoint> graph;

    /// <summary>
    /// Awake is called before Start
    ///
    /// Note: Leave this method public to support automated grading
    /// </summary>
    public void Awake()
    {
        BuildGraph();
    }

    private void OnEnable()
    {
        GraphRebuilder.OnRebuildGraph += BuildGraph;
    }

    private void OnDisable()
    {
        GraphRebuilder.OnRebuildGraph -= BuildGraph;
    }

    [ContextMenu("Build Graph")]
    public async void BuildGraph()
    {
        // add nodes (all waypoints, including start and end) to graph
        var waypoints = FindObjectsOfType<Waypoint>();
        graph = new Graph<Waypoint>();

        await Build(waypoints);
    }

    private async Task Build(Waypoint[] waypoints)
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

    /// <summary>
    /// Gets and sets the graph
    /// 
    /// CAUTION: Set should only be used by the autograder
    /// </summary>
    /// <value>graph</value>
    public static Graph<Waypoint> Graph
    {
        get { return graph; }
        set { graph = value; }
    }
}