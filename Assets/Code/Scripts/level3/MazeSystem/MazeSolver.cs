using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver
{


    public List<MazeCell> FindPathDFS(MazeCell startCell, MazeCell targetCell, MazeMatrix grid)
    {
        HashSet<MazeCell> visited = new HashSet<MazeCell>();
        Stack<MazeCell> stack = new Stack<MazeCell>();
        Dictionary<MazeCell, MazeCell> cameFrom = new Dictionary<MazeCell, MazeCell>();

        stack.Push(startCell);

        while (stack.Count > 0)
        {
            MazeCell currentCell = stack.Pop();
            visited.Add(currentCell);

            if (currentCell == targetCell)
            {
                List<MazeCell> path = new List<MazeCell>();
                MazeCell traceBack = targetCell;
                while (traceBack != startCell)
                {
                    path.Add(traceBack);
                    traceBack = cameFrom[traceBack];
                }
                path.Add(startCell);
                path.Reverse();
                return path;
            }

            foreach (MazeCell neighbour in GetUnvisitedNeighbours(currentCell, grid))
            {
                if (!visited.Contains(neighbour))
                {
                    stack.Push(neighbour);
                    cameFrom[neighbour] = currentCell;
                }
            }
        }

        return null;
    }

    private List<MazeCell> GetUnvisitedNeighbours(MazeCell cell, MazeMatrix grid)
    {
        List<MazeCell> neighbours = new List<MazeCell>();
        int x = (int)Math.Round(cell.transform.localPosition.x) / cell.width;
        int z = (int)Math.Round(cell.transform.localPosition.z) / cell.height;

        if (x > 0 && !cell.HasWall(Direction.West) && !grid.Get(x - 1, z).dfsVisited)
        {
            neighbours.Add(grid.Get(x - 1, z));
        }
        if (x < grid.width - 1 && !cell.HasWall(Direction.East) && !grid.Get(x + 1, z).dfsVisited)
        {
            neighbours.Add(grid.Get(x + 1, z));
        }
        if (z > 0 && !cell.HasWall(Direction.South) && !grid.Get(x, z - 1).dfsVisited)
        {
            neighbours.Add(grid.Get(x, z - 1));
        }
        if (z < grid.height - 1 && !cell.HasWall(Direction.North) && !grid.Get(x, z + 1).dfsVisited)
        {
            neighbours.Add(grid.Get(x, z + 1));
        }

        return neighbours;
    }

}
