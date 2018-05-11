using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject linked_portal = null;

    public Vector3 pos;
    public Vector3 dir;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        pos = transform.position;
        dir = transform.forward;
	}

    public Vector3 GetLinkedPosition()
    {
        return linked_portal.transform.position;
    }

    public Vector3 GetLinkedDirection()
    {
        return linked_portal.transform.forward;
    }
}
