using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {


    public Material[] material;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material = material[1];
    }
    public void OnTriggerEnter(Collider other)
    {
//        rend.sharedMaterial = material[1];
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
//        rend.sharedMaterial = material[1];
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
//        rend.sharedMaterial = material[0];
    }
}
