using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {


    public float speed = 5;
    [SerializeField] float horizontalMultiplier = 2;
    public float speedIncreasePerPoint = 0.1f;

    private float horizontalInput;
    private bool m_alive = true;
    private Vector2 m_currentLane;
    private Vector3 m_currentVel;
    public float rotSpeed = 5f;

    private void Start()
    {
        m_currentLane = new Vector2(2.5f, 0);
    }


    private void Update()
    {
        if (transform.position.y < -5) Die();
        if (!m_alive) return;

        // Forward Movement
        Vector3 forwardMove = Vector3.forward * speed * Time.fixedDeltaTime;
        transform.position = transform.position + forwardMove;

        if (Input.GetKeyDown(KeyCode.A))
            SwitchLanes(-1);
        if (Input.GetKeyDown(KeyCode.D))
            SwitchLanes(1);

        Vector3 targetpos = new Vector3(m_currentLane.x, m_currentLane.y, transform.position.z);
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetpos, ref m_currentVel, 0.1f);
        transform.position = smoothedPos;

        //Rotate Player based on his current lane
        RotatePlayer();
    }

    private void SwitchLanes(int input)
    {
        if (input == 1f)
        {
            if (m_currentLane.x == -2.5f && m_currentLane.y == 0) m_currentLane = new Vector2(2.5f, 0);
            else if (m_currentLane.x == 2.5f && m_currentLane.y == 0) m_currentLane = new Vector2(5f, 2.5f);
            else if (m_currentLane.x == 5f && m_currentLane.y == 2.5f) m_currentLane = new Vector2(5f, 7.5f);
            else if (m_currentLane.x == 5f && m_currentLane.y == 7.5f) m_currentLane = new Vector2(2.5f, 10f);
            else if (m_currentLane.x == 2.5f && m_currentLane.y == 10f) m_currentLane = new Vector2(-2.5f, 10f);
            else if (m_currentLane.x == -2.5f && m_currentLane.y == 10f) m_currentLane = new Vector2(-5f, 7.5f);
            else if (m_currentLane.x == -5f && m_currentLane.y == 7.5f) m_currentLane = new Vector2(-5f, 2.5f);
            else if (m_currentLane.x == -5f && m_currentLane.y == 2.5f) m_currentLane = new Vector2(-2.5f, 0f);
        }
        else if (input == -1f)
        {
            if (m_currentLane.x == -2.5f && m_currentLane.y == 0) m_currentLane = new Vector2(-5f, 2.5f);
            else if (m_currentLane.x == 2.5f && m_currentLane.y == 0) m_currentLane = new Vector2(-2.5f, 0f);
            else if (m_currentLane.x == 5f && m_currentLane.y == 2.5f) m_currentLane = new Vector2(2.5f, 0f);
            else if (m_currentLane.x == 5f && m_currentLane.y == 7.5f) m_currentLane = new Vector2(5f, 2.5f);
            else if (m_currentLane.x == 2.5f && m_currentLane.y == 10f) m_currentLane = new Vector2(5f, 7.5f);
            else if (m_currentLane.x == -2.5f && m_currentLane.y == 10f) m_currentLane = new Vector2(2.5f, 10f);
            else if (m_currentLane.x == -5f && m_currentLane.y == 7.5f) m_currentLane = new Vector2(-2.5f, 10f);
            else if (m_currentLane.x == -5f && m_currentLane.y == 2.5f) m_currentLane = new Vector2(-5f, 7.5f);
        }
    }

    private void RotatePlayer()
    {
        float rotAngle = 0f;

        if (m_currentLane.y == 0f) rotAngle = 0;
        else if (m_currentLane.x == 5f) rotAngle = 90f;
        else if (m_currentLane.y == 10f) rotAngle = -180f;
        else if (m_currentLane.x == -5f) rotAngle = -90f;

        Quaternion targetRot = Quaternion.Euler(0, 0, rotAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
    }

    public void Die()
    {
        m_alive = false;
        // Restart the game
        Invoke("Restart", 2);
    }

    void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}