using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Characters
{
    // Physics value
    public float force_x;
    public float force_y;
    public float mass;

    // Physics reaction
    public bool is_grav_applied;
    public bool is_invicible;

    // Max stats used to set the character
    public int max_life;
    public int max_health_point;
    public int max_ammo;

    // General active stats
    public int life;
    public int health_point;
    public int ammo;

    // Grant immortality if no sub-class is in use
    public Characters()
    {
        force_x = 20;
        force_y = 5;
        mass = 1;

        is_grav_applied = false;
        is_invicible = true;

        max_life = -1;
        max_health_point = -1;
        max_ammo = -1;

        life = max_life;
        health_point = max_health_point;
        ammo = max_ammo;
    }

    public void HasBeenTouched()
    {
        health_point--;

        if(health_point == 0)
        {
            IsDead();
        }
    }

    public void IsDead()
    {
        life--;
    }
}
