using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarController : MonoBehaviour
{
    public Image normal_star;
    public Image disabled_star;
    public bool disabled = true;

	// Use this for initialization
	void Start ()
    {
        normal_star.enabled = false;
        disabled_star.enabled = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ActiveStar(bool active)
    {
        normal_star.enabled = active;
        disabled = !active;
    }
}
