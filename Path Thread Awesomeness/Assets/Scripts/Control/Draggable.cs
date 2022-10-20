using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    float _mouseZCoordinate;

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _layerMask);

        return raycastHit.point;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        GraphRebuilder.RebuildGraph();
    }
}