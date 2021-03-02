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

        private PlayerMovement m_Player;
        private Collider m_PlayerCollider;

        private void Awake()
        {
            m_Collider = GetComponentInChildren<Collider>();
        }

        public void Spawn(float offset, Transform player)
        {
            m_Player = player.GetComponentInChildren<PlayerMovement>();
            m_PlayerCollider = m_Player.GetComponentInChildren<Collider>();
            transform.position = transform.forward * offset +transform.right * m_AvailablePositions[Random.Range(0, m_AvailablePositions.Length)];
        }

        private void FixedUpdate()
        {
            CheckPlayerCollision();
        }

        private void Update()
        {
            CheckIfPassed();
        }



        private void CheckPlayerCollision()
        {
            if (m_PlayerCollider)
            {
                if (Bounds.Intersects(m_PlayerCollider.bounds))
                {
                    m_Player.Die();
                    enabled = false;
                }
            }
        }

        private void CheckIfPassed()
        {
            if (m_Player)
            {
                var playerOffset = transform.position - m_Player.transform.position;
                if (Vector3.Dot(transform.forward, playerOffset) < -10.0f)
                {
                    Destroy(gameObject); //TODO use object pool
                }
            }
        }
    }
}