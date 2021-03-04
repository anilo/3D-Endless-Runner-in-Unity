using UnityEngine;

public class GameplayDifficulty : MonoBehaviour, IDifficultyCurve
{
    public float DifficultyMultiplier => EvaluateDifficulty(Time.timeSinceLevelLoad);
    public float CurrentSpeed => EvaluateSpeed(Time.timeSinceLevelLoad);

    [Header("Player movement")]
    [SerializeField] private float m_BaseSpeed;
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private AnimationCurve m_SpeedCurve;
    [SerializeField] private float m_TimeToMaxSpeed;

    [Header("Obstacles")]
    [SerializeField] private float m_DifficultyTick;
    [Tooltip("Speed of difficulty raising")]
    [SerializeField] private float m_RaiseFactor;
    [Tooltip("Lower values will create more drastic differences between spikes")]
    [SerializeField][Min(1)] private float m_SpikeAmplitudeFactor;
    [Tooltip("Higher values will result in more frequent difficulty spikes and no spikes for 0")]
    [SerializeField][Min(0)] private float m_SpikeFrequency;

    public float EvaluateSpeed(float time)
    {
        float t = Mathf.Clamp01(time / m_TimeToMaxSpeed);
        float curve = m_SpeedCurve.Evaluate(t);
        float tt = Mathf.Clamp01(curve);
        return Mathf.Lerp(m_BaseSpeed, m_MaxSpeed, tt);
    }

    /// difficulty curve based on this shape https://www.desmos.com/calculator/hnpcmxma0o
    public float EvaluateDifficulty(float time)
    {
        float min = 1.0f;
        if (time <= 0) return min;
        float t = time / m_DifficultyTick;
        float tt = t * t;
        float a = m_SpikeFrequency, b = m_RaiseFactor, c = m_SpikeAmplitudeFactor;
        
        float S = Mathf.Sin(tt * a);
        float B = tt * b;
        float A = S / (c * t);
        float d = min + A + B;

        return Mathf.Clamp(d, min, Mathf.Abs(d));
    }
}
