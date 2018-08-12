using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenu : MonoBehaviour {
    //Premade steam VR script
    private SteamVR_TrackedObject trackedObj;
    //Used to store the current object you touching
    private GameObject collidingObject;
    //Stores the material that the menu buttons use when they are selected/unselected
    public Material[] material;
    //used to hold the colliging objects renderer
    Renderer rend;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
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
    //Sets the object you touch to the colliding object and, if it is a button, changes it's texture.
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
        rend=collidingObject.GetComponent<Renderer>();
        rend.material = material[0];
    }
    //Prevents wonkiness.
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
        rend=collidingObject.GetComponent<Renderer>();
        rend.material = material[0];
    }
    //removes the colliding object reference and restores original texture when you stop touching it.
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        rend = collidingObject.GetComponent<Renderer>();
        rend.material = material[1];
        collidingObject = null;
    }
    //Checks to see if you clicked on a button and activates the correct method if you did.
    void Update()
    {
        // 1
        if (Controller.GetHairTriggerDown())
        {
            Debug.Log("test1");
            if (collidingObject.GetComponent<MenuClick>())
            {
                Debug.Log("test2");
                collidingObject.GetComponent<MenuClick>().Select();
            }
        }
    }
}
