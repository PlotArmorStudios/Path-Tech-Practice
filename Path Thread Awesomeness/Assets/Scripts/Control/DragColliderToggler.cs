using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragColliderToggler : MonoBehaviour
{
    [SerializeField] private GameObject _visual;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void ToggleCollider()
    {
        _collider.enabled = !_collider.enabled;
        _visual.SetActive(_collider.enabled);
    }
}
