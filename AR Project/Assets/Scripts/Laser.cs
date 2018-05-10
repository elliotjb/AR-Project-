using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers
{
    public GameObject follow;
    public GameObject laser;
    public bool active;
}

public class Laser : MonoBehaviour
{

   // public LineRenderer laser;

    public Transform gun;
    public GameObject prefab_follow;
    public GameObject prefab_laser;

    public bool stop = false;
    public float speed;

    public List<Lasers> lasers;

    // Use this for initialization
    void Start () {
        lasers = new List<Lasers>();
        GameObject temp = Instantiate(prefab_follow) as GameObject;
        temp.transform.position = gun.transform.position;
        temp.transform.rotation = gun.transform.rotation;
        Lasers temp2 = new Lasers();
        temp2.follow = temp;
        temp = Instantiate(prefab_laser) as GameObject;
        temp.transform.position = gun.transform.position;
        temp.transform.rotation = gun.transform.rotation;
        temp.GetComponent<LineRenderer>().SetPosition(0, gun.position);
        temp.GetComponent<LineRenderer>().SetPosition(1, temp2.follow.transform.position);
        temp.GetComponent<LineRenderer>().startWidth = 0.45f;
        temp.GetComponent<LineRenderer>().endWidth = 0.45f;
        temp2.laser = temp;
        temp2.active = true;
        lasers.Add(temp2);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
        if(!stop)
        {
            float step = speed * Time.deltaTime;
            for (int i = 0; i< lasers.Count; i++)
            {
                if(lasers[i].active)
                {
                    lasers[i].follow.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    lasers[i].laser.GetComponent<LineRenderer>().SetPosition(1, lasers[i].follow.transform.position);
                    //lasers[i].follow.transform.position = Vector3.MoveTowards(lasers[i].follow.transform.position, gun.position, step);
                }
            }
        }
	}

    public void CreateNewLaser(Transform collision, Vector3 glass)
    {
        GameObject temp = Instantiate(prefab_follow) as GameObject;
        temp.transform.position = collision.transform.position;

        temp.transform.rotation = Quaternion.FromToRotation(Vector3.forward, glass);
        Lasers temp2 = new Lasers();
        temp2.follow = temp;
        temp = Instantiate(prefab_laser) as GameObject;
        temp.transform.position = collision.transform.position;
        temp.transform.rotation = collision.transform.rotation;
        temp.GetComponent<LineRenderer>().SetPosition(0, collision.position);
        temp.GetComponent<LineRenderer>().SetPosition(1, temp2.follow.transform.position);
        temp.GetComponent<LineRenderer>().startWidth = 0.45f;
        temp.GetComponent<LineRenderer>().endWidth = 0.45f;
        temp2.laser = temp;
        temp2.active = true;
        lasers.Add(temp2);
    }

    public void DesactivateLaser(GameObject col)
    {
        for (int i = 0; i < lasers.Count; i++)
        {
            if (lasers[i].follow == col)
            {
                lasers[i].active = false;
                lasers[i].follow.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
