using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {


    public float speed = 5;
    [SerializeField] float horizontalMultiplier = 2;
    public float speedIncreasePerPoint = 0.1f;
    public AnimationCurve jumpTrajectory;

    private float horizontalInput;
    private bool m_alive = true;
    private Vector2 m_currentLane;
    private int m_currentLaneIndex;
    private Vector3 m_currentVel;
    public float rotSpeed = 5f;
    private float timePeriod;
    private float timeElapsed;
    public bool isJumping;
    private float jumpTime = 1.5f;

    private void Start()
    {
        m_currentLane = new Vector2(-2, 0);
        m_currentLaneIndex = 0;
    }


    private void Update()
    {
        if (transform.position.y < -5) Die();
        if (!m_alive) return;

        // Forward Movement
        Vector3 forwardMove = Vector3.forward * speed * Time.deltaTime;
        transform.position = transform.position + forwardMove;

        if (Input.GetKeyDown(KeyCode.A))
            SwitchLanes(-1);
        if (Input.GetKeyDown(KeyCode.D))
            SwitchLanes(1);

        Vector3 targetpos = new Vector3(m_currentLane.x, m_currentLane.y, transform.position.z);

        //Rotate Player based on his current lane
        RotatePlayer();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            timeElapsed = 0f;
            timePeriod = 1f;
        }
        if (isJumping)
        {
            if (timeElapsed < timePeriod)
            {
                float t = (timeElapsed / timePeriod);
                float y = jumpTrajectory.Evaluate(t);
                //Change the targetpos here for jumping
                targetpos = targetpos + transform.up * y;

                timeElapsed += Time.deltaTime * jumpTime;
            }
            else
                isJumping = false;

            if (timeElapsed > 0.1f && timeElapsed < 0.3f && Input.GetKeyDown(KeyCode.Space))
            {
                timeElapsed = timePeriod - 0.1f;
                SwitchLanes(69);
            }
        }

        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetpos, ref m_currentVel, 0.1f);
        transform.position = smoothedPos;
    }


    Vector2[] Lanes =
    {
        new Vector2(-2, 0), 
        new Vector2(2, 0),  
        new Vector2(6, 4),  
        new Vector2(6, 8),  
        new Vector2(2, 12), 
        new Vector2(-2, 12),
        new Vector2(-6, 8), 
        new Vector2(-6, 4)  
    };

    //        -2,12    2,12
    // -6,8                   6,8
    // -6,4                   6,4
    //        -2,0     2,0

    // Opposite Lanes
    // 0 - 5
    // 1 - 4
    // 2 - 7
    // 3 - 6
    // 4 - 1
    // 5 - 0
    // 6 - 3
    // 7 - 2            
    private void SwitchLanes(int input)
    {
        var nextIndex = m_currentLaneIndex + 1;
        var prevIndex = m_currentLaneIndex - 1;
        var oppIndex = m_currentLaneIndex;

        if (m_currentLaneIndex == 7)
            nextIndex = 0;
        else if(m_currentLaneIndex == 0)
            prevIndex = 7;

        if (input == 1f)
        {
            m_currentLane = Lanes[nextIndex];
            m_currentLaneIndex = nextIndex;
        }
        else if (input == -1f)
        {
            m_currentLane = Lanes[prevIndex];
            m_currentLaneIndex = prevIndex;
        }
        else if(input == 69)
        {
            Debug.Log("Jump To Opposite Lane");
            if (m_currentLaneIndex == 1 || m_currentLaneIndex == 3) m_currentLaneIndex += 3;
            else if (m_currentLaneIndex == 0 || m_currentLaneIndex == 2) m_currentLaneIndex += 5;
            else if (m_currentLaneIndex == 4 || m_currentLaneIndex == 6) m_currentLaneIndex -= 3;
            else if (m_currentLaneIndex == 5 || m_currentLaneIndex == 7) m_currentLaneIndex -= 5;

            m_currentLane = Lanes[m_currentLaneIndex];
        }
    }

    private void RotatePlayer()
    {
        float rotAngle = 0f;

        if (m_currentLane.y == 0f) rotAngle = 0;
        else if (m_currentLane.x == 6f) rotAngle = 90f;
        else if (m_currentLane.y == 12f) rotAngle = -180f;
        else if (m_currentLane.x == -6f) rotAngle = -90f;

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