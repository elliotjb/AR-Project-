using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerObjective : MonoBehaviour
{
    public List<StarController> stars;
    public List<GameObject> objectives;

    public GameObject canvas;
    private bool finished = false;

    public string next_level;

    public float time_to_complete = 2.0f;
    float time = 0.0f;

    AudioSource audio;

	// Use this for initialization
	void Start ()
    {
        canvas.SetActive(false);
        audio = GetComponent<AudioSource>();
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
        if (num_activated == objectives.Count && finished == false)
        {
            if(time >= time_to_complete)
            {
                canvas.SetActive(true);
                finished = true;
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
        if (finished == false)
        {
            if (audio != null)
            {
                audio.Play();
            }
            for (int i = 0; i < stars.Count; i++)
            {
                if (stars[i].disabled == true && finished == false)
                {
                    stars[i].ActiveStar(true);
                    break;
                }
            }
        }
    }

    public void DeactiveStar()
    {
        if (finished == false)
        {
            for (int i = stars.Count - 1; i >= 0; i--)
            {
                if (stars[i].disabled == false && finished == false)
                {
                    stars[i].ActiveStar(false);
                    break;
                }
            }
        }
    }

    public void GoNextLevel()
    {
        SceneManager.LoadScene(next_level);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
