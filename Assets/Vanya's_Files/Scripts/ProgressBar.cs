using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progress;
    public float procents;
    // Start is called before the first frame update
    void Start()
    {
         progress = progress.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        progress.fillAmount = procents;
    }
}
