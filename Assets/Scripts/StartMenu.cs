using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadSceneAsync("Player_Home");
        SceneManager.UnloadSceneAsync("TitleScreen");
    }
}
