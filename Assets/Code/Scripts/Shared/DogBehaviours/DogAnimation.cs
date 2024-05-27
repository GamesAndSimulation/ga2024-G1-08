using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : MonoBehaviour
{

    public Animator animator;

    private const string VEL = "Velocity";
    private const string SBARK = "SingleBark";
    private const string SNIFFDOWN = "IsSniffingDown";
    private const string SNIFFUP = "IsSniffingUp";

    private const float TAIL_WAG = 0.0f;
    private const float SINGLE_BARK = 0.5f;

    private const float BARK_TIME = 0.25f;

    public AnimationClip barkClip;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovingAnim(float velocity)
    {
        animator.SetFloat(VEL, velocity);
    }

    public void StartIdle()
    {
        animator.SetFloat(VEL, 0);
    }

    public void PlaySingleBark()
    {
        animator.SetBool(SBARK, true);
        StartCoroutine(BarkTimer());
    }

    public void SniffDown()
    {
        animator.SetBool(SNIFFDOWN, true);
    }

    public void StopSniffDown()
    {
        animator.SetBool(SNIFFDOWN, false);
    }

    public void SniffUp()
    {
        animator.SetBool(SNIFFUP, true);
    }

    public void StopSniffUp()
    {
        animator.SetBool(SNIFFUP, false);
    }

    private IEnumerator BarkTimer()
    {
        yield return new WaitForSeconds(barkClip.length);
        animator.SetBool(SBARK, false);
    }

}
