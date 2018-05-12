using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public Material color_red;
    public Material color_white;
    float time = 0.0f;
    public float time_progresive = 1.0f;
    bool hit = false;

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
                    gameObject.GetComponent<Renderer>().material = color_red;
                }
            }
            else
            {
                time = 0.0f;
                gameObject.GetComponent<Renderer>().material = color_white;
            }
        }
        else if (mode == ObjectiveMode.DIRECT)
        {
            if (hit)
            {
                gameObject.GetComponent<Renderer>().material = color_red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material = color_white;
            }
        }
        hit = false;
    }

    public void HitLaser()
    {
        hit = true;
    }

}
