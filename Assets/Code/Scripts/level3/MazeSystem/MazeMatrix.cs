using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MazeMatrix
{

    [SerializeField] public List<MazeCell> grid;

    [SerializeField] public int width;
    [SerializeField] public int height;

    public void InitGrid(int height, int width)
    {
        grid = new List<MazeCell>(height*width);
     
        this.width = width;
        this.height = height;

        for(int x = 0; x < width*height; x++)
        {
            grid.Add(null);
        }
    }

    public void Set(int x, int z, MazeCell cell)
    {
        int index = x * width + z;
        grid[index] = cell;
    }

    public MazeCell Get(int x, int z)
    {
        int index = x * width + z;
        return grid[index];
    }

}
