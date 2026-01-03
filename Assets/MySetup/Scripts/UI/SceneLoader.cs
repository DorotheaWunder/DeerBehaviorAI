using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject SceneMenu;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void ToggleMenu()
    {
        bool isActive = SceneMenu.activeSelf;
        SceneMenu.SetActive(!isActive);
    }
}
