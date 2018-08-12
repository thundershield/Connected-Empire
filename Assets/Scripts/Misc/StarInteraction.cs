using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarInteraction : Interactable
{
    public GameObject label;
    // Use this for initialization
    private bool clicked;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Select()
    {
        if (clicked == false)
            label.transform.Find("Selected").gameObject.SetActive(true);
    }
    public override void Unselect()
    {
        if (clicked == false)
            label.transform.Find("Selected").gameObject.SetActive(false);
    }
    public override void Click()
    {
        label.transform.Find("Selected").gameObject.SetActive(false);
        clicked = true;
        label.transform.Find("Clicked").gameObject.SetActive(true);
    }
    public override void Unclick()
    {
        clicked = false;
        label.transform.Find("Clicked").gameObject.SetActive(false);
    }
}
