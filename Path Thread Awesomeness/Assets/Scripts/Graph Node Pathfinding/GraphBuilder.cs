using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphBuilder : MonoBehaviour
{
    #region Field

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

    /// <summary>
    /// Awake is called before Start
    ///
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

    #endregion

    #region Private Methods

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

    #endregion
    
    #region Public Methods

    [ContextMenu("Build Graph")]
    public async void BuildGraph()
    {
        // add nodes (all waypoints, including start and end) to graph
        var waypoints = FindObjectsOfType<Waypoint>();
        graph = new Graph<Waypoint>();

        await Build(waypoints);
    }

    #endregion
}