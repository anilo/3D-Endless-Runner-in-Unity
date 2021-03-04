using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WGJ.Rooms
{
    public class ObstacleSpawner : MonoBehaviour
    {
        private static event Action<ObstacleSpawner, Obstacle> s_OnObstacleSpawned;
        private static Transform s_Container; //i hate statics but this will do for a game jam

        [SerializeField] private float m_FirstObstaclePosition;
        [SerializeField] private float m_InitialRange;
        
        [SerializeField] private int m_MinDistanceBetweenObstacles;
        [SerializeField] private int m_MaxDistanceBetweenObstacles;

        [SerializeField] private Obstacle[] m_Obstacles;

        private Obstacle m_NextToSpawn;

        private PlayerMovement m_PlayerMovement;
        private float m_NextObstacle;
        private float m_DistanceToNextObstacle;
        private float m_LastPlayerZ;
        private Transform m_Player => m_PlayerMovement.transform;
        
        private IDifficultyCurve m_DifficultyCurve;

        private void Awake()
        {
            m_DifficultyCurve = GameManager.inst.GetComponentInChildren<IDifficultyCurve>();

            InitialiseContainer();
            ConfigurePlayer();


            for (float t = m_FirstObstaclePosition;
                t <= m_FirstObstaclePosition + m_InitialRange + m_MaxDistanceBetweenObstacles;
                t += CalculateDistanceToNext())
            {
                CreateObstacle(t);
                m_NextObstacle = t;
            }
            m_DistanceToNextObstacle = CalculateDistanceToNext();
            m_NextObstacle += m_DistanceToNextObstacle;
            m_LastPlayerZ = m_Player.position.z;
        }

        private void ConfigurePlayer()
        {
            m_PlayerMovement = FindObjectOfType<PlayerMovement>();
        }

        

        private void InitialiseContainer()
        {
            if (!s_Container)
            {
                var go = new GameObject("__obstaclePool");
                go.transform.SetParent(transform.parent);
                go.SetActive(false);
                s_Container = go.transform;
            }
        }

        private void OnEnable()
        {
            s_OnObstacleSpawned += HandleObstacleSpawned;
        }

        private void OnDisable()
        {
            s_OnObstacleSpawned -= HandleObstacleSpawned;
        }

        private void HandleObstacleSpawned(ObstacleSpawner spawnedBy, Obstacle spawnedObstacle)
        {
            if (spawnedBy == this) return; //ignored spawned by self
            AdjustSpawnIfOverlapping(spawnedObstacle);
        }

        private void AdjustSpawnIfOverlapping(Obstacle spawnedObstacle)
        {
            if (spawnedObstacle.Bounds.Intersects(m_NextToSpawn.Bounds))
            {
                m_DistanceToNextObstacle += spawnedObstacle.Bounds.extents.z * 2.0f;
            }
        }

        private void CreateObstacle(float position)
        {
            Obstacle obstacle = GetNextObstacle();

            m_NextToSpawn = CreateRandomInstance();
            m_NextToSpawn.transform.SetParent(s_Container);

            obstacle.transform.SetParent(null);
            obstacle.Spawn(position, m_Player);
            s_OnObstacleSpawned?.Invoke(this, obstacle);
        }

        private Obstacle GetNextObstacle()
        {
            Obstacle obstacle;
            if (m_NextToSpawn)
            {
                obstacle = m_NextToSpawn;
            }
            else
            {
                obstacle = CreateRandomInstance();
            }

            return obstacle;
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

        private Obstacle CreateRandomInstance() => Instantiate(m_Obstacles[Random.Range(0, m_Obstacles.Length)]);
        private float CalculateDistanceToNext() => 
            Mathf.Round(
                Random.Range(m_MinDistanceBetweenObstacles, m_MaxDistanceBetweenObstacles)
                / m_DifficultyCurve.DifficultyMultiplier)
                ;
    }
}
