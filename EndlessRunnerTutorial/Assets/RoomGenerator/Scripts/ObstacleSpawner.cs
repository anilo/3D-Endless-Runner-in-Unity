using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WGJ.Rooms
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private float m_FirstObstaclePosition;
        [SerializeField] private float m_InitialRange;
        
        [SerializeField] private int m_MinDistanceBetweenObstacles;
        [SerializeField] private int m_MaxDistanceBetweenObstacles;
        [SerializeField] private Obstacle[] m_Obstacles;
        [SerializeField] private Transform m_Player;

        [SerializeField] private RoomTicks m_RoomTicks;

        private float m_NextObstacle;
        private float m_DistanceToNextObstacle;
        private float m_LastPlayerZ;

        //private void OnEnable()
        //{
        //    m_RoomTicks.Tick += HandleTick;
        //}

        //private void HandleTick()
        //{

        //}

        private void Awake()
        {
            for (float t = m_FirstObstaclePosition; t <= m_FirstObstaclePosition + m_InitialRange + m_MaxDistanceBetweenObstacles; t += CalculateDistanceToNext())
            {
                CreateObstacle(t);
                m_NextObstacle = t;
            }
            m_DistanceToNextObstacle = CalculateDistanceToNext();
            m_NextObstacle += m_DistanceToNextObstacle;
            m_LastPlayerZ = m_Player.position.z;
        }

        private void CreateObstacle(float position)
        {
            var obstacle = Instantiate(m_Obstacles[Random.Range(0, m_Obstacles.Length)]);
            obstacle.Spawn(position, m_Player);
        }

        private int CalculateDistanceToNext()
        {
            return Random.Range(m_MinDistanceBetweenObstacles, m_MaxDistanceBetweenObstacles);
        }

        private void Update()
        {
            var diff = m_Player.position.z - m_LastPlayerZ;
            m_DistanceToNextObstacle -= diff;

            if (m_DistanceToNextObstacle < 0)
            {
                m_DistanceToNextObstacle = CalculateDistanceToNext();

                CreateObstacle(m_NextObstacle);

                m_NextObstacle += m_DistanceToNextObstacle;

            }
            m_LastPlayerZ = m_Player.position.z;

        }
    }
}