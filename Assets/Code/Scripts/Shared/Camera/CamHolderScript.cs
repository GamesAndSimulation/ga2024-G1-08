using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolderScript : MonoBehaviour
{

    public Transform headPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = headPos.position;
    }
}
