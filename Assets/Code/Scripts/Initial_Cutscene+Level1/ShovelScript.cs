using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelScript : MonoBehaviour
{
    public Animator shovelAnimator;
    public float distance = 3f;
    public const string POOP_LAYER = "Poop";
  

    [SerializeField]
    private GameEvent poopShoveled;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            //triggers the animation of shoveling
            shovelAnimator.SetTrigger("isShoveling");

            Vector3 start = transform.parent.gameObject.transform.position;
            Vector3 direction = transform.parent.gameObject.transform.forward;
            RaycastHit hit;

      
            if (Physics.Raycast(start, direction, out hit, distance))
            {
                GameObject objectFound = hit.collider.gameObject;
                if (objectFound.layer == LayerMask.NameToLayer(POOP_LAYER))
                {
                    poopShoveled.Raise(this, objectFound);

                }
                    
            }
        }
    }

    public void Appear()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        this.gameObject.GetComponent<AudioSource>().Play();
    }
  
}
