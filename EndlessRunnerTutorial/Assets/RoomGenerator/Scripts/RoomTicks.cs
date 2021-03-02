using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTicks : MonoBehaviour
{
    [SerializeField] private float m_TickInterval;
    [SerializeField] private Transform m_Player;

    public event Action Tick;

    private float m_Distance;
    private float m_PreviousDistance;

    private void Start()
    {
        m_PreviousDistance = m_Player.position.z;
        m_Distance = 0;
    }

    private void Update()
    {
        m_Distance += (m_Player.position.z - m_PreviousDistance);
        m_PreviousDistance = m_Player.position.z;

        if (m_Distance > m_TickInterval)
        {
            m_Distance = m_Distance % m_TickInterval;
            Tick?.Invoke();
            Debug.Log("TICK");
        }
    }
}
