using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class StarNameTesting : MonoBehaviour {
    public GameObject camera;
    public string starName;
	// Use this for initialization
	void Start () {
        //this.GetComponent<Text>().text = "testing";
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(camera.transform.position);
        this.transform.Rotate(0, 180, 0);
    }
}
