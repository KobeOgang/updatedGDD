using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMrenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("DojoRemake");
    }
}
