
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Camera m_Camera;
    private Rigidbody m_RB;
    private PlayerMovement m_PlayerMovement;


    private Animator m_Animator;


    private void Awake()
    {
        m_Camera = FindObjectOfType<Camera>();
        m_RB = GetComponent<Rigidbody>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_PlayerMovement.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        m_PlayerMovement.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath() => StartCoroutine(Death());

    private IEnumerator Death()
    {
        m_Animator.enabled = false;
        m_Camera.transform.SetParent(null);

        Vector3 positionOut = m_Camera.transform.position - Vector3.forward * 3f;
        Quaternion rotationOut = Quaternion.LookRotation(transform.forward, m_Camera.transform.up);
        Vector3 vel = default;

        m_RB.AddForce(transform.forward * 10f + transform.up * 10f, ForceMode.VelocityChange);
        while (enabled)
        {
            var pos = Vector3.SmoothDamp(m_Camera.transform.position, positionOut, ref vel, 2f * Time.deltaTime);
            var rot = Quaternion.Slerp(m_Camera.transform.rotation, rotationOut, 2f * Time.deltaTime);
            m_Camera.transform.SetPositionAndRotation(pos, rot);
            yield return null;
        }
    }
}