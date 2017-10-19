﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconControllerScript : MonoBehaviour {

    private Dictionary<string, GameObject> icons = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> clickedDatabase = new Dictionary<string, bool>();
    private bool free; //used for efficiency, so you just have to check if clicked during update instead of going through all icons
    private string clickedName;
    private float rotationLeft;

    private GameObject[] balls;
    private GameObject laserPointer;
    private CrosshairScript crosshairScript;

	void Start () {
        foreach(GameObject gameObject in GameObject.FindGameObjectsWithTag("Icon")) { icons.Add(gameObject.name, gameObject); }
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Icon")) { clickedDatabase.Add(gameObject.name, false); }
        free = true;
        clickedName = "";
        rotationLeft = 180f;

        balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls) { ball.SetActive(false); }
        laserPointer = GameObject.Find("LaserPointer");
        laserPointer.SetActive(false);
        crosshairScript = GameObject.Find("Crosshair").GetComponent<CrosshairScript>();
    }

    void Update () {
        if (!free)
        {
            foreach (string key in clickedDatabase.Keys)
            {
                if (key == clickedName)
                {
                    float rotation = 180f * Time.deltaTime;
                    if (rotationLeft > rotation)
                    {
                        rotationLeft -= rotation;
                    }
                    else
                    {
                        rotation = rotationLeft;
                        rotationLeft = 180;
                        free = true;
                    }
                    icons[key].transform.Rotate(rotation, 0, 0);
                }
            }
        }		
	}

    public void Click(string targetIconName)
    {
        if (free)
        {
            clickedName = targetIconName;
            free = false;
            if(clickedName == "IconBalls")
            {
                if (!clickedDatabase[clickedName])  //if not clicked
                {
                    foreach (GameObject ball in balls) { ball.SetActive(true); }
                    clickedDatabase[clickedName] = true;
                }
                else
                {
                    foreach (GameObject ball in balls) { ball.SetActive(false); }
                    clickedDatabase[clickedName] = false;
                }

            }
            else if (clickedName == "IconCrosshair")
            {
                print("change implementation");
            }
            else if (clickedName == "IconLaser")
            {
                if (!clickedDatabase[clickedName])  //if not clicked
                {
                    crosshairScript.SetVisible(false);
                    laserPointer.SetActive(true);
                    clickedDatabase[clickedName] = true;
                }
                else
                {
                    laserPointer.SetActive(false);
                    crosshairScript.SetVisible(true);
                    clickedDatabase[clickedName] = false;
                }
            }
        }
    }
}
