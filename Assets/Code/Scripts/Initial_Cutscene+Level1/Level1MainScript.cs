using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1MainScript : MonoBehaviour
{
    public AudioSource poopSfx;
    public TextMeshProUGUI scoreText;
    public GameObject poop;
    public GameObject UI;
    public GameObject room;
    public ShovelScript shovel;
    private int score;
    private int totalPoops;
  
    

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        totalPoops = poop.transform.childCount;
        scoreText.text = score.ToString() + " / " + totalPoops.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPoopShovelled(Component sender, object data)
    {
        GameObject poop = (GameObject)data;
        poopSfx.Play();
        poop.SetActive(false);
        score++;
        scoreText.text = score.ToString() + " / " + totalPoops.ToString();
    }

    public void OnGardenEntered(Component sender, object data) {
        shovel.Appear();
        room.SetActive(false);
        poop.SetActive(true);
        UI.SetActive(true);
        
    }

}
