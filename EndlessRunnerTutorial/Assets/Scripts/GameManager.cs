using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(int.MinValue)] //pls never do this in bigger project
public class GameManager : MonoBehaviour {

    public static GameManager inst;

    [SerializeField] PlayerMovement playerMovement;

    private IDifficultyCurve m_Difficulty;

    private void Awake ()
    {
        inst = this;
        m_Difficulty = GetComponentInChildren<IDifficultyCurve>();
    }

    private void Start () {

	}

	private void Update () {
        playerMovement.speed = m_Difficulty.CurrentSpeed;
	}
}