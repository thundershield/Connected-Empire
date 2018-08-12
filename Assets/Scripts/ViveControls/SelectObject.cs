using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;

    private GameObject collidingObject;

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
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    void Update()
    {
        // 1
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                Debug.Log(collidingObject.name + " Trigger Release");
            }
        }
    }
}
