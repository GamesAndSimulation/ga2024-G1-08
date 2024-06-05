using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDecorGen : MonoBehaviour, Decoration
{

    [SerializeField] private List<GameObject> positions;
    [SerializeField] private List<GameObject> items;

    [SerializeField] private Vector3 basePosition;
    [SerializeField] private Vector3 baseRotation;

    public void GenerateBooks()
    {
        for (int i = 0; i < positions.Count; i++)
        {

            if (positions[i].transform.childCount > 0)
                DestroyImmediate(positions[i].transform.GetChild(0).gameObject);

            int item = Random.Range(0, items.Count);

            Instantiate(items[item], positions[i].transform);
        }
    }

    public void GenObject(Direction dir)
    {
        transform.localRotation = Quaternion.Euler(baseRotation);
        transform.localPosition = basePosition;
        
        GenerateBooks();
    }

}
