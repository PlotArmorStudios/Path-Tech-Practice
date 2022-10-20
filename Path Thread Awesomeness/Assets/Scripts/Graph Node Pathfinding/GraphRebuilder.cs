using System;
using System.Threading.Tasks;
using UnityEngine;

public class GraphRebuilder : MonoBehaviour
{
    public static event Action OnRebuildGraph;
    public static event Action OnRebuildEdges;

    public static async void RebuildGraph()
    {
        await TriggerRebuildEvent();
        await TriggerEdgeRebuild();
    }

    private static async Task TriggerEdgeRebuild()
    {
        OnRebuildEdges?.Invoke();
    }

    private static async Task TriggerRebuildEvent()
    {
        OnRebuildGraph?.Invoke();
    }
    
    
}