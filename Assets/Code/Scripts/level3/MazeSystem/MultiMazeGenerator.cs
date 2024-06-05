using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiMazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenPrefab;
    [SerializeField] private MazeGenerator[] mazes;
    [SerializeField] private MazeTheme[] themes;
    [SerializeField] private int nMazes;
    [SerializeField] private int distanceBetweenMazes;

    public List<PortalDecor> portals;

    private int playerCurrentMaze = 0;


    private void Start()
    {
        playerCurrentMaze = 0;
        
    }

    private void AddMaze(int i)
    {

        if(i > 0)
        {
            Vector3 prevGen = mazes[i-1].transform.position;
            mazes[i] = Instantiate(mazeGenPrefab, new Vector3(prevGen.x, prevGen.y + distanceBetweenMazes, prevGen.z), Quaternion.identity, this.transform);
        }
        else
        {
            mazes[0] = Instantiate(mazeGenPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        }
            
    }

    public void AddAllMazes()
    {
        DestroyAllMazes();
        mazes = new MazeGenerator[nMazes];
        themes = new MazeTheme[nMazes];

        for(int i = 0; i < nMazes; i++)
        {
            if (mazes[i] == null)
                AddMaze(i);
        }
    }

    public void GenerateAllMazes()
    {
        portals = new List<PortalDecor>();

        for(int i = 0; i < nMazes; i++)
        {
            mazes[i].CreateGrid();

            if (themes[i] != null)
                mazes[i].SetTheme(themes[i]);
                  
            mazes[i].GenerateMaze();

            mazes[i].GenDecorations();

            if (i == 0)
               portals.Add(mazes[i].GenPortalB());

            else if (i == nMazes-1)
            {
                portals.Add(mazes[i].GenPortalA(mazes[i-1].portalB));
                mazes[i].PathToEnd();
            }

            else
            {
                portals.Add(mazes[i].GenPortalA(mazes[i - 1].portalB));
                portals.Add(mazes[i].GenPortalB());
            }
        }

        HandlePortals();
    }

    private void HandlePortals()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject playerCam = GameObject.Find("Main Camera");

        for(int i = 0; i < portals.Count-1; i += 2)
        {
            int j = i + 1;

            PortalDecor portalA = portals[i];
            PortalDecor portalB = portals[j];

            portalA.SetPlayer(player.transform);
            portalB.SetPlayer(player.transform);

            portalA.SetReceiver(portalB.GetComponentInChildren<PortalTeleport>().transform);
            portalB.SetReceiver(portalA.GetComponentInChildren<PortalTeleport>().transform);

            portalA.SetPlayerCam(playerCam.transform);
            portalB.SetPlayerCam(playerCam.transform);
            portalA.SetThisPortal();
            portalB.SetThisPortal();
            portalA.SetOtherPortal(portalB.transform);
            portalB.SetOtherPortal(portalA.transform);

            Camera camA = portalA.GetComponentInChildren<Camera>();
            Camera camB = portalB.GetComponentInChildren<Camera>();

            portalA.SetPortalCam(camA);
            portalB.SetPortalCam(camB);

            Material matA = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
            Material matB = new Material(Shader.Find("Unlit/ScreenCutoutShader"));

            matA.name = "PortalMatA" + i;
            matB.name = "PortalMatB" + j;

            portalA.SetPortalMat(matA);
            portalB.SetPortalMat(matB);

            portalA.ApplyMatToPlane(matB);
            portalB.ApplyMatToPlane(matA);

        }
    }

    public void DestroyAllMazes()
    {
        if(mazes == null)
        {
            return;
        }

        for(int i = 0; i < nMazes; i++)
        {
            if (mazes[i] != null)
            {
                mazes[i].DestroyGrid();
                DestroyImmediate(mazes[i].gameObject);
            }

            mazes[i] = null;
        }

        mazes = null;
        themes = null;
    }

    public void OnMazeChange(Component sender, object data)
    {
        int index = portals.FindIndex(p => p == sender.gameObject.GetComponentInParent<PortalDecor>());

        if (index % 2 == 0) //B Portals
        {
            playerCurrentMaze++;
            mazes[playerCurrentMaze].ActivatePortalAWalls();
            mazes[playerCurrentMaze - 1].DeactivatePortalBWalls();
            
            if(playerCurrentMaze != nMazes-1) // Last B Portal
                mazes[playerCurrentMaze + 1].DeactivatePortalAWalls();
        
        }
        else //A Portals
        {
            playerCurrentMaze--;

            mazes[playerCurrentMaze].ActivatePortalBWalls();
            mazes[playerCurrentMaze + 1].DeactivatePortalAWalls();

            if(playerCurrentMaze != 0) // First A Portal
                mazes[playerCurrentMaze - 1].DeactivatePortalBWalls();

        }
       
    }

    public void DeleteFirstWall()
    {
        mazes[0].DeleteFirstWall();
    }

    public void DeleteFirstAWall()
    {
        mazes[1].DeleteFirstWall();
    }

    public void DeleteLastWall()
    {
        mazes[nMazes-1].DeleteLastWall();
    }

    public void AddCeilings()
    {
       foreach(MazeGenerator maze in mazes)
        {
           maze.AddCeiling();
       }
    }
}
