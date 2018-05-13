using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerObjective : MonoBehaviour {


    public List<GameObject> objectives;
    public string next_level;

    public float time_to_complet = 2.0f;
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
            if(time >= time_to_complet)
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
}
