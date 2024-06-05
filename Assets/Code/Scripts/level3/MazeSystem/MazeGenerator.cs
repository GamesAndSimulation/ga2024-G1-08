using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

[Serializable]
public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Vars")]
    [SerializeField] private MazeCell cellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private MazeTheme theme;

    [Header ("Portals")]
    public PortalDecor portalA;
    public PortalDecor portalB;

    [Header("Grid")]
    [SerializeField]private MazeMatrix grid;
    private int cellWidth;
    private int cellHeight;
    private Transform cellsParent;

    private List<MazeCell> deadEnds;
    [SerializeField]private List<MazeCell> portalBsurrounders;
    private MazeSolver solver;


    private void Start()
    {
        Debug.Log("surrounders: " + portalBsurrounders.Count);
    }

    public void SetTheme(MazeTheme theme)
    {
        this.theme = theme;
    }

    public void CreateGrid()
    {
        DestroyGrid();

        cellsParent = new GameObject("MazeCells").transform;
        cellsParent.transform.position = this.transform.position;
        cellsParent.SetParent(transform);

        cellWidth = cellPrefab.width;
        cellHeight = cellPrefab.height;

        grid = new MazeMatrix();
        grid.InitGrid(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                MazeCell newCell = Instantiate(cellPrefab, new Vector3(x * cellWidth, 0, z * cellHeight) + cellsParent.transform.position, Quaternion.identity, cellsParent);
                grid.Set(x,z,newCell);
            }
        }
    }

    public void DestroyGrid()
    {
        if (grid == null || grid.height == 0 || grid.width == 0)
        {
            return;
        }

        int gridWidth = grid.width;
        int gridHeight = grid.height;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                if (grid.Get(x,z) != null)
                {
                    DestroyImmediate(grid.Get(x,z).gameObject);
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

        MazeCell startCell = grid.Get(0,0);
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
                break;
            }
        } while (nextCell != null);

    }

    private List<MazeCell> GetUnvisitedNeighbours(MazeCell cell)
    {
        List<MazeCell> neighbours = new List<MazeCell>();
        int x = (int)Math.Round(cell.transform.localPosition.x) / cellWidth;
        int z = (int)Math.Round(cell.transform.localPosition.z) / cellHeight;


        if (x > 0 && !grid.Get(x - 1, z).visited)
        {
            neighbours.Add(grid.Get(x - 1, z));
        }
        if (x < width - 1 && !grid.Get(x + 1, z).visited)
        {
            neighbours.Add(grid.Get(x + 1, z));
        }
        if (z > 0 && !grid.Get(x, z - 1).visited)
        {
            neighbours.Add(grid.Get(x, z - 1));
        }
        if (z < height - 1 && !grid.Get(x, z + 1).visited)
        {
            neighbours.Add(grid.Get(x, z + 1));
        }

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

    }

    public void GenDecorations()
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MazeCell cell = grid.Get(x, y);
                cell.SetTheme(theme);
                cell.GenerateDecor();
            }
        }
    }


    public PortalDecor GenPortalA(PortalDecor prevPortal)
    {
        MazeCell mazeCell = grid.Get(0, 0);
        
        GameObject portal = theme.portal;

        portalA = mazeCell.CreatePortal(portal, Direction.South);

        portalA.x = 0;
        portalA.z = 0;

        return portalA;
    }

    public PortalDecor GenPortalB()
    {
        FindDeadEnds(Direction.North); 

        if(deadEnds.Count == 0)
        {
            GenerateMaze();
            return null;
        }

        MazeCell mazeCell = deadEnds.ToArray()[UnityEngine.Random.Range(0, deadEnds.Count)];

        GameObject toPortal = theme.portal;
        
        portalB = mazeCell.CreatePortal(toPortal, Direction.North);

        int x = (int)Math.Round(mazeCell.transform.localPosition.x) / cellWidth;
        int z = (int)Math.Round(mazeCell.transform.localPosition.z) / cellHeight;

        portalB.x = x;
        portalB.z = z;

        AddSurrounders(x, z, portalB.dir);

        if(solver == null)
            solver = new MazeSolver();

        List<MazeCell> path = solver.FindPathDFS(grid.Get(0,0), grid.Get(x,z), grid);

        foreach(MazeCell cell in path)
        {
            cell.SetPath();
        }
        
        return portalB;
    }

    private void FindDeadEnds() // for a random Dead End
    {
        if(deadEnds == null)
            deadEnds = new List<MazeCell>();
        else
            deadEnds.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if(!(x == 0 && z == 0))
                {
                    MazeCell cell = grid.Get(x, z);
                    if (cell.nWalls == 3)
                    {
                        deadEnds.Add(cell);
                    }

                }
                    
            }
        }

    }

    private void FindDeadEnds(Direction dir) // for a pretended direction for portal
    {
        if (deadEnds == null)
            deadEnds = new List<MazeCell>();
        else
            deadEnds.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (!(x == 0 && z == 0))
                {
                    MazeCell cell = grid.Get(x, z);
                    if (cell.nWalls == 3 && cell.DeadEndCentered(dir))
                    {
                        deadEnds.Add(cell);
                    }

                }

            }
        }

    }

    private void AddSurrounders(int x, int z, Direction dir)
    {
        if(portalBsurrounders == null)
            portalBsurrounders = new List<MazeCell>();

        bool xPlusSafe = x + 1 < width;
        bool xMinusSafe = x - 1 >= 0;
        bool zPlusSafe = z + 1 < height;
        bool zMinusSafe = z - 1 >= 0;

        bool xPlus2Safe = x + 2 < width;
        bool xMinus2Safe = x - 2 >= 0;
        bool zPlus2Safe = z + 2 < height;
        bool zMinus2Safe = z - 2 >= 0;

        switch (dir)
        {
            case Direction.North:
                {
                    if (xPlusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x + 1, z));
                        if (zPlusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 1, z + 1));
                            if(zPlus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x + 1, z + 2));
                            }
                        }
                        if(xPlus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 2, z));
                            if(zPlusSafe)
                            {
                                portalBsurrounders.Add(grid.Get(x + 2, z + 1));
                                if(zPlus2Safe)
                                {
                                    portalBsurrounders.Add(grid.Get(x + 2, z + 2));
                                }
                            }
                        }
                    }
                    if (xMinusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x - 1, z));
                        if (zPlusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 1, z + 1));
                            if(zPlus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 1, z + 2));
                            }
                        }
                        if (xMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 2, z));
                            if (zPlusSafe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 2, z + 1));
                                if(zPlus2Safe)
                                {
                                    portalBsurrounders.Add(grid.Get(x - 2, z + 2));
                                }
                            }
                        }
                    }
                    if (zPlusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x, z + 1));
                        if(zPlus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x, z + 2));
                        }
                    }

                    break;
                }
            case Direction.South:
                {
                    if(xPlusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x + 1, z));
                        if (zMinusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 1, z - 1));
                        }
                        if (zMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 1, z - 2));
                        }
                    }
                    if(xPlus2Safe)
                    {
                        portalBsurrounders.Add(grid.Get(x + 2, z));
                        if(zMinusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 2, z - 1));
                        }
                        if(zMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 2, z - 2));
                        }
                    }
                    if (xMinusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x - 1, z));
                        if(zMinusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 1, z - 1));
                            if(zMinus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 1, z - 2));
                            }
                        }
                    }
                    if (xMinus2Safe)
                    {
                       portalBsurrounders.Add(grid.Get(x - 2, z));
                        if (zMinusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 2, z - 1));
                            if (zMinus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 2, z - 2));
                            }
                        }
                    }
                    if (zMinusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x, z - 1));
                        if(zMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x, z - 2));
                        }   
                    }

                    break;
                }
            case Direction.East:
                {
                    if (zPlusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x, z + 1));
                        if (xPlusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 1, z + 1));
                            if(xPlus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x + 2, z + 1));
                            }
                        }
                        if (zPlus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x, z + 2));
                            if (xPlusSafe)
                            {
                                portalBsurrounders.Add(grid.Get(x + 1, z + 2));
                                if (xPlus2Safe)
                                {
                                    portalBsurrounders.Add(grid.Get(x + 2, z + 2));
                                }
                            }
                        }
                    }
                    if(zMinusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x, z - 1));
                        if (xPlusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 1, z - 1));
                            if(xPlus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x + 2, z - 1));
                            }
                        }
                        if (zMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x, z - 2));
                            if (xPlusSafe)
                            {
                                portalBsurrounders.Add(grid.Get(x + 1, z - 2));
                                if (xPlus2Safe)
                                {
                                    portalBsurrounders.Add(grid.Get(x + 2, z - 2));
                                }
                            }
                        }
                    }
                    if(xPlusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x + 1, z));
                        if(xPlus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x + 2, z));
                        }
                    }
                    break;
                }
            case Direction.West:
                {
                    if (zPlusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x, z + 1));
                        if (xMinusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 1, z + 1));
                            if(xMinus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 2, z + 1));
                            }
                        }
                        if (zPlus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x, z + 2));
                            if (xMinusSafe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 1, z + 2));
                                if(xMinus2Safe)
                                {
                                    portalBsurrounders.Add(grid.Get(x - 2, z + 2));
                                }
                            }
                        }
                    }
                    if(zMinusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x, z - 1));
                        if(xMinusSafe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 1, z - 1));
                            if(xMinus2Safe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 2, z - 1));
                            }
                        }
                        if (zMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x, z - 2));
                            if (xMinusSafe)
                            {
                                portalBsurrounders.Add(grid.Get(x - 1, z - 2));
                                if(xMinus2Safe)
                                {
                                    portalBsurrounders.Add(grid.Get(x - 2, z - 2));
                                }
                            }
                        }
                    }
                    if (xMinusSafe)
                    {
                        portalBsurrounders.Add(grid.Get(x - 1, z));
                        if (xMinus2Safe)
                        {
                            portalBsurrounders.Add(grid.Get(x - 2, z));
                        }
                    }
                    break;
                }
        }

        Debug.Log("Surrounders: " + portalBsurrounders.Count);
    }

    public void DeactivatePortalAWalls()
    {
        MazeCell mazeCell = grid.Get(0, 0);
        mazeCell.DeactivateWall(portalA.dir);
    }

    public void DeactivatePortalBWalls()
    {
        MazeCell mazeCell = grid.Get(portalB.x, portalB.z);
        mazeCell.DeactivateWall(portalB.dir);
        foreach (MazeCell cell in portalBsurrounders)
        {
            cell.DeactivateCell();
        }

    }

    public void ActivatePortalAWalls()
    {
        MazeCell mazeCell = grid.Get(0, 0);
        mazeCell.ActivateWall(portalA.dir);
    }

    public void ActivatePortalBWalls()
    {
        MazeCell mazeCell = grid.Get(portalB.x,portalB.z);
        mazeCell.ActivateWall(portalB.dir);

        foreach (MazeCell cell in portalBsurrounders)
        {
            cell.ReActivateCell();
        }
    }

    public void DeleteFirstWall()
    {
        MazeCell mazeCell = grid.Get(0, 0);
        mazeCell.DeactivateWall(Direction.South);
        mazeCell.DeleteDecor();
    }
    
    public void PathToEnd()
    {
        if(solver == null)
            solver = new MazeSolver();

        List<MazeCell> path = solver.FindPathDFS(grid.Get(0, 0), grid.Get(width - 1, height - 1), grid);

        foreach (MazeCell cell in path)
        {
            cell.SetPath();
        }
    }

    public void DeleteLastWall()
    {
        MazeCell mazeCell = grid.Get(width - 1, height - 1);
        mazeCell.DeactivateWall(Direction.North);
        mazeCell.DeleteDecor();    
    }

    public void AddCeiling()
    {
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                MazeCell cell = grid.Get(j, i);
                cell.AddCeiling();
            }
        }
    }
}
