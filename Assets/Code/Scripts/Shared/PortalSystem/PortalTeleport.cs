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

            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            Debug.Log("DOT PRODUCT " + dotProduct);

            if(dotProduct < 0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);
                Vector3 posOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + posOffset;

                //push player
                //Vector3 force = receiver.forward * 10f;
                //player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

                enter = false;
                Debug.Log("TELEPORT " + dotProduct);

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Entered " + this.name);
            enter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Exit " + this.name);
            enter = false;
        }
    }
}
