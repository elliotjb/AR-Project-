using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    WHITE = 0,
    BLUE,
    RED
}

public class Objective : MonoBehaviour
{
    public Material color_red;
    public Material color_blue;
    public Material color_white;
    float time = 0.0f;
    public float time_progresive = 1.0f;
    bool hit = false;
    public ColorType actualColor = ColorType.WHITE;
    public ColorType reset_color = ColorType.WHITE;

    public enum ObjectiveMode
    {
        NONE = -1,
        PROGRESIVE,
        DIRECT
    }

    public ObjectiveMode mode;

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (mode == ObjectiveMode.PROGRESIVE)
        {
            if(hit)
            {
                time += Time.deltaTime;
                if (time > time_progresive)
                {
                    ChangeColor(actualColor);
                }
            }
            else
            {
                time = 0.0f;
                ChangeColor(reset_color);
            }
        }
        else if (mode == ObjectiveMode.DIRECT)
        {
            if (hit)
            {
                ChangeColor(actualColor);
            }
            else
            {
                ChangeColor(reset_color);
            }
        }
        hit = false;
    }

    void ChangeColor(ColorType color)
    {
        switch(color)
        {
            case ColorType.WHITE:
                {
                    gameObject.GetComponent<Renderer>().material = color_white;
                    break;
                }
            case ColorType.RED:
                {
                    gameObject.GetComponent<Renderer>().material = color_red;
                    break;
                }
            case ColorType.BLUE:
                {
                    gameObject.GetComponent<Renderer>().material = color_blue;
                    break;
                }
        }
    }

    public void HitLaser(ColorType color)
    {
        actualColor = color;
        hit = true;
    }

}
