using System;
using UnityEngine;

public class DifficultyTester : MonoBehaviour
{
#pragma warning disable 414
    [Header("Current")]
    [SerializeField] private float m_CurrentSpeed;
    [SerializeField] private float m_CurrentDifficulty;

    [Header("Manual")]
    [SerializeField] private float m_DebugTime;
    [SerializeField] private float m_DebugSpeed;
    [SerializeField] private float m_DebugDifficulty;
    [NonSerialized] private GameplayDifficulty m_Target;

#pragma warning restore 414
#if UNITY_EDITOR

    private void Update()
    {
        if (m_Target)
        {
            m_CurrentDifficulty = m_Target.DifficultyMultiplier;
            m_CurrentSpeed = m_Target.CurrentSpeed;

            m_DebugDifficulty = m_Target.EvaluateDifficulty(m_DebugTime);
            m_DebugSpeed = m_Target.EvaluateSpeed(m_DebugTime);
        } 
        else
        {
            m_Target = GetComponent<GameplayDifficulty>();
        }
    }
#endif
}