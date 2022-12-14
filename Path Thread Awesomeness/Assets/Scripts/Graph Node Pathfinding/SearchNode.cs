using System;

/// <summary>
/// A node for searching using Dijkstra's algorithm
/// </summary>
/// <typeparam>type for search node</typeparam>
public class SearchNode<T> : IComparable
{
    #region Fields

    private float _distance;
    GraphNode<T> _graphNode;
    SearchNode<T> _previous;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructs a new search node with the given graph node.
    /// Distance is set to the max float value and previous is
    /// set to null
    /// </summary>
    /// <param name="graphNode">graph node</param>
    public SearchNode(GraphNode<T> graphNode)
    {
        _graphNode = graphNode;
        _distance = float.MaxValue;
        _previous = null;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the graph node
    /// </summary>
    /// <value>graph node</value>
    public GraphNode<T> GraphNode
    {
        get { return _graphNode; }
    }

    /// <summary>
    /// Gets and sets the distance for the node
    /// </summary>
    /// <value>distance</value>
    public float Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }

    /// <summary>
    /// Gets and sets the previous node for the
    /// path to the graph node
    /// </summary>
    /// <value>previous</value>
    public SearchNode<T> Previous
    {
        get { return _previous; }
        set { _previous = value; }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Compares this instance to the provided object and
    /// returns their relative order
    /// </summary>
    /// <returns>relative order</returns>
    /// <param name="obj">object to compare to</param>
    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        // check for correct object type
        SearchNode<T> otherSearchNode = obj as SearchNode<T>;
        if (otherSearchNode != null)
        {
            if (_distance < otherSearchNode.Distance)
            {
                return -1;
            }
            else if (_distance == otherSearchNode.Distance)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            throw new ArgumentException("Object is not a SearchNode");
        }        
    }

    public override string ToString()
    {
        return _distance.ToString();
    }
	
    #endregion
}
