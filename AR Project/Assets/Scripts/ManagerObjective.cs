using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerObjective : MonoBehaviour
{
    public List<StarController> stars;
    public List<GameObject> objectives;
    public string next_level;

    public float time_to_complete = 2.0f;
    float time = 0.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        int num_activated = 0;
		for(int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].GetComponent<Objective>().state == StateType.ACTIVATED)
            {
                num_activated++;
            }
        }
        if (num_activated == objectives.Count)
        {
            if(time >= time_to_complete)
            {
                SceneManager.LoadScene(next_level);
            }
            else
            {
                time += Time.deltaTime;
            }
        }
        else
        {
            time = 0.0f;
        }
	}

    public void ActiveStar()
    {
        for (int i = 0; i < stars.Count; i++) 
        {
            if(stars[i].disabled == true)
            {
                stars[i].ActiveStar(true);
                break;
            }
        }
    }

    public void DeactiveStar()
    {
        for (int i = stars.Count - 1; i >= 0; i--)
        {
            if (stars[i].disabled == false)
            {
                stars[i].ActiveStar(false);
                break;
            }
        }
    }

}
