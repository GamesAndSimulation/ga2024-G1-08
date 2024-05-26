using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell cellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;

    private MazeCell[,] grid;
    private int cellWidth;
    private int cellHeight;
    private Transform cellsParent;

    public void CreateGrid()
    {
        // Clear any existing grid
        DestroyGrid();

        // Create a new parent GameObject to hold all cells
        cellsParent = new GameObject("MazeCells").transform;
        cellsParent.SetParent(transform); // Set this GameObject as a child of the MazeGenerator object

        cellWidth = cellPrefab.width;
        cellHeight = cellPrefab.height;

        grid = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                MazeCell newCell = Instantiate(cellPrefab, new Vector3(x * cellWidth, 0, z * cellHeight), Quaternion.identity, cellsParent);
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

        // Destroy the parent GameObject as well
        if (cellsParent != null)
        {
            DestroyImmediate(cellsParent.gameObject);
        }
    }

    public void GenerateMaze()
    {
        // Always create a new grid before generating the maze
        CreateGrid();

        // Start maze generation from the initial cell
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
                nextCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                GenerateMazeRecursive(currentCell, nextCell);
            }
            else
            {
                break;
            }
        } while (nextCell != null);
    }

    private List<MazeCell> GetUnvisitedNeighbours(MazeCell cell)
    {
        List<MazeCell> neighbours = new List<MazeCell>();
        int x = (int)cell.transform.position.x / cellWidth;
        int z = (int)cell.transform.position.z / cellHeight;

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

        return neighbours;
    }

    private void RemoveWall(MazeCell currentCell, MazeCell neighbour)
    {
        int x = (int)currentCell.transform.position.x / cellWidth;
        int z = (int)currentCell.transform.position.z / cellHeight;
        int nx = (int)neighbour.transform.position.x / cellWidth;
        int nz = (int)neighbour.transform.position.z / cellHeight;

        if (x > nx)
        {
            currentCell.RemoveWall(Direction.West);
            neighbour.RemoveWall(Direction.East);
        }
        else if (x < nx)
        {
            currentCell.RemoveWall(Direction.East);
            neighbour.RemoveWall(Direction.West);
        }
        else if (z > nz)
        {
            currentCell.RemoveWall(Direction.South);
            neighbour.RemoveWall(Direction.North);
        }
        else if (z < nz)
        {
            currentCell.RemoveWall(Direction.North);
            neighbour.RemoveWall(Direction.South);
        }
    }
}
