using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paladin : Characters
{
    public Paladin()
    {
        force_x = 500;
        force_y = 30;
        mass = 80;
        max_speed = 10;

        max_life = 2;
        max_health_point = 75;
        max_ammo = 0;

        life = max_life;
        health_point = max_health_point;
        ammo = max_ammo;
    }

    public void HasBeenTouched(Transform paladin_transform, Transform spawn_transform)
    {
        health_point--;

        if (health_point <= 0)
        {
            IsDead(paladin_transform, spawn_transform);
        }
    }

    public void IsDead(Transform paladin_transform, Transform spawn_transform)
    {
        life--;

        if (life <= 0)
        {
            SceneManager.LoadScene("DieMenu");
        }
        else
        {
            Respawn(paladin_transform, spawn_transform);
        }
    }

    public void Respawn(Transform paladin_transform, Transform spawn_transform)
    {
        paladin_transform.position = spawn_transform.position;
        paladin_transform.rotation = spawn_transform.rotation;
        health_point = max_health_point;
    }
}
