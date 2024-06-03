using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAnimation : MonoBehaviour
{

    public Animator animator;

    private const string VEL = "Velocity";
    private const string SBARK = "SingleBark";

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
        //Debug.Log(animator.GetFloat(VEL));
        //Debug.Log(animator.GetBool(SBARK));
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

    private IEnumerator BarkTimer()
    {
        yield return new WaitForSeconds(barkClip.length);
        animator.SetBool(SBARK, false);
    }

}
