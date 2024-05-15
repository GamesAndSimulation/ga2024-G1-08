using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : MonoBehaviour
{

    public Animator animator;

    private const string VEL = "Velocity";

    public void MovingAnim(float velocity)
    {
        animator.SetFloat(VEL, velocity);
    }

    public void StartIdle()
    {
        animator.SetFloat(VEL, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
