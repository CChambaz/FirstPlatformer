using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pause_panel;
    [SerializeField] private GameObject ui_panel;

    public event Action on_pause_event;

    bool is_paused = false;

    private static PauseScript instance;

    public static PauseScript Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PauseScript>();
            }

            return instance;
        }
    }

    public void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetButton("Pause") && !is_paused)
        {
            is_paused = true;
            pause_panel.SetActive(true);
            ui_panel.SetActive(false);
            Time.timeScale = 0;
        }
	}

    public void Resume()
    {
        is_paused = false;
        pause_panel.SetActive(false);
        ui_panel.SetActive(true);
        Time.timeScale = 1;
    }
}
