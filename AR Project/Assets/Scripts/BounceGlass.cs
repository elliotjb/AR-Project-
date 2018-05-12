using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceGlass : MonoBehaviour {

    public GameObject manager_lasers;
    bool active = true;
    public float time_desactive = 1.0f;
    float time = 0.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(active == false)
        {
            time += Time.deltaTime;
            if(time >= time_desactive)
            {
                time = 0.0f;
                active = true;
            }
        }
		
	}

    void OnCollisionEnter(Collision collider)
    {
        //if (active)
        //{
        //    Vector3 reflect = Vector3.Reflect(collider.gameObject.transform.TransformDirection(Vector3.forward), collider.contacts[0].normal);
        //    manager_lasers.GetComponent<Laser>().CreateNewLaser(collider.transform, reflect);
        //    manager_lasers.GetComponent<Laser>().DesactivateLaser(collider.gameObject);
        //    active = false;
        //}
    }
}
