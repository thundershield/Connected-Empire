using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControl : MonoBehaviour
{
    public int quality;
    public int income;
    public int industry;
    public int population;
    public int habitability;
    public string owner;

    public List<int> primaryNode = new List<int>();
    public List<int> secondaryNode = new List<int>();
    public List<GameObject> fleets = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Create(int qual)
    {
        quality = qual;
    }
}
