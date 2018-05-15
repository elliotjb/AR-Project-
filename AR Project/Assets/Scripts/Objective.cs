using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    WHITE = 0,
    BLUE,
    RED
}

public enum StateType
{
    DESACTIVATED = 0,
    ACTIVATED
}

public class Objective : MonoBehaviour
{
    ManagerObjective level_manager = null;
    public Material color_red;
    public Material color_blue;
    public Material color_blue_dark;
    public Material color_red_dark;
    float time = 0.0f;
    public float time_progresive = 1.0f;
    bool hit = false;
    public ColorType light_color = ColorType.RED;
    public ColorType actualColor = ColorType.WHITE;
    public ColorType reset_color = ColorType.WHITE;

    public StateType state = StateType.DESACTIVATED;

    bool active = false;

    public enum ObjectiveMode
    {
        NONE = -1,
        PROGRESIVE,
        DIRECT,
        SPECIAL
    }

    public ObjectiveMode mode;

    // Use this for initialization
    void Start()
    {
        active = false;
        GameObject level = GameObject.Find("LevelManager");
        if (level != null)
        {
            level_manager = level.GetComponent<ManagerObjective>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (mode == ObjectiveMode.PROGRESIVE)
        {
            if(hit)
            {
                if (time > time_progresive && state == StateType.DESACTIVATED)
                {
                    ChangeColor(actualColor);
                    state = StateType.ACTIVATED;
                    if(level_manager != null)
                        level_manager.ActiveStar();
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
            else
            {
                time = 0.0f;
                ChangeColor(reset_color);
                state = StateType.DESACTIVATED;
                if (level_manager != null)
                    level_manager.DeactiveStar();
            }
        }
        else if (mode == ObjectiveMode.DIRECT)
        {
            if (hit && state == StateType.DESACTIVATED)
            {
                ChangeColor(actualColor);
                state = StateType.ACTIVATED;
                if (level_manager != null)
                    level_manager.ActiveStar();
            }
            else if(hit == false && state == StateType.ACTIVATED)
            {
                ChangeColor(reset_color);
                state = StateType.DESACTIVATED;
                if (level_manager != null)
                    level_manager.DeactiveStar();
            }
        }
        else if (mode == ObjectiveMode.SPECIAL)
        {
            if (hit)
            {
                if (time > time_progresive && state == StateType.DESACTIVATED)
                {
                    ChangeColor(actualColor);
                    state = StateType.ACTIVATED;
                    if (level_manager != null)
                        level_manager.ActiveStar();
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
            else
            {
                time -= Time.deltaTime;
                if (time <= 0.0f)
                {
                    time = 0.0f;
                    ChangeColor(reset_color);
                    state = StateType.DESACTIVATED;
                    if (level_manager != null)
                        level_manager.DeactiveStar();
                }
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
                    if (light_color == ColorType.RED)
                    {
                        gameObject.GetComponent<Renderer>().material = color_red_dark;
                    }
                    else if (light_color == ColorType.BLUE)
                    {
                        gameObject.GetComponent<Renderer>().material = color_blue_dark;
                    }
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
        //Only light if the objective color is the same of the laser
        if (light_color == color)
        {
            actualColor = color;
            hit = true;
        }
    }

}
