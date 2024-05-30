using System.Collections;
using UnityEngine;

public class MultiMazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenPrefab;
    [SerializeField] private MazeGenerator[] mazes;
    [SerializeField] private MazeTheme[] themes;
    [SerializeField] private int nMazes;
    [SerializeField] private int distanceBetweenMazes;

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
        for(int i = 0; i < nMazes; i++)
        {
            mazes[i].CreateGrid();

            if (themes[i] != null)
                mazes[i].SetTheme(themes[i]);
                  
            mazes[i].GenerateMaze();
        }
    }

    public void DestroyAllMazes()
    {
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
}
