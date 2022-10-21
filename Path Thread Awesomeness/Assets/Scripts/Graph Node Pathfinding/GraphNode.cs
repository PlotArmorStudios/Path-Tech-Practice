using System;
using System.Collections.Generic;

/// <summary>
/// A graph node
/// </summary>
/// <typeparam name="T">type of value stored in node</typeparam>
public class GraphNode<T>
{
    #region Fields

    private T _value;
    private List<GraphNode<T>> _neighbors;
    private List<float> _weights;

    #endregion

    #region Constructors

    public GraphNode(T value)
    {
        _value = value;
        _neighbors = new List<GraphNode<T>>();
        _weights = new List<float>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value stored in the node
    /// 
    /// </summary>
    public T Value => _value;

    /// <summary>
    /// Gets a read-only list of the neighbors of the node
    /// 
    /// </summary>
    public IList<GraphNode<T>> Neighbors => _neighbors.AsReadOnly();

    #endregion

    #region Public Methods

    public bool AddNeighbor(GraphNode<T> neighbor, float weight)
    {
        // don't add duplicate nodes
        if (_neighbors.Contains(neighbor))
        {
            return false;
        }
        else
        {
            _neighbors.Add(neighbor);
            _weights.Add(weight);
            return true;
        }
    }

    /// <summary>
    /// Gets the weight of the edge from this node to
    /// the given neighbor. If the edge doesn't exist,
    /// throws an InvalidOperationException
    /// 
    /// </summary>
    /// <param name="neighbor">neighbor</param>
    /// <returns>weight of edge to neighbor</returns>
    public float GetEdgeWeight(GraphNode<T> neighbor)
    {
        // make sure edge exists
        if (!_neighbors.Contains(neighbor))
        {
            throw new InvalidOperationException("Trying to retrieve weight of non-existent edge");
        }
        else
        {
            int index = _neighbors.IndexOf(neighbor);
            return _weights[index];
        }
    }

    public bool RemoveNeighbor(GraphNode<T> neighbor)
    {
        // remove weight for neighbor
        int index = _neighbors.IndexOf(neighbor);
        if (index == -1)
        {
            // neighbor not in list
            return false;
        }
        else
        {
            // remove neighbor and edge weight
            _neighbors.RemoveAt(index);
            _weights.RemoveAt(index);
            return true;
        }
    }

    public bool RemoveAllNeighbors()
    {
        for (int i = _neighbors.Count - 1; i >= 0; i--)
        {
            _neighbors.RemoveAt(i);
        }
        _weights.Clear();
        return true;
    }

    #endregion
}
