using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClick : MonoBehaviour
{
    public Activate activeScript;
    public void Select()
    {
        if (activeScript)
        {
            activeScript.PerformAction();
        }
        //SceneManager.LoadScene(sceneString, LoadSceneMode.Single);
    }


}
