using UnityEngine;

public class DifficultyTester : MonoBehaviour
{
    [Header("Current")]
    [SerializeField] private float m_CurrentSpeed;
    [SerializeField] private float m_CurrentDifficulty;

    [Header("Manual")]
    [SerializeField] private float m_DebugTime;
    [SerializeField] private float m_DebugSpeed;
    [SerializeField] private float m_DebugDifficulty;

    private GameplayDifficulty m_Target;

#if UNITY_EDITOR

    private void Awake()
    {
        m_Target = FindObjectOfType<GameplayDifficulty>(); //just for debug
    }

    private void Update()
    {
        if (m_Target)
        {
            m_CurrentDifficulty = m_Target.DifficultyMultiplier;
            m_CurrentSpeed = m_Target.SpeedMultiplier;

            m_DebugDifficulty = m_Target.EvaluateDifficulty(m_DebugTime);
            m_DebugSpeed = m_Target.EvaluateSpeed(m_DebugTime);
        }
    }
#endif
}