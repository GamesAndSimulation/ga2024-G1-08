using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1MainScript : MonoBehaviour
{
    public AudioSource poopSfx;
    public AudioSource music;
    public AudioSource ambience;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject poop;
    public GameObject UI;
    public GameObject trigger;
    public GameObject camera;
    public ShovelScript shovel;
    public int totalPoops = 10;
    public float totalTime = 3.0f;
    public AudioVolume audioVolume;

    private int score;
    private bool timerStart;
    private float timer;

    public const string GAMEOVER = "GameOver";
    public const string WINNINGSCENE = "WinningScene";
    public const float WAITTIME_STARTMUSIC = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString() + " / " + totalPoops.ToString();


        timer = totalTime * 60;
        timerStart = false;
        timerText.text = totalTime.ToString("00") + ":00";
    }

    // Update is called once per frame
    void Update()
    {
        if(timerStart)
        {
            int minutes, seconds;
            if (timer <= 0.5)
            {
                minutes = 0;
                seconds = 0;
                if(score < totalPoops)
                    SceneManager.LoadScene(GAMEOVER);
                else
                    SceneManager.LoadScene(WINNINGSCENE);
            }
            else
            {
                timer -= Time.deltaTime;
                minutes = Mathf.FloorToInt(timer / 60f);
                seconds = Mathf.FloorToInt(timer - minutes * 60);
            }
           

            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            

        }
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
        StartCoroutine(audioVolume.IncreaseVolume(music, WAITTIME_STARTMUSIC));
        StartCoroutine(audioVolume.ReduceVolume(ambience, WAITTIME_STARTMUSIC));
        music.Play();
        shovel.Appear();
        trigger.SetActive(false);
        camera.SetActive(false);
        poop.SetActive(true);
        UI.SetActive(true);
        StartCoroutine(WaitForMusic());
    }

    IEnumerator WaitForMusic()
    {
        yield return new WaitForSeconds(WAITTIME_STARTMUSIC);
        timerStart = true;
        ambience.Stop();
    }

}
