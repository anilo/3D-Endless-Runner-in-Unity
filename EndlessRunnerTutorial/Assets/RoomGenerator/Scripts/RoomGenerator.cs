using System;
using System.Collections.Generic;
using UnityEngine;

namespace WGJ.Rooms
{
    internal class RoomGenerator : MonoBehaviour
    {
        [Tooltip("Tiles to spawn in front of player")]
        [SerializeField][Min(1)] private int m_VisibleTiles;
        [Tooltip("Tiles behind player")]
        [SerializeField][Min(0)] private int m_ExtraTiles;
        [SerializeField] private float m_RoomLength;
        [SerializeField] private Transform m_Prototype;

        private Queue<Transform> m_Queue; //fifo

        private PlayerMovement m_Player;
        private Vector3 m_InitialPosition;
        private Vector3 m_PlayerPosition => m_Player.transform.position;

        private void Awake()
        {
            m_Player = FindObjectOfType<PlayerMovement>(); //TODO temporary player reference, should be refactored later
            InitialiseTiles();
        }

        private void InitialiseTiles()
        {
            m_Queue = new Queue<Transform>();
            for (int i = -m_ExtraTiles; i < m_VisibleTiles; i++)
            {
                var item = Instantiate(m_Prototype, transform);
                item.position = transform.forward * m_RoomLength * i;
                m_Queue.Enqueue(item);
            }
        }

        private void OnEnable()
        {
            m_InitialPosition = m_PlayerPosition;
            m_Player.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            m_Player.OnPlayerDeath -= HandlePlayerDeath;
        }

        private void Update()
        {
            var playerOffset = m_PlayerPosition - m_InitialPosition;
            if (Vector3.Dot(transform.forward, playerOffset) > m_RoomLength)
            {
                var item = m_Queue.Dequeue();
                m_Queue.Enqueue(item);
                item.transform.position += m_RoomLength * (m_VisibleTiles + m_ExtraTiles) * transform.forward;
                m_InitialPosition += m_RoomLength * transform.forward;
            }
        }

        private void HandlePlayerDeath() => enabled = false;
    }
}