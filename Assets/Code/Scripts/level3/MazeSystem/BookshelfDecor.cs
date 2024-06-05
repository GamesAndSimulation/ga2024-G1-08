using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfDecor : MonoBehaviour, Decoration
{

    [SerializeField] private List<GameObject> positions;
    [SerializeField] private List<GameObject> items;

    [SerializeField] private Vector3 basePosition;
    [SerializeField] private Vector3 baseRotation;

    public void GenerateShelves()
    {
        for (int i = 0; i < positions.Count; i++)
        {

            if (positions[i].transform.childCount > 0)
                DestroyImmediate(positions[i].transform.GetChild(0).gameObject);

            int item = Random.Range(0, items.Count+2);

            if(i == positions.Count - 1 && item > 1) //these items are too big for the last shelf
            {
                item = items.Count + 1;
            }

            if(item < items.Count) //some shelves will have items others will not
                Instantiate(items[item], positions[i].transform);
        }
    }

    public void GenObject(Direction dir)
    {
        transform.localRotation = Quaternion.Euler(baseRotation);
        transform.localPosition = basePosition;
        
        GenerateShelves();
    }

}
