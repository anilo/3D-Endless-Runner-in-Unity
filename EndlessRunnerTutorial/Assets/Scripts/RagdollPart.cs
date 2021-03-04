using UnityEngine;

public class RagdollPart : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;
    private Rigidbody m_RB;

    private Vector3 m_PreviousPosition;
    private Vector3 m_PreviousForward;
    private Vector3 m_PreviousRight;
    private Vector3 m_PreviousUp;

    private Vector3 m_Torque;
    private Vector3 m_Velocity;

    private void Awake()
    {
        m_PlayerMovement = GetComponentInParent<PlayerMovement>();
        m_RB = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_PlayerMovement.OnPlayerDeath += HandlePlayerDeath;
        m_RB.isKinematic = true;
    }

    private void OnDisable()
    {
        m_PlayerMovement.OnPlayerDeath -= HandlePlayerDeath;
    }
    private void HandlePlayerDeath()
    {
        m_RB.isKinematic = false;

        m_RB.AddForce(m_Velocity, ForceMode.VelocityChange);
        m_RB.AddRelativeTorque(m_Torque, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        m_Velocity = (transform.position - m_PreviousPosition) / Time.fixedDeltaTime;

        float tX = Vector3.Angle(transform.right, m_PreviousRight);
        float tY = Vector3.Angle(transform.up, m_PreviousUp);
        float tZ = Vector3.Angle(transform.forward, m_PreviousForward);

        m_Torque = new Vector3(tX, tY, tZ) / Time.fixedDeltaTime;

        m_PreviousRight = transform.right;
        m_PreviousForward = transform.forward;
        m_PreviousUp = transform.up;
        m_PreviousPosition = transform.position;
    }
}