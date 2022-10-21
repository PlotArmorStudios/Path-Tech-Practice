using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws all the edges in a graph
/// </summary>
public class EdgeRenderer : MonoBehaviour
{
    #region Fields

    private static List<GameObject> _lineRenderers;

    #endregion

    #region Unity Methods

    private void Start()
    {
        DrawLines();
    }

    private void OnEnable()
    {
        GraphBuilder.OnRebuildEdges += RedrawLines;
    }

    private void OnDisable()
    {
        GraphBuilder.OnRebuildEdges -= RedrawLines;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Build line visuals based on the current graph.
    /// 
    /// </summary>
    /// <param name="graph">the current graph. </param>
    private void BuildLines(Graph<Waypoint> graph)
    {
        foreach (GraphNode<Waypoint> node in graph.Nodes)
        {
            foreach (GraphNode<Waypoint> neighbor in node.Neighbors)
            {
                // add line renderer and draw line
                GameObject lineObj = new GameObject("LineObj");
                LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
                _lineRenderers.Add(lineObj);

                //Set color
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;

                //Set width
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;

                //Set line count which is 2
                lineRenderer.positionCount = 2;

                //Set the postion of both two lines
                lineRenderer.SetPosition(0, node.Value.transform.position);
                lineRenderer.SetPosition(1, neighbor.Value.transform.position);
            }
        }
    }

    #endregion

    #region Public Methods

    public void DrawLines()
    {
        // add a line renderer for each graph edge
        _lineRenderers = new List<GameObject>();
        Graph<Waypoint> graph = GraphBuilder.Graph;

        BuildLines(graph);
    }

    public void ClearLines()
    {
        for (int i = _lineRenderers.Count - 1; i >= 0; i--)
        {
            Destroy(_lineRenderers[i]);
        }
    }

    [ContextMenu("Redraw Lines")]
    public void RedrawLines()
    {
        ClearLines();
        DrawLines();
    }

    #endregion
}