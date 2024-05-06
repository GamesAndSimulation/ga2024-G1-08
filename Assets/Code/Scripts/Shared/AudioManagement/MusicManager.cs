using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MusicManager : MonoBehaviour
{

    [SerializeReference]
    private MusicObject musicToPlay;

    [SerializeField]
    private AudioSource musicSource;

    private MusicObject.MusicNode currentNode;

    private int currentMusicState;
    private int maxMusicState;
    public bool toPlayMusic;


    private List<MusicObject.MusicNode> musicNodes;

    // Start is called before the first frame update
    void Start()
    {
        toPlayMusic = false;
        
        resetMusicData();
        startMusic();
    }

    private void resetMusicData() {
       
        currentMusicState = 0;

        maxMusicState = 0;

        musicNodes = musicToPlay.musicNodes;

        foreach (MusicObject.MusicNode node in musicToPlay.musicNodes)
            maxMusicState = Mathf.Max(maxMusicState, node.musicState);

        currentNode = musicNodes[0];
        musicSource.clip = currentNode.clip.clip;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A)) {

            currentMusicState += 1;
            Debug.Log("Changed current state: " + currentMusicState);

        }

        if (Input.GetKeyDown(KeyCode.B)) {
            currentMusicState = 0;
            Debug.Log("Reset current state: " + currentMusicState);

        }
    }

    public void stopMusic() {

        Debug.Log("Stopping music");
        toPlayMusic = false;
        musicSource.Stop();

    }

    public void startMusic() {

        Debug.Log("Starting to play music");

        if (!toPlayMusic) {

            toPlayMusic = true;
            
            StartCoroutine(playMusic());
        }

    }
    private IEnumerator playMusic() {

        musicSource.Play();

        while (toPlayMusic) {

            //we wait until the music stops playing or until the point we should not be playing music
            yield return new WaitUntil(() => (musicSource.time >= musicSource.clip.length) || !toPlayMusic || !musicSource.isPlaying);

            decideNextMusic();

            musicSource.Play();

        }

    }

    private void decideNextMusic() {

        if (currentMusicState > maxMusicState && currentNode.musicState == maxMusicState) {
            
            currentMusicState = 0;
            currentNode = musicNodes[currentNode.activeNextNodeIndex];
        }
        else if(currentMusicState > currentNode.musicState) { 
            currentNode = musicNodes[currentNode.activeNextNodeIndex];

            }
        else
            currentNode = musicNodes[currentNode.passiveNextNodeIndex];


        musicSource.clip = currentNode.clip.clip;
    }

}
