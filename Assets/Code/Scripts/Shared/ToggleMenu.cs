using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{

    public GameObject menu;

    [SerializeField] private GameEvent pauseGameEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleMenu()
    {
        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            pauseGameEvent.Raise(this, false);


        }
        else
        {
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            pauseGameEvent.Raise(this, true);
        }
    }
}
