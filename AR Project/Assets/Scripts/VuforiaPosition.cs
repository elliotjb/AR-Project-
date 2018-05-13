using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VuforiaPosition : MonoBehaviour {

    public GameObject child;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = transform.position;

        child.transform.position = new Vector3(pos.x, child.transform.position.y, pos.z);
        child.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));


    }
}
