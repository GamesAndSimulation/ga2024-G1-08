using System;
using UnityEngine;

[Serializable]
public class PortalTeleport : MonoBehaviour
{
    public Transform player;
    public Transform receiver;

    public bool mazeMode;
    public GameEvent changeMazeEvent;


    private bool enter;

    private void Start()
    {
        enter = false;
    }

    void Update()
    {
        if (enter)
        {
            Vector3 portalToPlayer = player.position - transform.position;

            Vector3 vel = player.gameObject.GetComponent<Rigidbody>().velocity;

            if ((vel + portalToPlayer).magnitude > portalToPlayer.magnitude)
            {

                float dotProduct = Vector3.Dot(transform.up, portalToPlayer);


                if (dotProduct < 0f)
                {
                    float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                    rotationDiff += 180;
                    player.Rotate(Vector3.up, rotationDiff);
                    Vector3 posOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;

                    player.position = receiver.position + posOffset;
                    player.GetComponent<Rigidbody>().position = receiver.position + posOffset;

                    enter = false;

                    if (mazeMode)
                    {
                        changeMazeEvent.Raise(this, null);
                    }

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enter = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enter = false;
        }
    }
}
