using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Score Manager script for game</summary>
/// <remarks>
/// <Author>Anil Punjabi</Author>
/// Attach to game object in scene. 
/// This is a singleton so only one instance can be active at a time.
/// </remarks>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;  // Static instance for singleton
	
	public int coinsCollected = 0;
	public int bulletsCollected = 0;
	public int playerHealth = 5;
	public int maxPlayerHealth = 5;
	
	
    void Awake() {
        // Ensure only one instance is running
        if (instance == null)
            instance = this; // Set instance to this object
        else
            Destroy(gameObject); // Kill yo self
    }
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void incrementScore(string objectTag)
	{
		switch (objectTag)
		{
	         case "Coin":
	             coinsCollected++;
	             break;
	          case "Bullet":
	             bulletsCollected++;
	             break;
   	          case "PlayerHealth":
			  	 if (playerHealth < maxPlayerHealth)
   	             	   playerHealth++;
   	             break;
		}
	 }
	 
 	public void decrementScore(string objectTag)
 	{
 	 	switch (objectTag)
 	    {
 	         case "Coin":
 	             coinsCollected--;
 	             break;
 	          case "Bullet":
 	             bulletsCollected--;
 	             break;
      	      case "PlayerHealth":
				 if (playerHealth > 0)
	      	         playerHealth--;
      	         break;
		}
	}

	    // /// <summary>Notify this manager of a change in score</summary>
	    // public void UpdatedUI () {
	    //     field.text = SumScore.Score.ToString("0"); // Post new score to text field
	    // }
	    //
	    // /// <summary>Notify this manager of a change in high score</summary>
	    // public void UpdatedHS () {
	    //     if(storeHighScore)
	    //         highScoreField.text = SumScore.HighScore.ToString("0"); // Post new high score to text field
	    // }


}
