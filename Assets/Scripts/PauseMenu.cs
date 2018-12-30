using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool Game_Is_Paused = false;
    public GameObject pause_Menu_UI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !FindObjectOfType<Game>().IsGameOver())
        {
            if (Game_Is_Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pause_Menu_UI.SetActive(false);
        Time.timeScale = 1f;
        Game_Is_Paused = false;
        GameObject.FindGameObjectWithTag("Grid").GetComponent<AudioSource>().Play();
    }
    public void Pause()
    {
        pause_Menu_UI.SetActive(true);
        Time.timeScale = 0f;
        Game_Is_Paused = true;
        GameObject.FindGameObjectWithTag("Grid").GetComponent<AudioSource>().Pause();
    }
}
