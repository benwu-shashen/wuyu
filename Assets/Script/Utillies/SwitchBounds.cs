using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
    }

    private void OnAfterSceneUnloadEvent()
    {
        SwitchConfinerShape();
    }

    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        
        confiner.m_BoundingShape2D = confinerShape;

        confiner.InvalidatePathCache();
    }
}
