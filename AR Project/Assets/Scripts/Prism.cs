using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    public GameObject other_face_1 = null;
    public GameObject other_face_2 = null;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
    public Vector3 GetFace1Position()
    {
        return other_face_1.transform.position;
    }

    public Vector3 GetFace1Direction()
    {
        return other_face_1.transform.right;
    }

    public Vector3 GetFace2Position()
    {
        return other_face_2.transform.position;
    }

    public Vector3 GetFace2Direction()
    {
        return other_face_2.transform.right;
    }
}
