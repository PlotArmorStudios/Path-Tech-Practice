using System.Collections.Generic;

public class Graph<T>
{
    #region Fields

    List<GraphNode<T>> nodes = new List<GraphNode<T>>();

    #endregion

    #region Properties

    public int Count => nodes.Count;

    public IList<GraphNode<T>> Nodes => nodes.AsReadOnly();

    #endregion

    #region Public Methods

    public void Clear()
    {
        // remove all the neighbors from each node
        // so nodes can be garbage collected
        foreach (GraphNode<T> node in nodes)
        {
            node.RemoveAllNeighbors();
        }

        // now remove all the nodes from the graph
        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            nodes.RemoveAt(i);
        }
    }

    public bool AddNode(T value)
    {
        if (Find(value) != null)
        {
            // duplicate value
            return false;
        }
        else
        {
            nodes.Add(new GraphNode<T>(value));
            return true;
        }
    }

    public bool RemoveNode(T value)
    {
        GraphNode<T> removeNode = Find(value);
        if (removeNode == null)
        {
            return false;
        }
        else
        {
            // need to remove as neighor for all nodes
            // in graph
            nodes.Remove(removeNode);
            foreach (GraphNode<T> node in nodes)
            {
                node.RemoveNeighbor(removeNode);
            }
            return true;
        }
    }

    public bool RemoveEdge(T value1, T value2)
    {
        GraphNode<T> node1 = Find(value1);
        GraphNode<T> node2 = Find(value2);
        if (node1 == null ||
            node2 == null)
        {
            return false;
        }
        else if (!node1.Neighbors.Contains(node2))
        {
            // edge doesn't exist
            return false;
        }
        else
        {
            // undirected graph, so remove as neighbors to each other
            node1.RemoveNeighbor(node2);
            node2.RemoveNeighbor(node1);
            return true;
        }
    }

    public GraphNode<T> Find(T value)
    {
        foreach (GraphNode<T> node in nodes)
        {
            if (node.Value.Equals(value))
            {
                return node;
            }
        }
        return null;
    }

    #endregion
}