﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject linked_portal = null;

	void Start ()
    {		
	}
	
	void Update ()
    {
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
