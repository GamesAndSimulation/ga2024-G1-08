using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour
{

    public GameObject dog;
    public GameObject player;

    public MazeCell pastCell;
    public MazeCell currentCell;

    public float barkInterval = 5f;

    IEnumerator barkWarning;

    // Start is called before the first frame update
    void Start()
    {
        pastCell = null;
        currentCell = null;
        dog.GetComponent<DogLevel3>().target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        dog.GetComponent<DogLevel3>().FollowPlayer();
    }

    public void OnCurrentMazeCellChange(Component sender, object data)
    {

        pastCell = currentCell;
        currentCell = (MazeCell) sender;

        if (currentCell != null)
        {
            if (currentCell.isPath)
            {
                Debug.Log("Player is on path");
                if(barkWarning != null)
                    StopCoroutine(barkWarning);
            }
            else if (pastCell != null && pastCell.isPath)
            {
                if(barkWarning == null)
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

}
