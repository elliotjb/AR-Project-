using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModePosition
{
    MOVEROTATION,
    MOVE,
    ROTATION
}

public class VuforiaPosition : MonoBehaviour {

    public GameObject child;

    public ModePosition type = ModePosition.MOVEROTATION;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 pos = transform.position;

        switch(type)
        {
            case ModePosition.MOVEROTATION:
                {
                    child.transform.position = new Vector3(pos.x, child.transform.position.y, pos.z);
                    child.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
                    break;
                }
            case ModePosition.MOVE:
                {
                    child.transform.position = new Vector3(pos.x, child.transform.position.y, pos.z);
                    break;
                }
            case ModePosition.ROTATION:
                {
                    child.transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
                    break;
                }
        }
    }
}
