using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WGJ.Rooms
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float[] m_AvailablePositions;

        private Collider m_Collider;
        public Bounds Bounds => m_Collider.bounds;

        Transform m_Player;

        private void Awake()
        {
            m_Collider = GetComponentInChildren<Collider>();
        }

        public void Spawn(float offset, Transform player)
        {
            m_Player = player;
            transform.position = transform.forward * offset +transform.right * m_AvailablePositions[Random.Range(0, m_AvailablePositions.Length)];
        }

        private void Update()
        {
            if (m_Player) {
                var playerOffset = transform.position - m_Player.position;
                if (Vector3.Dot(transform.forward, playerOffset) < -10.0f)
                {
                    Destroy(gameObject); //TODO use object pool
                }
            }
        }
    }
}