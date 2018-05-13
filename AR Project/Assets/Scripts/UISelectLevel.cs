using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISelectLevel : MonoBehaviour {

    public GameObject levels13;
    public GameObject levels46;
    public GameObject levels79;

    public float speed = 5.0f;
    public float dist = 400.0f;

    private float dist_moved = 0.0f;

    Vector3 new_pos13;
    Vector3 new_pos46;
    Vector3 new_pos79;

    public enum MoveLevels
    {
        NONE,
        MOVELEFT,
        MOVERIGHT
    }

    public enum PositionLevels
    {
        LEVEL13,
        LEVEL46,
        LEVEL79
    }

    MoveLevels move_ui = MoveLevels.NONE;
    PositionLevels positionlevels = PositionLevels.LEVEL13;
    bool move = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (move)
        {
            if (move_ui == MoveLevels.MOVELEFT)
            {
                levels13.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(levels13.GetComponent<RectTransform>().localPosition, new_pos13, speed * Time.deltaTime);
                levels46.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(levels46.GetComponent<RectTransform>().localPosition, new_pos46, speed * Time.deltaTime);
                levels79.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(levels79.GetComponent<RectTransform>().localPosition, new_pos79, speed * Time.deltaTime);

                dist_moved += speed * Time.deltaTime;
            }
            else if (move_ui == MoveLevels.MOVERIGHT)
            {
                levels13.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(levels13.GetComponent<RectTransform>().localPosition, new_pos13, speed * Time.deltaTime);
                levels46.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(levels46.GetComponent<RectTransform>().localPosition, new_pos46, speed * Time.deltaTime);
                levels79.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(levels79.GetComponent<RectTransform>().localPosition, new_pos79, speed * Time.deltaTime);

                dist_moved += speed * Time.deltaTime;
            }

            if (dist_moved >= dist)
            {
                dist_moved = 0.0f;
                move = false;
                move_ui = MoveLevels.NONE;
            }
        }
	}

    public void MoveRight()
    {
        bool canmove = false;
        if (move)
        {
            return;
        }
        switch (positionlevels)
        {
            case PositionLevels.LEVEL13:
                {
                    //positionlevels
                    //canmove = true;
                    break;
                }
            case PositionLevels.LEVEL46:
                {
                    positionlevels = PositionLevels.LEVEL13;
                    canmove = true;
                    break;
                }
            case PositionLevels.LEVEL79:
                {
                    positionlevels = PositionLevels.LEVEL46;
                    canmove = true;
                    break;
                }
        }
        if (canmove && move == false)
        {
            new_pos13 = new Vector3(levels13.GetComponent<RectTransform>().localPosition.x + 400.0f, 0, 0);
            new_pos46 = new Vector3(levels46.GetComponent<RectTransform>().localPosition.x + 400.0f, 0, 0);
            new_pos79 = new Vector3(levels79.GetComponent<RectTransform>().localPosition.x + 400.0f, 0, 0);
            
            move_ui = MoveLevels.MOVERIGHT;
            move = true;
        }
    }

    public void MoveLeft()
    {
        bool canmove = false;
        if (move)
        {
            return;
        }
        switch (positionlevels)
        {
            case PositionLevels.LEVEL13:
                {
                    positionlevels = PositionLevels.LEVEL46;
                    canmove = true;
                    break;
                }
            case PositionLevels.LEVEL46:
                {
                    positionlevels = PositionLevels.LEVEL79;
                    canmove = true;
                    break;
                }
            case PositionLevels.LEVEL79:
                {
                    //positionlevels = PositionLevels.LEVEL46;
                    //canmove = true;
                    break;
                }
        }
        if (canmove)
        {
            new_pos13 = new Vector3(levels13.GetComponent<RectTransform>().localPosition.x - 400.0f, 0, 0);
            new_pos46 = new Vector3(levels46.GetComponent<RectTransform>().localPosition.x - 400.0f, 0, 0);
            new_pos79 = new Vector3(levels79.GetComponent<RectTransform>().localPosition.x - 400.0f, 0, 0);

            move_ui = MoveLevels.MOVELEFT;
            move = true;
        }
    }

    public void LoadLevel1()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    public void LoadLevel2()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level2");
        }
    }

    public void LoadLevel3()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level3");
        }
    }

    public void LoadLevel4()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level4");
        }
    }

    public void LoadLevel5()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level5");
        }
    }

    public void LoadLevel6()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level6");
        }
    }

    public void LoadLevel7()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level7");
        }
    }
    public void LoadLevel8()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level8");
        }
    }

    public void LoadLevel9()
    {
        if (move == false)
        {
            SceneManager.LoadScene("Level9");
        }
    }

    public void BackToMenu()
    {
        if (move == false)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

}
