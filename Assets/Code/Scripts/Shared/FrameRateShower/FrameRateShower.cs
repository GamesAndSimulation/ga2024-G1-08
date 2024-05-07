using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class FrameRateShower : MonoBehaviour
{

    public float textRefreshRate = 0.1f;

    public TextMeshProUGUI text;
    private float currentFrameRate = 0;

    private void OnEnable() {
        StartCoroutine(refreshFrameRateText());
    }

    // Update is called once per frame
    void Update()
    {

        currentFrameRate = Mathf.Round(1 / Time.deltaTime);
        
    }

    IEnumerator refreshFrameRateText() {

        while (this.isActiveAndEnabled) {

            text.text = currentFrameRate.ToString();

            yield return new WaitForSeconds(textRefreshRate);

        }

    }

}
