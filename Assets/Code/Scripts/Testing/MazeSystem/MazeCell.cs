using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{

    [SerializeField] private GameObject n_wall;
    [SerializeField] private GameObject s_wall;
    [SerializeField] private GameObject e_wall;
    [SerializeField] private GameObject w_wall;

    [SerializeField] public int width;
    [SerializeField] public int height;

    public bool visited = false;

    private void Start()
    {
        width = 1;
        height = 1;
    }

    public void SetVisited()
    {
        visited = true;
    }

    public void RemoveWall(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                n_wall.SetActive(false);
                break;
            case Direction.South:
                s_wall.SetActive(false);
                break;
            case Direction.East:
                e_wall.SetActive(false);
                break;
            case Direction.West:
                w_wall.SetActive(false);
                break;
        }
    }   

}
