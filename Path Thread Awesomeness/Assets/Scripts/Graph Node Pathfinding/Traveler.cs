using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

/// <summary>
/// Object that travels along graph nodes.
/// </summary>
public class Traveler : MonoBehaviour
{
    #region Fields

    [SerializeField] private Waypoint _start;
    [SerializeField] private Waypoint _end;
    [SerializeField] private int _currentListNodeID;

    // needed for the PathLength property
    private float _pathLength;

    private NavMeshAgent _navAgent;

    private Graph<Waypoint> _graph;
    private LinkedList<Waypoint> _path;
    private LinkedListNode<Waypoint> _target;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the length of the final path
    /// 
    /// </summary>
    public float PathLength => _pathLength;

    #endregion

    #region Unity Methods

    public void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _graph = GraphBuilder.Graph;
    }

    /// <summary>
    /// Updates traveler's target on collision.
    /// 
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        var waypoint = other.GetComponent<Waypoint>();

        if (waypoint == null || _target == null)
            return;

        if (waypoint == _target.Value)
        {
            UpdateTarget();
        }
    }

    #endregion
    
    #region Private methods

    private Waypoint FindNearestWaypoint()
    {
        var nearestDistance = float.MaxValue;
        Waypoint nearestWaypoint = null;

        foreach (var graphNode in _graph.Nodes)
        {
            float distanceToWaypoint = Vector3.Distance(graphNode.Value.transform.position, transform.position);

            if (distanceToWaypoint < nearestDistance)
            {
                nearestDistance = distanceToWaypoint;

                nearestWaypoint = graphNode.Value;
            }
        }

        return nearestWaypoint;
    }

    /// <summary>
    /// Builds a waypoint path from the start node to the given end node
    /// Side Effect: sets the pathLength field
    /// 
    /// </summary>
    /// <returns>waypoint path</returns>
    /// <param name="endNode">end node</param>
    private LinkedList<Waypoint> BuildWaypointPath(SearchNode<Waypoint> endNode)
    {
        LinkedList<Waypoint> path = new LinkedList<Waypoint>();

        path.AddFirst(endNode.GraphNode.Value);
        _pathLength = endNode.Distance;

        SearchNode<Waypoint> previous = endNode.Previous;

        while (previous != null)
        {
            path.AddFirst(previous.GraphNode.Value);
            previous = previous.Previous;
        }

        return path;
    }

    private string ConvertSearchListToString(SortedLinkedList<SearchNode<Waypoint>> searchList)
    {
        StringBuilder pathString = new StringBuilder();
        LinkedListNode<SearchNode<Waypoint>> currentNode = searchList.First;

        while (currentNode != null)
        {
            pathString.Append("[");
            pathString.Append(currentNode.Value.GraphNode.Value.ID + " ");
            pathString.Append(currentNode.Value.Distance + "] ");
            currentNode = currentNode.Next;
        }

        return pathString.ToString();
    }

    #endregion
    
    #region Public methods

    [ContextMenu("Test path")]
    public async void TestPathAsync()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        SetStartAndEndPoints();

        // Calculate new search path
        await Task.Run(() =>
        {
            _graph = GraphBuilder.Graph;
            _path = Search(_start, _end, _graph);
            _target = _path.First;
        });

        // To account for when entity is already at the start node to find a new path
        bool atStartNode = Vector3.Distance(transform.position, _start.transform.position) < 1f;

        if (atStartNode)
        {
            UpdateTarget();
        }
        else
        {
            _currentListNodeID = _target.Value.ID;
            _navAgent.SetDestination(_target.Value.transform.position);
        }

        watch.Stop();
        var elapsedTime = watch.ElapsedMilliseconds;
        Debug.Log($"Async task took {elapsedTime} ms.");
    }

    private void SetStartAndEndPoints()
    {
        _start = FindNearestWaypoint();

        //Find random end point
        var lastEndPoint = _end;

        do
        {
            var randomEndPoint = Random.Range(0, _graph.Nodes.Count);
            _end = _graph.Nodes[randomEndPoint].Value;
        } while (_start == _end || _end == lastEndPoint);
    }

    public void UpdateTarget()
    {
        _target = _target.Next;

        if (_target != null)
        {
            _currentListNodeID = _target.Value.ID;
            _navAgent.SetDestination(_target.Value.transform.position);
        }
    }

    /// <summary>
    /// Does a search for a path from start to end on graph
    /// </summary>
    /// <param name="start">start value</param>
    /// <param name="finish">finish value</param>
    /// <param name="graph">graph to search</param>
    /// <returns>string for path or empty string if there is no path</returns>
    public LinkedList<Waypoint> Search(Waypoint start, Waypoint end,
        Graph<Waypoint> graph)
    {
        // Create a search list a sorted linked list of search nodes to sync waypoints with the graph's graph nodes
        SortedLinkedList<SearchNode<Waypoint>> waypoints = new SortedLinkedList<SearchNode<Waypoint>>();
        Dictionary<GraphNode<Waypoint>, SearchNode<Waypoint>> searchNodes =
            new Dictionary<GraphNode<Waypoint>, SearchNode<Waypoint>>();

        var startNode = graph.Find(start);
        var endNode = graph.Find(end);

        // Create a search node for each graph node 
        foreach (var graphNode in graph.Nodes)
        {
            SearchNode<Waypoint> searchNode = new SearchNode<Waypoint>(graphNode);

            // If the graph node is the start node, set the distance for the search node to 0
            if (graphNode == startNode)
            {
                searchNode.Distance = 0;
            }

            waypoints.Add(searchNode);
            searchNodes.Add(graphNode, searchNode);
        }

        // Generate search list
        while (waypoints.Count > 0)
        {
            var currentSearchNode = waypoints.First.Value;

            waypoints.RemoveFirst();
            var currentGraphNode = currentSearchNode.GraphNode;
            waypoints.Remove(currentSearchNode);

            if (currentGraphNode == endNode)
            {
                return BuildWaypointPath(currentSearchNode);
            }


            // Calculate search node distances
            foreach (var neighbor in currentGraphNode.Neighbors)
            {
                if (searchNodes.ContainsKey(neighbor))
                {
                    var currentGraphNodeDistance =
                        currentSearchNode.Distance + currentGraphNode.GetEdgeWeight(neighbor);

                    SearchNode<Waypoint> neighborSearchNode = searchNodes[neighbor];

                    if (currentGraphNodeDistance < neighborSearchNode.Distance)
                    {
                        neighborSearchNode.Distance = currentGraphNodeDistance;
                        neighborSearchNode.Previous = currentSearchNode;

                        // Organize neighbor search nodes
                        waypoints.Reposition(neighborSearchNode);
                    }
                }
            }
        }

        // didn't find a path from start to end nodes
        return null;
    }

    #endregion

  
}