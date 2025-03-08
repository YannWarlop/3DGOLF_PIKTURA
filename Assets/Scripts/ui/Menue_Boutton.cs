using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menue_Boutton : MonoBehaviour
{
    public Image ImageOutline;

    public void OnpointerEnter()
    {
        Color color = ImageOutline.color;
        color.a = 1;
        ImageOutline.color = color;
    }
    void Start()


    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

