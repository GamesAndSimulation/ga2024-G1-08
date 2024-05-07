using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MusicManager : MonoBehaviour
{

    [SerializeReference]
    private MusicObject musicToPlay;


    [SerializeField]  private AudioSource currentMusicSource; //we use 2 music sources to switch from one to the other without delay
    [SerializeField]  private AudioSource nextMusicSource;

    private MusicObject.MusicNode currentNode; //we keep track of the current node (music we're playing and musics that should follow it)
    private MusicObject.MusicNode nextNode; //and the current next in line music

    private int currentMusicState; //what defines if we should advance in the music
    private int maxMusicState;

    private bool toPlayMusic;


    private List<MusicObject.MusicNode> musicNodes; //the nodes of the music

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
        currentMusicSource.clip = currentNode.clip.clip;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A)) 
            changeMusicState(currentMusicState + 1);


        
    }

    private void changeMusicState(int newState) {
        
        currentMusicState = newState;

        decideNextMusic();

        Debug.Log("Changed current state: " + currentMusicState);

    }

    public void stopMusic() {

        Debug.Log("Stopping music");
        toPlayMusic = false;
        currentMusicSource.Stop();
        nextMusicSource.Stop();

    }

    public void startMusic() {

        Debug.Log("Starting to play music");

        if (!toPlayMusic) {

            toPlayMusic = true;
            
            decideNextMusic() ;

            StartCoroutine(playMusic());
        }

    }
    private IEnumerator playMusic() {

        currentMusicSource.Play();

        while (toPlayMusic) {

            //we wait until the music stops playing or until the point we should not be playing music
            yield return new WaitUntil(() => (currentMusicSource.time >= currentMusicSource.clip.length - 0.15f) || !toPlayMusic || !currentMusicSource.isPlaying);


            if (toPlayMusic)
                schedulePlayMusic();


        }

    }

    private void schedulePlayMusic() {

        nextMusicSource.clip = nextNode.clip.clip;

        nextMusicSource.PlayDelayed(currentMusicSource.clip.length - currentMusicSource.time - 0.04f); //schedule to play the next clip

        currentNode = nextNode;

        AudioSource placeholder = currentMusicSource; //switch the sources
        currentMusicSource = nextMusicSource;
        nextMusicSource = placeholder;

        decideNextMusic();

    }

    private void decideNextMusic() {

        //if we looped our music
        if (currentMusicState >= maxMusicState && currentNode.musicState == maxMusicState) {
            
            currentMusicState = 0;
            nextNode = musicNodes[currentNode.activeNextNodeIndex];
        }

        //if we're behind the currentMusicState
        else if(currentMusicState > currentNode.musicState) 
            nextNode = musicNodes[currentNode.activeNextNodeIndex];

        //if we're stable in the current music state    
        else
            nextNode = musicNodes[currentNode.passiveNextNodeIndex];

        

    }

}
