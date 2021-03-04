using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastPosition ;
    private double totalDistance ;
	public Transform playerTransform;
	public int playerScore;
    [SerializeField] Text scoreText;
	
	
    void Start()
    {
        lastPosition = playerTransform.position ;
    }

    // Update is called once per frame
    void Update()
    {
		StartCoroutine("MyDelayedScoreCalculator");
	}

	IEnumerator MyDelayedScoreCalculator()
	{
	    yield return new WaitForSeconds(1.0f);
		float distance = Vector3.Distance(lastPosition, playerTransform.position);
		totalDistance += distance ;
		lastPosition = playerTransform.position ;
		playerScore = (int) totalDistance;
		scoreText.text = "SCORE: " + playerScore;

		//Debug.Log("Total distance travelled:" + totalDistance ) ;

	}
}
