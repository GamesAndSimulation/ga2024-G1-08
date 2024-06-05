using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDecoration : MonoBehaviour, Decoration 
{

    [SerializeField] private Vector3 basePosition;
    [SerializeField] private Vector3 baseRotation; //baseRotation is always set by the North decor spot


    public void GenObject(Direction dir)
    {
        transform.localPosition = basePosition;
        transform.localRotation = Quaternion.Euler(baseRotation);
    }

}
