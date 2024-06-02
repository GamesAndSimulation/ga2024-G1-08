using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [Header("Walls and Floor")]
    [SerializeField] private GameObject n_wall;
    [SerializeField] private GameObject s_wall;
    [SerializeField] private GameObject e_wall;
    [SerializeField] private GameObject w_wall;
    [SerializeField] private GameObject floor;

    [Header("Wall Decors")]
    [SerializeField] private GameObject n_wall_decor;
    [SerializeField] private GameObject s_wall_decor;
    [SerializeField] private GameObject e_wall_decor;
    [SerializeField] private GameObject w_wall_decor;

    [Header("Floor Decors")]
    [SerializeField] private GameObject m_decor;
    [SerializeField] private GameObject n_decor;
    [SerializeField] private GameObject s_decor;
    [SerializeField] private GameObject e_decor;
    [SerializeField] private GameObject w_decor;

    [Header("Size Vars")]
    [SerializeField] public int width;
    [SerializeField] public int height;

    [Header("Theme")]
    [SerializeField] private MazeTheme theme;

    private GameObject setDecor;

    public bool visited = false;
    public bool hasGeneratedDecor = false;
    public int nWalls;

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
                dir = (Direction)Random.Range(0, 4);

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
            if (decorPlace != null && decors.Count > 0 && Random.Range(0,3) > 0) // 2/3 chance to spawn a standing decoration
            {
                int index = Random.Range(0, decors.Count);
                   
                GameObject decor = Instantiate(decors.ToArray()[index] , decorPlace.transform.position, Quaternion.identity, decorPlace.transform);

                decor.GetComponent<Decoration>().GenObject(dir);

                setDecor = decor;
            }
            else if(wallDecorPlace != null && wallDecors.Count > 0){

                int index = Random.Range(0, wallDecors.Count);

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

    public PortalDecor CreatePortal(GameObject portal)
    {
        DeleteDecor();
        Transform trans = null;

        if(this.transform.position != new Vector3(0,0,0)) // If not the first cell
            trans = DeadEndWall().transform;
        else
            trans = s_wall.transform;

        if (trans != null)
        {
            GameObject portalObj = Instantiate(portal, n_decor.transform.position, Quaternion.identity, trans.transform);
            setDecor = portalObj;
            hasGeneratedDecor = true;
            portalObj.GetComponent<PortalDecor>().GenObject(Direction.South);//direction doesn't actually matter for portals
            return portalObj.GetComponent<PortalDecor>();
        }

        return null;
    }

    private GameObject DeadEndWall()
    {
        if(!n_wall.activeInHierarchy)
        {
            return s_decor;
        }
        else if (!s_wall.activeInHierarchy)
        {
            return n_decor;
        }
        else if (!e_wall.activeInHierarchy)
        {
            return w_decor;
        }
        else if (!w_wall.activeInHierarchy)
        {
            return e_decor;
        }

        return null;
    }
}
