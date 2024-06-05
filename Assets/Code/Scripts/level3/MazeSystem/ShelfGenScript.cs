using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfGenScript : MonoBehaviour, Decoration
{

    [SerializeField] private List<GameObject> positions;
    [SerializeField] private List<GameObject> items;

    [SerializeField] private Vector3 basePosition;
    [SerializeField]private Vector3 baseRotation;

    public void GenerateShelves()
    {
        for (int i = 0; i < positions.Count; i++)
        {

            if (positions[i].transform.childCount > 0)
                DestroyImmediate(positions[i].transform.GetChild(0).gameObject);

            Instantiate(items[Random.Range(0,items.Count)], positions[i].transform);
        }
    }

    public void GenObject(Direction dir)
    {
        transform.localRotation = Quaternion.Euler(baseRotation);
        transform.localPosition = basePosition;
        switch (dir)
        {
            case Direction.North:
                transform.localPosition = basePosition;
                break;
            case Direction.South:
                transform.localPosition = new Vector3(basePosition.x, -basePosition.y, basePosition.z);
                break;
            case Direction.East:
                transform.localPosition = new Vector3(basePosition.x, -basePosition.y, basePosition.z);
                break;
            case Direction.West:
                transform.localPosition = basePosition;
                break;
        }
        GenerateShelves();
    }

}
