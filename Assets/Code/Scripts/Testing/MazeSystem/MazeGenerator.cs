using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell cellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private MazeTheme theme;

    private MazeCell[,] grid;
    private int cellWidth;
    private int cellHeight;
    private Transform cellsParent;

    public void CreateGrid()
    {
        DestroyGrid();

        cellsParent = new GameObject("MazeCells").transform;
        cellsParent.transform.position = this.transform.position;
        cellsParent.SetParent(transform); 

        cellWidth = cellPrefab.width;
        cellHeight = cellPrefab.height;

        grid = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                MazeCell newCell = Instantiate(cellPrefab, new Vector3(x * cellWidth, 0, z * cellHeight) + cellsParent.transform.position, Quaternion.identity, cellsParent);
                grid[x, z] = newCell;
            }
        }
    }

    public void DestroyGrid()
    {
        if (grid == null)
        {
            return;
        }

        int gridWidth = grid.GetLength(0);
        int gridHeight = grid.GetLength(1);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                if (grid[x, z] != null)
                {
                    DestroyImmediate(grid[x, z].gameObject);
                }
            }
        }

        grid = null;

        if (cellsParent != null)
        {
            DestroyImmediate(cellsParent.gameObject);
        }
    }

    public void GenerateMaze()
    {
        CreateGrid();

        MazeCell startCell = grid[0, 0];
        GenerateMazeRecursive(null, startCell);
    }

    private void GenerateMazeRecursive(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.SetVisited();

        if (previousCell != null)
        {
            RemoveWall(previousCell, currentCell);
        }
       
        List<MazeCell> unvisitedNeighbours;
        MazeCell nextCell;

        do
        {
            unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);

            if (unvisitedNeighbours.Count > 0)
            {
                nextCell = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                GenerateMazeRecursive(currentCell, nextCell);
            }
            else
            {
                if(!currentCell.hasGeneratedDecor)
                {
                    currentCell.SetTheme(theme);
                    currentCell.GenerateDecor();
                }

                break;
            }
        } while (nextCell != null);

    }

    private List<MazeCell> GetUnvisitedNeighbours(MazeCell cell)
    {
        List<MazeCell> neighbours = new List<MazeCell>();
        int x = (int) Math.Round(cell.transform.localPosition.x) / cellWidth;
        int z = (int) Math.Round(cell.transform.localPosition.z) / cellHeight;


        if (x > 0 && !grid[x - 1, z].visited) 
        {
            neighbours.Add(grid[x - 1, z]);
        }
        if (x < width - 1 && !grid[x + 1, z].visited)
        {
            neighbours.Add(grid[x + 1, z]);
        }
        if (z > 0 && !grid[x, z - 1].visited)
        {
            neighbours.Add(grid[x, z - 1]);
        }
        if (z < height - 1 && !grid[x, z + 1].visited)
        {
            neighbours.Add(grid[x, z + 1]);
        }

        Debug.Log("Cell: " + cell.transform.localPosition + " X: " + cell.transform.localPosition.x + "/" + cellWidth + " = " + cell.transform.localPosition.x / cellWidth
        + " Z: " + cell.transform.localPosition.z + "/" + cellHeight + " = " + cell.transform.localPosition.z / cellHeight + " Neigbours: " + neighbours.Count);
        return neighbours;
    }

    private void RemoveWall(MazeCell previousCell, MazeCell currentCell)
    {
        int x = (int)previousCell.transform.position.x / cellWidth;
        int z = (int)previousCell.transform.position.z / cellHeight;
        int nx = (int)currentCell.transform.position.x / cellWidth;
        int nz = (int)currentCell.transform.position.z / cellHeight;

        if (x > nx)
        {
            previousCell.RemoveWall(Direction.West);
            currentCell.RemoveWall(Direction.East);
        }
        else if (x < nx)
        {
            previousCell.RemoveWall(Direction.East);
            currentCell.RemoveWall(Direction.West);
        }
        else if (z > nz)
        {
            previousCell.RemoveWall(Direction.South);
            currentCell.RemoveWall(Direction.North);
        }
        else if (z < nz)
        {
            previousCell.RemoveWall(Direction.North);
            currentCell.RemoveWall(Direction.South);
        }

        if(!previousCell.hasGeneratedDecor)
        {
            previousCell.SetTheme(theme);
            previousCell.GenerateDecor();
        }

    }
}
