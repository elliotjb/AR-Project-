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
    OBJECTIVE,

    PORTAL,
    PRISM
}

public enum DirectionType
{
    NORMAL = 0,
    LEFT,
    RIGHT
}

public class LaserClass
{
	public LineRenderer laser;
	public bool active = false;
    public DirectionType direction = DirectionType.NORMAL;
    public ColorType color = ColorType.RED;
    public ParticleSystem particles_collision;

    public ElementType type_hit = ElementType.NONE;
}

public class Directional
{
    public Vector3 this[DirectionType type]
    {
        get
        {
            if (type == DirectionType.NORMAL)
            {
                return direction_normal;
            }
            if (type == DirectionType.LEFT)
            {
                return direction_left;
            }
            if (type == DirectionType.RIGHT)
            {
                return direction_right;
            }
            return Vector3.zero;
        }
        set
        {
            if (type == DirectionType.NORMAL)
            {
                direction_normal = value;
            }
            if (type == DirectionType.LEFT)
            {
                direction_left = value;
            }
            if (type == DirectionType.RIGHT)
            {
                direction_right = value;
            }
        }
    }

    public Vector3 direction_normal = Vector3.zero;
    public Vector3 direction_left = Vector3.zero;
    public Vector3 direction_right = Vector3.zero;
}

public class LastPosition
{

    public Vector3 this[DirectionType type]
    {
        get
        {
            if (type == DirectionType.NORMAL)
            {
                return lastposition_normal;
            }
            if (type == DirectionType.LEFT)
            {
                return lastposition_left;
            }
            if (type == DirectionType.RIGHT)
            {
                return lastposition_right;
            }
            return Vector3.zero;
        }
        set
        {
            if (type == DirectionType.NORMAL)
            {
                lastposition_normal = value;
            }
            if (type == DirectionType.LEFT)
            {
                lastposition_left = value;
            }
            if (type == DirectionType.RIGHT)
            {
                lastposition_right = value;
            }
        }
    }

    public Vector3 lastposition_normal = Vector3.zero;
    public Vector3 lastposition_left = Vector3.zero;
    public Vector3 lastposition_right = Vector3.zero;
}

public class Laserv3 : MonoBehaviour
{
	public LineRenderer laser_temp;
	public int max_lines = 20;
	public int max_distance = 100;

    //Test
    public float angle = 0.0f;
    public float delta_angle = 0.0f;

	List<LaserClass> lasers;

    //MATERIALS TO APPLY COLOR
    public Material red;
    public Material blue;

    public ParticleSystem particles;

    Directional direction;
    LastPosition lastposition;


    void Start ()
    {
		lasers = new List<LaserClass> (10);
        direction = new Directional();
        lastposition = new LastPosition();

        for (int i = 0; i < 10; i++) 
		{
			LaserClass new_laser = new LaserClass();
			new_laser.laser = Instantiate (laser_temp);
            new_laser.particles_collision = Instantiate(particles);
            new_laser.particles_collision.Stop();
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
        lastposition.lastposition_normal = transform.position;
        direction.direction_normal = transform.forward;
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
				bool hit_do = Physics.Raycast(lastposition[lasers[i].direction], direction[lasers[i].direction], out ray_hit, max_distance);
                ElementType mode = SelectMode (ray_hit);

                //MODIFY LASER DEPENDING ON THE COLLIDED OBJECT ----------------------------------------------------

				if (mode == ElementType.MIRROR)
                {
                    Mirror(ref count_hits, ref div_hits, ray_hit, lastposition, direction, i);             
                } 

				else if (mode == ElementType.RECEIVER)
                {
                    Receiver(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, i);
				} 

				else if(mode == ElementType.OBSTACLE)
                {
                    Obstacle(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, i);
				}

				else if(mode > ElementType.FIRST_COLOR && mode < ElementType.LAST_COLOR)
                {
                    ChangeColor(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, mode, i);
				}

                else if (mode == ElementType.PORTAL)
                {
                    Portal(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, i);
                }

                else if (mode == ElementType.PRISM)
                {
                    if (i + 1 < lasers.Count && i + 2 < lasers.Count)
                    {
                        Prism(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, i);
                    }
                    else
                    {
                        Obstacle(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, i);
                    }
                }

                else if (mode == ElementType.OBJECTIVE)
                {
                    Objective(ref count_hits, ref div_hits, ray_hit, lastposition, direction, ref hit, i);
                }
                // --------------------------------------------------------------------------------------------------------------

                //END OF LASER, LAST LINE
                else
				{
                    count_hits++;
                    div_hits++;
                    lasers[i].laser.positionCount = div_hits;
					lasers[i].laser.SetPosition (div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * max_distance));
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
            else if (hit.transform.tag.Equals("Prism"))
            {
                return ElementType.PRISM;
            }
            else if (hit.transform.tag.Equals("ColorBlue"))
            {
                return ElementType.COLOR_CHANGER_BLUE;
            }
            else if (hit.transform.tag.Equals("ColorRed"))
            {
                return ElementType.COLOR_CHANGER_RED;
            }
            else if (hit.transform.tag.Equals("Objective"))
            {
                return ElementType.OBJECTIVE;
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
            lasers[i].direction = DirectionType.NORMAL;
            lasers[i].color = ColorType.RED;

            if(lasers[i].type_hit != ElementType.OBSTACLE && lasers[i].type_hit != ElementType.OBJECTIVE)
            {
                lasers[i].particles_collision.Stop();
            }
            lasers[i].type_hit = ElementType.NONE;
        }
        lasers[0].active = true;
    }


    //LASER MODIFIER FUNCTIONS -----------------

    //Reflect laser depending on the mirror rotation
    void Mirror(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, int i)
    {
        count_hits++;
        div_hits += 3;
        lasers[i].laser.positionCount = div_hits;
        lasers[i].laser.SetPosition(div_hits - 3, Vector3.MoveTowards(ray_hit.point, lastposition[lasers[i].direction], 0.01f));
        lasers[i].laser.SetPosition(div_hits - 2, ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, ray_hit.point);
        lastposition[lasers[i].direction] = ray_hit.point;
        direction[lasers[i].direction] = Vector3.Reflect(direction[lasers[i].direction], ray_hit.normal);

        lasers[i].type_hit = ElementType.MIRROR;
    }

    //Receive laser to Do Something...
    void Receiver(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, ref bool hit, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastposition[lasers[i].direction], ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance));

        // Do Something...

        lasers[i].type_hit = ElementType.RECEIVER;
        hit = false;
    }

    //Stop the laser line at the hit point
    void Obstacle(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, ref bool hit, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastposition[lasers[i].direction], ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance));

        if (!lasers[i].particles_collision.isEmitting)
        {
            lasers[i].particles_collision.Play();
        }
        lasers[i].particles_collision.transform.position = lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance);
        lasers[i].particles_collision.transform.rotation = Quaternion.LookRotation(ray_hit.normal);

        lasers[i].type_hit = ElementType.OBSTACLE;
        hit = false;
    }

    //Change color of the laser: BLUE
    void ChangeColor(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, ref bool hit, ElementType mode, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastposition[lasers[i].direction], ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance));

        // Do Something...

        //Set the laser color
        int next_laser = GetAvailableLaser();
        SetColor(mode, next_laser);
        lasers[next_laser].direction = lasers[i].direction;
        lasers[next_laser].active = true;
        lasers[next_laser].laser.positionCount = 1;
        lastposition[lasers[next_laser].direction] = ray_hit.point + (direction[lasers[i].direction].normalized * 0.4261f); //0.4261 = Element width
        lasers[next_laser].laser.SetPosition(0, lastposition[lasers[next_laser].direction]);

        hit = false;
    }

    //Set the color of the laser depending on its previously compared tag
    void SetColor(ElementType mode, int next_laser)
    {
        if (mode == ElementType.COLOR_CHANGER_BLUE) 
        {
            lasers[next_laser].laser.material = blue;
            lasers[next_laser].color = ColorType.BLUE;
        }
        else if (mode == ElementType.COLOR_CHANGER_RED)
        {
            lasers[next_laser].laser.material = red;
            lasers[next_laser].color = ColorType.RED;
        }
    }

    //Set the laser at the point of the linked portal
    void Portal(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, ref bool hit, int i)
    {
        Portal portal = ray_hit.transform.GetComponent<Portal>();

        count_hits++;
        div_hits++;

        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastposition[lasers[i].direction], ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance));

        int next_laser = GetAvailableLaser();
        lasers[next_laser].active = true;
        lasers[next_laser].direction = lasers[i].direction;
        lasers[next_laser].laser.positionCount = 1;

        Vector3 local_hit_point = Vector3.zero;
        local_hit_point = ray_hit.transform.InverseTransformPoint(ray_hit.point);
        Vector3 other_hit_point = portal.linked_portal.transform.TransformPoint(local_hit_point);
        lastposition[lasers[next_laser].direction] = other_hit_point - portal.GetLinkedDirection().normalized * 0.2f;

        lasers[next_laser].laser.SetPosition(0, lastposition[lasers[next_laser].direction]);
        angle = Vector3.Angle(direction[lasers[i].direction], portal.transform.forward);
        Vector3 cross = Vector3.Cross(direction[lasers[i].direction], portal.transform.forward);
        if (cross.y < 0) 
        {
            angle = -angle;
        }
        direction[lasers[i].direction] = Quaternion.AngleAxis(-angle, portal.transform.up) * -portal.GetLinkedDirection();

        //Set the correct color
        lasers[next_laser].laser.material = lasers[i].laser.material;        

        hit = false;
    }


    void Prism(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, ref bool hit, int i)
    {
        Prism prism = ray_hit.transform.GetComponent<Prism>();

        count_hits+=2;
        div_hits++;

        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastposition[lasers[i].direction], ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance));



        int next_laser = GetAvailableLaser();

        //Set the correct color
        lasers[next_laser].laser.material = lasers[i].laser.material;
        lasers[next_laser].direction = DirectionType.LEFT;

        //LASER LEFT
        lasers[next_laser].active = true;
        lasers[next_laser].laser.positionCount = 1;

        Vector3 local_hit_point = Vector3.zero;
        local_hit_point = ray_hit.transform.InverseTransformPoint(ray_hit.point);
        Vector3 other_hit_point = prism.other_face_1.transform.TransformPoint(local_hit_point);
        lastposition[lasers[next_laser].direction] = other_hit_point + prism.GetFace1Direction().normalized * 0.12f;
        lasers[next_laser].laser.SetPosition(0, lastposition[lasers[next_laser].direction]);
        direction[lasers[next_laser].direction] = prism.GetFace1Direction();

        ////LASER RIGHT
        next_laser = GetAvailableLaser();

        //Set the correct color
        lasers[next_laser].laser.material = lasers[i].laser.material;
        lasers[next_laser].direction = DirectionType.RIGHT;

        lasers[next_laser].active = true;
        lasers[next_laser].laser.positionCount = 1;
        local_hit_point = Vector3.zero;
        local_hit_point = ray_hit.transform.InverseTransformPoint(ray_hit.point);
        other_hit_point = prism.other_face_2.transform.TransformPoint(local_hit_point);
        lastposition[lasers[next_laser].direction] = other_hit_point + prism.GetFace2Direction().normalized * 0.12f;
        lasers[next_laser].laser.SetPosition(0, lastposition[lasers[next_laser].direction]);
        direction[lasers[next_laser].direction] = prism.GetFace2Direction();

        hit = false;
    }

    int GetAvailableLaser()
    {
        for (int i = 0; i < lasers.Count; i++) 
        {
            if (lasers[i].active == false)
            {
                return i;
            }
        }

        return 0;
    }

    void Objective(ref int count_hits, ref int div_hits, RaycastHit ray_hit, LastPosition lastposition, Directional direction, ref bool hit, int i)
    {
        count_hits++;
        div_hits++;
        lasers[i].laser.positionCount = div_hits;
        float distance = Vector3.Distance(lastposition[lasers[i].direction], ray_hit.point);
        lasers[i].laser.SetPosition(div_hits - 1, lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance));
        hit = false;
        ray_hit.collider.gameObject.GetComponent<Objective>().HitLaser(lasers[i].color);

        if (!lasers[i].particles_collision.isEmitting)
        {
            lasers[i].particles_collision.Play();
        }
        lasers[i].particles_collision.transform.position = lastposition[lasers[i].direction] + (direction[lasers[i].direction].normalized * distance);
        lasers[i].particles_collision.transform.rotation = Quaternion.LookRotation(ray_hit.normal);
        lasers[i].type_hit = ElementType.OBJECTIVE;
    }

    // -----------------------------------------

}