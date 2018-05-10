using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    NONE = -1,
    MIRROR,
    RECEIVER,
    OBSTACLE,

    //Encapsulate colors between FIRST_COLOR & LAST_COLOR
    FIRST_COLOR, 
    COLOR_CHANGER_BLUE,
    COLOR_CHANGER_RED,
    LAST_COLOR,
    // --------------------------------------------------

    PORTAL
}
            
public class LaserClass
{
	public LineRenderer laser;
	public bool active = false;
}

public class Laserv3 : MonoBehaviour
{
	public LineRenderer laser_temp;
	public int max_lines = 10;
	public int max_distance = 100;

	List<LaserClass> lasers;

    //MATERIALS TO APPLY COLOR
    public Material red;
    public Material blue;


    void Start ()
    {
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
		lasers[0].laser.SetPosition(0, transform.position);

		for (int i = 0; i < lasers.Count; i++) 
		{
            int count_hits = 1;
            int div_hits = 1;

            bool hit = true;
			if (lasers[i].active == false)
            {
				continue;
			}

			while (hit)
			{
				RaycastHit ray_hit;
				bool hit_do = Physics.Raycast(lastLaserPosition, direction, out ray_hit, max_distance);
                ElementType mode = SelectMode (ray_hit);

                //MODIFY LASER DEPENDING ON THE COLLIDED OBJECT ----------------------------------------------------

				if (mode == ElementType.MIRROR)
                {
                    Mirror(ref count_hits, ref div_hits, ref ray_hit, ref lastLaserPosition, ref direction, i);             
                } 

				else if (mode == ElementType.RECEIVER)
                {
                    Receiver(ref count_hits, ref div_hits, ray_hit.point, lastLaserPosition, direction, ref hit, i);
				} 

				else if(mode == ElementType.OBSTACLE)
                {
                    Obstacle(ref count_hits, ref div_hits, ray_hit.point, lastLaserPosition, direction, ref hit, i);
				}

				else if(mode > ElementType.FIRST_COLOR && mode < ElementType.LAST_COLOR)
                {
                    ChangeColor(ref count_hits, ref div_hits, ray_hit.point, ref lastLaserPosition, direction, ref hit, mode, i);
				}

                else if (mode == ElementType.PORTAL)
                {
                    Portal(ref count_hits, ref div_hits, ray_hit, ref lastLaserPosition, ref direction, ref hit, i);
                }

                // --------------------------------------------------------------------------------------------------------------

                //END OF LASER, LAST LINE
                else
				{
                    count_hits++;
                    div_hits++;
                    lasers[i].laser.positionCount = div_hits;
					lasers[i].laser.SetPosition (div_hits - 1, lastLaserPosition + (direction.normalized * max_distance));
					hit = false;
				}

				if (count_hits > max_lines)
				{
					hit = false;
				}
			}
		}
	}

    ElementType SelectMode(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            if (hit.transform.tag.Equals("Mirror"))
            {
                return ElementType.MIRROR;
            }
            else if (hit.transform.tag.Equals("Receiver"))
            {
                return ElementType.RECEIVER;
            }
            else if (hit.transform.tag.Equals("Obstacle"))
            {
                return ElementType.OBSTACLE;
            }
            else if (hit.transform.tag.Equals("Portal"))
            {
                return ElementType.PORTAL;
            }
            else if (hit.transform.tag.Equals("ColorBlue"))
            {
                return ElementType.COLOR_CHANGER_BLUE;
            }
            else if (hit.transform.tag.Equals("ColorRed"))
            {
                return ElementType.COLOR_CHANGER_RED;
            }
        }
        return ElementType.NONE;
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


    //LASER MODIFIER FUNCTIONS -----------------

    //Reflect laser depending on the mirror rotation
    void Mirror(ref int count_hits, ref int div_hits, ref RaycastHit ray_hit, ref Vector3 lastLaserPosition, ref Vector3 direction, int i)
    {
        count_hits++;
        div_hits += 3;
        lasers[i].laser.positionCount = div_hits;
        lasers[i].laser.SetPosition(div_hits - 3, Vector3.MoveTowards(ray_hit.point, lastLaserPosition, 0.01f));
        lasers[i].laser.SetPosition(div_hits - 2, ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, ray_hit.point);
        lastLaserPosition = ray_hit.point;
        direction = Vector3.Reflect(direction, ray_hit.normal);
    }

    //Receive laser to Do Something...
    void Receiver(ref int count_hits, ref int div_hits, Vector3 ray_hit_point, Vector3 lastLaserPosition, Vector3 direction, ref bool hit, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastLaserPosition, ray_hit_point);
        lasers[i].laser.SetPosition(div_hits - 1, lastLaserPosition + (direction.normalized * distance));

        // Do Something...


        hit = false;
    }

    //Stop the laser line at the hit point
    void Obstacle(ref int count_hits, ref int div_hits, Vector3 ray_hit_point, Vector3 lastLaserPosition, Vector3 direction, ref bool hit, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastLaserPosition, ray_hit_point);
        lasers[i].laser.SetPosition(div_hits - 1, lastLaserPosition + (direction.normalized * distance));
        hit = false;
    }

    //Change color of the laser: BLUE
    void ChangeColor(ref int count_hits, ref int div_hits, Vector3 ray_hit_point, ref Vector3 lastLaserPosition, Vector3 direction, ref bool hit, ElementType mode, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastLaserPosition, ray_hit_point);
        lasers[i].laser.SetPosition(div_hits - 1, lastLaserPosition + (direction.normalized * distance));

        // Do Something...

        //Set the laser color
        SetColor(mode, i);

        lasers[i + 1].active = true;
        lasers[i + 1].laser.positionCount = 1;
        lastLaserPosition = ray_hit_point + (direction.normalized * 0.4261f); //0.4261 = Element width
        lasers[i + 1].laser.SetPosition(0, lastLaserPosition);


        hit = false;
    }

    //Set the color of the laser depending on its previously compared tag
    void SetColor(ElementType mode, int i)
    {
        if (mode == ElementType.COLOR_CHANGER_BLUE) 
        {
            lasers[i + 1].laser.material = blue;
        }
        else if (mode == ElementType.COLOR_CHANGER_RED)
        {
            lasers[i + 1].laser.material = red;
        }
    }

    //Set the laser at the point of the linked portal
    void Portal(ref int count_hits, ref int div_hits, RaycastHit ray_hit, ref Vector3 lastLaserPosition, ref Vector3 direction, ref bool hit, int i)
    {
        Portal linked_portal = ray_hit.transform.GetComponent<Portal>();

        count_hits++;
        div_hits++;

        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastLaserPosition, ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastLaserPosition + (direction.normalized * distance));

        // Do Something...

        lasers[i + 1].active = true;
        lasers[i + 1].laser.positionCount = 1;
        lastLaserPosition = linked_portal.GetLinkedPosition();
        lasers[i + 1].laser.SetPosition(0, lastLaserPosition);

        direction = linked_portal.GetLinkedDirection().normalized;

        hit = false;
    }

    // -----------------------------------------

}