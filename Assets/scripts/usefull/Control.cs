using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control
{
    public string text_life = "Life : ";
    public string text_health_point = "Health : ";
    public string text_ammo = "Ammo : ";

    public void LoadScene(int scene_id)
    {
        SceneManager.LoadScene(scene_id);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
