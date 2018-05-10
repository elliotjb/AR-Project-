using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserClass
{
	public LineRenderer laser;
	public bool active = false;
}

public class Laserv3 : MonoBehaviour {


	public LineRenderer laser_temp;
	public int max_lines = 10;
	public int max_distance = 100;

	//int layer_mirror = 1<<8;
	//int layer_obstacle = 1<<9;
	//int layer_reciver = 1<<10;
	//int layer_colorblue = 1<<11;


	List<LaserClass> lasers;

    public Material red;
    public Material blue;

    //public LaserClass this[int index]
    //{
    //    get
    //    {
    //        if (index >= 0 && index < lasers.Count)
    //            return lasers[index];
    //        return null;
    //    }
    //}

    // Use this for initialization
    void Start () {
		lasers = new List<LaserClass> (10);

		for (int i = 0; i < 10; i++) 
		{
			LaserClass new_laser = new LaserClass();
			new_laser.laser = Instantiate (laser_temp);
			lasers.Add(new_laser);
		}
		lasers [0].active = true;
	}

	// Update is called once per frame
	void Update () 
	{
        ResetValues();
        DoLaser ();
    }


	void DoLaser()
	{

		Vector3 lastLaserPosition = transform.position;
		Vector3 direction = transform.up;

        lasers[0].laser.positionCount = 1;
		lasers [0].laser.SetPosition(0, transform.position);



		for (int i = 0; i < lasers.Count; i++) 
		{
            int count_hits = 1;
            int div_hits = 1;

            bool hit = true;
			if (lasers [i].active == false) {
				continue;
			}

			while (hit)
			{
				RaycastHit ray_hit;
				bool hit_do = Physics.Raycast(lastLaserPosition, direction, out ray_hit, max_distance);
				int mode = SelectMode (ray_hit);
				if (mode == 1) {
					count_hits++;
					div_hits += 3;
					lasers [i].laser.positionCount = div_hits;
					lasers [i].laser.SetPosition (div_hits - 3, Vector3.MoveTowards (ray_hit.point, lastLaserPosition, 0.01f));
					lasers [i].laser.SetPosition (div_hits - 2, ray_hit.point);
					lasers [i].laser.SetPosition (div_hits - 1, ray_hit.point);
					lastLaserPosition = ray_hit.point;
					direction = Vector3.Reflect (direction, ray_hit.normal);
				} 
				else if (mode == 2) {
					div_hits++;
					count_hits++;
					lasers [i].laser.positionCount = div_hits;
					float distance = Vector3.Distance (lastLaserPosition, ray_hit.point);
					lasers [i].laser.SetPosition (div_hits - 1, lastLaserPosition + (direction.normalized * distance));

                    // Do Something....


                    hit = false;
				} 
				else if(mode == 3){
					div_hits++;
					count_hits++;
					lasers [i].laser.positionCount = div_hits;

                    float distance = Vector3.Distance (lastLaserPosition, ray_hit.point);
					lasers [i].laser.SetPosition (div_hits - 1, lastLaserPosition + (direction.normalized * distance));
					hit = false;
				}
				else if(mode == 4){
					div_hits++;
					count_hits++;
					lasers [i].laser.positionCount = div_hits;
					float distance = Vector3.Distance (lastLaserPosition, ray_hit.point);
					lasers [i].laser.SetPosition (div_hits - 1, lastLaserPosition + (direction.normalized * distance));

                    // Do Something....

                    lasers[i + 1].laser.material = blue;
                    lasers [i + 1].active = true;
					lasers [i + 1].laser.positionCount = 1;
					lastLaserPosition = ray_hit.point + (direction.normalized * 0.4261f);
                    lasers [i + 1].laser.SetPosition(0, lastLaserPosition);


					hit = false;
				}
				else
				{
					div_hits++;
					count_hits++;
					lasers [i].laser.positionCount = div_hits;
					lasers [i].laser.SetPosition (div_hits - 1, lastLaserPosition + (direction.normalized * max_distance));
					hit = false;
				}

				if (count_hits > max_lines)
				{
					hit = false;
				}
			}
		}
	}


	int SelectMode(bool hit_do, bool hit_reciver, bool hit_obstacle, bool hit_colorblue)
	{
		if (hit_do) 
		{
			return 1;
		} 
		else if (hit_reciver)
		{
			return 2;
		}
		else if (hit_obstacle)
		{
			return 3;
		}
		else if (hit_colorblue)
		{
			return 4;
		}
		return 0;
	}

    int SelectMode(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            if (hit.transform.tag.Equals("Mirror"))
            {
                return 1;
            }
            else if (hit.transform.tag.Equals("Reciver"))
            {
                return 2;
            }
            else if (hit.transform.tag.Equals("Obstacle"))
            {
                return 3;
            }
            else if (hit.transform.tag.Equals("ColorBlue"))
            {
                return 4;
            }
        }
        return 0;
    }

    void ResetValues()
    {
        for (int i = 0; i < lasers.Count; i++)
        {
            lasers[i].active = false;
            lasers[i].laser.material = red;
            lasers[i].laser.positionCount = 0;
        }
        lasers[0].active = true;
    }

}