using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotation : MonoBehaviour {

    public float speed = 5.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
        //transform.Rotate(Vector3.up, );

    }
}
