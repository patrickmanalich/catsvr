﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour {

//    public Texture2D crosshairInit;
//    public Texture2D crosshairHover;
//    public Texture2D crosshairClick;
    //private int width;
    //private int height;
    //private Rect position;
    //private float recticleSize;
    private GameObject mainCamera;
    private bool hover;
    private bool click;
    private bool objectHeld;
    private string[] clickableTags;
    private int time;
    private RaycastHit hit;
    private GameObject heldGameObject;
    private IconControllerScript iconControllerScript;
    private bool visible;
    private GameObject[] quads;

    void Start()
    {
        //recticleSize = 0.125f;
        //if (crosshairInit != null)
        //{
        //    width = crosshairInit.width;
        //    height = crosshairInit.height;
        //    position = new Rect((Screen.width - (width * recticleSize)) / 2, (Screen.height - (height * recticleSize)) / 2, (width * recticleSize), (height * recticleSize));
        //}
        mainCamera = GameObject.Find("Main Camera");
        hover = false;
        click = false;
        objectHeld = false;
        clickableTags = new string[5];    //change each time
        clickableTags[0] = "Cat";
        clickableTags[1] = "Block";
        clickableTags[2] = "Ball";
        clickableTags[3] = "Bed";
        clickableTags[4] = "Icon";
        time = 0;
        iconControllerScript = GameObject.Find("Icon Controller").GetComponent<IconControllerScript>();
        visible = true;
        quads = GameObject.FindGameObjectsWithTag("Quad");
        foreach(GameObject quad in quads) { quad.SetActive(false); }
    }

    void Update()
    {
        if (objectHeld)
        {
            heldGameObject.transform.position = Vector3.Lerp(heldGameObject.transform.position, mainCamera.transform.position + mainCamera.transform.forward * 0.75f, Time.deltaTime * 10);
            heldGameObject.transform.LookAt(mainCamera.transform.position);

        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButton("Tap")) && hover)
        {
            click = true;
            if (hit.collider.gameObject.tag == "Icon")
                iconControllerScript.Click(hit.collider.gameObject.name);
            else if (!objectHeld && visible)
            {
                objectHeld = true;
                HoldTarget(hit.collider.gameObject.name);
            }
            else if (objectHeld && heldGameObject.name == hit.collider.gameObject.name && visible)
            {
                objectHeld = false;
                ThrowTarget(hit.collider.gameObject.name);
            }
        }

        if (click)
        {
            if (time > 30)
            {
                time = 0;
                click = false;
            }
            else
                time++;
        }
        else if (Physics.Raycast(mainCamera.transform.position + mainCamera.transform.forward * 0.5f, mainCamera.transform.forward, out hit))
        {
            if (hit.collider)
            {
                bool found = false;
                for (int i = 0; i < clickableTags.Length; i++)
                {
                    if (hit.collider.tag == clickableTags[i])
                    {
                        found = true;
                        i = clickableTags.Length; //exit loop
                    }
                }
                if (found)
                    hover = true;
                else
                    hover = false;
            }
        }
    }

    void OnGUI()
    {
        if (!objectHeld && visible)
        {
            if (!hover)
            {
                foreach (GameObject quad in quads)
                {
                    if (quad.name == "Quad Init")
                        quad.SetActive(true);
                    else
                        quad.SetActive(false);
                }
            }
                
                //GUI.DrawTexture(position, crosshairInit);
            else
            {
                if (click)
                {
                    foreach (GameObject quad in quads)
                    {
                        if (quad.name == "Quad Click")
                            quad.SetActive(true);
                        else
                            quad.SetActive(false);
                    }
                }
                //GUI.DrawTexture(position, crosshairClick);
                else
                {
                    foreach (GameObject quad in quads)
                    {
                        if (quad.name == "Quad Hover")
                            quad.SetActive(true);
                        else
                            quad.SetActive(false);
                    }
                }
                //GUI.DrawTexture(position, crosshairHover);
            }
        }
    }

    private void HoldTarget(string targetGameObjectName)
    {
        heldGameObject = GameObject.Find(targetGameObjectName);
        Rigidbody rb = heldGameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        if (targetGameObjectName == "Snowball")
        {
            CatIdleScript script = heldGameObject.GetComponent<CatIdleScript>();
            script.enabled = false;
        }
    }

    private void ThrowTarget(string targetGameObjectName)
    {
        heldGameObject = GameObject.Find(targetGameObjectName);
        Rigidbody rb = heldGameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        if (targetGameObjectName == "Snowball")
        {
            CatIdleScript script = heldGameObject.GetComponent<CatIdleScript>();
            script.enabled = true;
        }
        else
        {
            rb.AddForce(mainCamera.transform.forward * 3.0f, ForceMode.Impulse);
        }
    }

    public void SetVisible(bool value)
    {
        visible = value;
        if(!value)
            foreach (GameObject quad in quads) { quad.SetActive(false); }

    }
}
