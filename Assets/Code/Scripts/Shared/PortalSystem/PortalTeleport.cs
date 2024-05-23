using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform player;
    public Transform receiver;

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

                    Debug.Log("vel + portal: " + (vel + portalToPlayer).magnitude + " portal: " + portalToPlayer.magnitude + "vel + portal: " + (vel + portalToPlayer) + " portal: " + portalToPlayer + " this: " + gameObject.name);

                    Debug.Log("Old position: " + player.position);

                    player.position = receiver.position + posOffset;
                    player.GetComponent<Rigidbody>().position = receiver.position + posOffset;

                    Debug.Log("New position: " + player.position);

                    enter = false;

                    //push player
                    //Vector3 force = receiver.forward * 10f;
                    //player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);


                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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
