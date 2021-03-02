using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WGJ.Rooms
{
    public class PlayerKiller : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision) //TODO using template player movement 
        {
            if (collision.rigidbody.gameObject.GetComponent<PlayerMovement>() is PlayerMovement player)
            {
                // Kill the player
                player.Die();
            }
        }
    }
}