using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelScript : MonoBehaviour
{
    public Animator shovelAnimator;
    public float distance = 3f;
    public const string IS_SHOVELING = "isShoveling";
    public const string POOP_LAYER = "Poop";
    public AudioSource audioSource;
    public GameObject text;

    private bool hasShoveled;

    [SerializeField]
    private GameEvent poopShoveled;
    // Start is called before the first frame update
    void Start()
    {
        hasShoveled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //right click to shovel
        if (Input.GetMouseButtonUp(1) && this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled == true )
        {
            Shovel();
        }
    }

    private void Shovel()
    {
        //triggers the animation of shoveling
        shovelAnimator.SetTrigger(IS_SHOVELING);

        //start and direction of the camera attached to the player
        Vector3 start = transform.parent.gameObject.transform.position;
        Vector3 direction = transform.parent.gameObject.transform.forward;
        RaycastHit hit;


        if (Physics.Raycast(start, direction, out hit, distance))
        {
            GameObject objectFound = hit.collider.gameObject;
            //checks if the object detected by the raycast is a poop. if so, raises an event to handle shovelling the poop
            if (objectFound.layer == LayerMask.NameToLayer(POOP_LAYER))
            {
                poopShoveled.Raise(this, objectFound);
                
                if(!hasShoveled)
                {
                    text.SetActive(false);
                    hasShoveled = true;
                }
            }

        }
    }

    public void Appear()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        audioSource.Play();
    }

    public void Disappear()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        audioSource.Play();
    }

}
