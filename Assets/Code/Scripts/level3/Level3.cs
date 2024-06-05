using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level3 : MonoBehaviour
{

    public GameObject dog;
    public GameObject player;

    public MazeCell pastCell;
    public MazeCell currentCell;

    public MultiMazeGenerator multiMazeGenerator;

    public float barkInterval = 5f;

    IEnumerator barkWarning;

    public GameObject lastWall;
    public Transform safeTp;

    private bool end = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        pastCell = null;
        currentCell = null;
        dog.GetComponent<DogLevel3>().target = player.transform;
        multiMazeGenerator.DeleteFirstWall();
        multiMazeGenerator.DeleteFirstAWall();
        multiMazeGenerator.AddCeilings();
        end = false;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            dog.GetComponent<DogLevel3>().FollowPlayer();
        }
        else
        {
            dog.GetComponent<DogLevel3>().DoLastAnim();
            if (Input.GetKeyDown(KeyCode.E))
            {
                LevelsManager.instance.transitionToGoodEndingCutscene();
            }
        }
    }

    public void OnCurrentMazeCellChange(Component sender, object data)
    {

        pastCell = currentCell;
        currentCell = (MazeCell)sender;

        if (currentCell != null)
        {
            if (currentCell.isPath)
            {
                Debug.Log("Player is on path");
                if (barkWarning != null)
                    StopCoroutine(barkWarning);
            }
            else if (pastCell != null && pastCell.isPath)
            {
                if (barkWarning == null)
                    barkWarning = BarkWarning();
                StartCoroutine(barkWarning);
            }
        }


    }

    public IEnumerator BarkWarning()
    {
        while (true)
        {
            yield return new WaitForSeconds(barkInterval);
            dog.GetComponent<DogLevel3>().playSingleBark();
        }
    }

    public void OnMazeChange(Component sender, object data)
    {
        dog.GetComponent<NavMeshAgent>().enabled = false;
        dog.transform.position = player.transform.position + new Vector3(1, 0, 1);
        dog.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void OnLastRoom(Component sender, object data)
    {
        if (dog.transform.position.z < safeTp.transform.position.z)
        {
            dog.GetComponent<NavMeshAgent>().enabled = false;
            dog.transform.position = safeTp.transform.position;
            dog.GetComponent<NavMeshAgent>().enabled = true;
        }

        lastWall.SetActive(true);
        end = true;



    }

}