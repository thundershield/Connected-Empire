using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Activate/LoadScene")]
public class LoadScene : Activate
{
    public string sceneString;

    public override void PerformAction()
    {
        SceneManager.LoadScene(sceneString, LoadSceneMode.Single);
    }
}
