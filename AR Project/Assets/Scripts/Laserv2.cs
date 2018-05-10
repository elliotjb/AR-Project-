using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserv2 : MonoBehaviour {

	public int laserDistance = 100;
	public int laserLimit = 10;
	public LineRenderer laser;

	Vector3 laserDirection;
	Vector3 lastLaserPosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		DrawLaser ();
	}

	void DrawLaser () {

		int laserReflected = 1;
		int vertexCounter = 1;
		bool loopActive = true;
		laserDirection = transform.up;
		lastLaserPosition = transform.position;

		laser.SetVertexCount(1);
		laser.SetPosition(0, transform.position);

		while (loopActive) {
			RaycastHit hit;
			bool hit_do = Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance);

			if (hit_do) 
			{
				laserReflected++;
				vertexCounter += 2;
				laser.SetVertexCount (vertexCounter);
				laser.SetPosition (vertexCounter-2, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
				laser.SetPosition(vertexCounter-1, hit.point);
				//laser.SetPosition(vertexCounter-1, hit.point);
				lastLaserPosition = hit.point;
				laserDirection = Vector3.Reflect(laserDirection, hit.normal);
			} 
			else 
			{
				laserReflected++;
				vertexCounter++;
				laser.SetVertexCount (vertexCounter);
				laser.SetPosition (vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

				loopActive = false;
			}
			if (laserReflected > laserLimit)
				loopActive = false;
		}
	}
}
