using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViveControllerGalaticView : MonoBehaviour
{


    private SteamVR_TrackedObject trackedObj;

    private GameObject collidingObject;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private Vector3 controllerPos;

    private bool held = false;

    private GameObject clickedObject;

    private GameObject defaultDial;

    private GameObject menuSelected;

    private GameObject endTurnSelected;

    private GameObject currentDialState;

    public GameObject cameraRig;
    // Use this for initialization
    private void Start()
    {
        defaultDial = transform.Find("Dial").gameObject.transform.Find("Default Dial").gameObject;
        menuSelected = transform.Find("Dial").gameObject.transform.Find("Menu Selected").gameObject;
        endTurnSelected = transform.Find("Dial").gameObject.transform.Find("End Turn Selected").gameObject;
    }
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }
    
    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        collidingObject = col.gameObject;
    }
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        //Debug.Log(other.name);
        if (other.tag == "Interactable")
        {
            Debug.Log("Test1");
            collidingObject= other.gameObject;
            Debug.Log(other.gameObject);
            Debug.Log(collidingObject);
            if (other.GetComponent<Interactable>())
                other.GetComponent<Interactable>().Select();
        }
        Debug.Log("Test2");
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        //Debug.Log(tempCollidingObject.name);
        if (other.tag == "Interactable")
        {
            Debug.Log("Test11");
            SetCollidingObject(other);
            if (other.GetComponent<Interactable>())
                other.GetComponent<Interactable>().Select();
        }
        Debug.Log("Test2");
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            Debug.Log("test9");
            return;
        }
        if (other.tag == "Interactable")
        {
            Debug.Log("Test6");
            SetCollidingObject(other);
            if (other.GetComponent<Interactable>())
                Debug.Log("Test7");
            other.GetComponent<Interactable>().Unselect();
        }
        collidingObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            held = true;
            controllerPos = transform.position;
        }
        else if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            held = false;
        }
        if (held == true)
        {
            cameraRig.transform.Translate(controllerPos - transform.position);
            transform.position = controllerPos;
        }
        if (Controller.GetHairTriggerDown())
        {
            if (clickedObject)
            {
                clickedObject.GetComponent<Interactable>().Unclick();
                clickedObject = null;
            }
            if (collidingObject)
            {
                collidingObject.GetComponent<Interactable>().Click();
                clickedObject = collidingObject;
            }
        }


        if (Controller.GetAxis() != Vector2.zero)
        {
            if (Controller.GetAxis().y > .2)
            {
                if (currentDialState != endTurnSelected)
                {
                    if (currentDialState != null)
                        currentDialState.SetActive(false);
                    currentDialState = endTurnSelected;
                }
                endTurnSelected.SetActive(true);
                if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    Debug.Log("End Turn Clicked");
                }
            }
            else if (Controller.GetAxis().y < -.2)
            {
                if (currentDialState != menuSelected)
                {
                    if (currentDialState != null)
                        currentDialState.SetActive(false);
                    currentDialState = menuSelected;
                }
                menuSelected.SetActive(true);
                if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
                }
            }
            else
            {
                if (currentDialState != defaultDial)
                {
                    if (currentDialState != null)
                        currentDialState.SetActive(false);
                    currentDialState = defaultDial;
                }
                defaultDial.SetActive(true);
            }
        }
        else
        {
            if (currentDialState != null)
                currentDialState.SetActive(false);
            currentDialState = null;

        }
    }
}
