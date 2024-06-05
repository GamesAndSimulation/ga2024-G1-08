using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TrampolineBehavior : MonoBehaviour
{

    [SerializeField] private float jumpForce = 50;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0) * jumpForce, ForceMode.Impulse);
        
    }
}
