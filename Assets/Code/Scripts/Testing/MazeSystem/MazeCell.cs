using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MazeCell : MonoBehaviour
{
    [Header("Walls and Floor")]
    [SerializeReference] private GameObject n_wall;
    [SerializeReference] private GameObject s_wall;
    [SerializeReference] private GameObject e_wall;
    [SerializeReference] private GameObject w_wall;
    [SerializeReference] private GameObject floor;

    [Header("Wall Decors")]
    [SerializeReference] private GameObject n_wall_decor;
    [SerializeReference] private GameObject s_wall_decor;
    [SerializeReference] private GameObject e_wall_decor;
    [SerializeReference] private GameObject w_wall_decor;

    [Header("Floor Decors")]
    [SerializeReference] private GameObject m_decor;
    [SerializeReference] private GameObject n_decor;
    [SerializeReference] private GameObject s_decor;
    [SerializeReference] private GameObject e_decor;
    [SerializeReference] private GameObject w_decor;

    [Header("Size Vars")]
    [SerializeReference] public int width;
    [SerializeReference] public int height;

    [Header("Theme")]
    [SerializeReference] private MazeTheme theme;

    private GameObject setDecor;

    [SerializeField]private List<GameObject> reactivatable;

    public bool visited = false;
    public bool hasGeneratedDecor = false;
    public int nWalls;

    public void SetVisited()
    {
        visited = true;
    }

    public void RemoveWall(Direction direction)
    {
        DeactivateWall(direction);

        nWalls--;
    }   

    public void SetTheme(MazeTheme theme)
    {
        this.theme = theme;
    }

    public void GenerateDecor()
    {
        if(theme != null && !hasGeneratedDecor)
        {
            //Apply Materials
            n_wall.GetComponentInChildren<Renderer>().material = theme.wallMaterial;
            s_wall.GetComponentInChildren<Renderer>().material = theme.wallMaterial;
            e_wall.GetComponentInChildren<Renderer>().material = theme.wallMaterial;
            w_wall.GetComponentInChildren<Renderer>().material = theme.wallMaterial;

            floor.GetComponentInChildren<Renderer>().material = theme.floorMaterial;

            //Gen Decoration
            List<GameObject> decors = theme.decorations;
            List<GameObject> wallDecors = theme.wallDecor;

            GameObject decorPlace = null;
            GameObject wallDecorPlace = null;
            Direction dir = Direction.North;

            int attempts = 0;
            while (decorPlace == null && attempts < 100)
            {
                attempts++;
                dir = (Direction)UnityEngine.Random.Range(0, 4);

                switch (dir)
                {
                    case Direction.North:
                        if (n_wall.activeInHierarchy)
                        {
                            decorPlace = n_decor;
                            wallDecorPlace = n_wall_decor;
                        }
                        break;
                    case Direction.South:
                        if (s_wall.activeInHierarchy)
                        {
                            decorPlace = s_decor;
                            wallDecorPlace = s_wall_decor;
                        }
                        break;
                    case Direction.East:
                        if (e_wall.activeInHierarchy)
                        {
                            decorPlace = e_decor;
                            wallDecorPlace = e_wall_decor;
                        }
                        break;
                    case Direction.West:
                        if (w_wall.activeInHierarchy)
                        {
                            decorPlace = w_decor;
                            wallDecorPlace = w_wall_decor;
                        }
                        break;
                }
            }
            if (decorPlace != null && decors.Count > 0 && UnityEngine.Random.Range(0,3) > 0) // 2/3 chance to spawn a standing decoration
            {
                int index = UnityEngine.Random.Range(0, decors.Count);
                   
                GameObject decor = Instantiate(decors.ToArray()[index] , decorPlace.transform.position, Quaternion.identity, decorPlace.transform);

                decor.GetComponent<Decoration>().GenObject(dir);

                setDecor = decor;
            }
            else if(wallDecorPlace != null && wallDecors.Count > 0){

                int index = UnityEngine.Random.Range(0, wallDecors.Count);

                GameObject decor = Instantiate(wallDecors.ToArray()[index], wallDecorPlace.transform.position, Quaternion.identity, wallDecorPlace.transform);

                decor.GetComponent<Decoration>().GenObject(dir);

                setDecor = decor;

            }

            hasGeneratedDecor = true;

        }

    }

    public void DeleteDecor()
    {
        
        if (hasGeneratedDecor)
        {
            DestroyImmediate(setDecor);
            hasGeneratedDecor = false;
        }

    }

    public PortalDecor CreatePortal(GameObject portal, Direction dir)
    {
        DeleteDecor();
        Transform trans = null;
        //Direction dir = DeadEndWall(); -> Due to the difficulty of making the portals work with different directions, this will be the last implemented thing #TODO

        switch (dir)
        {
            case Direction.North:
                {
                     trans = n_decor.transform;
                     break;
                }
            case Direction.South:
                { 
                     trans = s_decor.transform;
                     break;
                }
            case Direction.East:
                {
                    trans = e_decor.transform;
                    break;
                }
            case Direction.West:
                {
                    trans = w_decor.transform;
                    break;
                }
        }

        if (trans != null)
        {
            GameObject portalObj = Instantiate(portal, trans.transform.position, Quaternion.identity, trans.transform);
            portalObj.name = "Portal " + transform.position;
            portalObj.transform.GetChild(0).name = "PortalCam " + transform.position;
            portalObj.transform.GetChild(1).name = "PortalPlane " + transform.position;
            portalObj.transform.GetChild(2).name = "PortalCollider " + transform.position;
            setDecor = portalObj;
            hasGeneratedDecor = true;

            portalObj.GetComponent<PortalDecor>().GenObject(dir);
            return portalObj.GetComponent<PortalDecor>();
        }

        return null;
    }

    private Direction DeadEndWall()
    {
        if(!n_wall.activeInHierarchy)
        {
            return Direction.South;
        }
        else if (!s_wall.activeInHierarchy)
        {
            return Direction.North;
        }
        else if (!e_wall.activeInHierarchy)
        {
            return Direction.West;
        }
        else 
        {
            return Direction.East;
        }

    }

    public void DeactivateCell()
    {
        if(reactivatable == null)
        {
            reactivatable = new List<GameObject>();
        }
        
        floor.SetActive(false);
        reactivatable.Add(floor);

        if(n_wall.activeSelf)
        {
            n_wall.SetActive(false);
            reactivatable.Add(n_wall);
        }
        if(s_wall.activeSelf)
        {
            s_wall.SetActive(false);
            reactivatable.Add(s_wall);
        }
        if(e_wall.activeSelf)
        {
            e_wall.SetActive(false);
            reactivatable.Add(e_wall);
        }
        if(w_wall.activeSelf)
        {
            w_wall.SetActive(false);
            reactivatable.Add(w_wall);
        }

    }

    public void ReActivateCell()
    {
        foreach(GameObject obj in reactivatable)
        {
            obj.SetActive(true);
        }
    }

    public void DeactivateWall(Direction dir)
    {
        switch(dir)
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
    public void ActivateWall(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                n_wall.SetActive(true);
                break;
            case Direction.South:
                s_wall.SetActive(true);
                break;
            case Direction.East:
                e_wall.SetActive(true);
                break;
            case Direction.West:
                w_wall.SetActive(true);
                break;
        }
    }

    public bool DeadEndCentered(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return n_wall.activeInHierarchy && !s_wall.activeInHierarchy;
            case Direction.South:
                return s_wall.activeInHierarchy && !n_wall.activeInHierarchy;
            case Direction.East:
                return e_wall.activeInHierarchy && !w_wall.activeInHierarchy;
            case Direction.West:
                return w_wall.activeInHierarchy && !e_wall.activeInHierarchy;
        }

        return false;
    }
}
