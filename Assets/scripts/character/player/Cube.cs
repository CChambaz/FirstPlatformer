using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cube : Characters
{
    public Cube()
    {
        force_x = 7;
        force_y = 2;

        max_life = 5;
        max_health_point = 10;
        max_ammo = 25;

        life = max_life;
        health_point = max_health_point;
        ammo = max_ammo;
    }
	
    // need to instantiate the projectile before and destroy after
    public void Fire(float projectile_velocity, GameObject projectile, Transform projectile_transform)
    {
        projectile.GetComponent<Rigidbody2D>().velocity = projectile_transform.right * projectile_velocity;
        ammo -= 1;
    }
    
    public void HasBeenTouched(Transform cube_transform, Transform spawn_transform)
    {
        health_point--;

        if (health_point <= 0)
        {
            IsDead(cube_transform, spawn_transform);
        }
    }

    public void IsDead(Transform cube_transform, Transform spawn_transform)
    {
        life--;

        if (life <= 0)
        {
            SceneManager.LoadScene("DieMenu");
        }
        else
        {
            Respawn(cube_transform, spawn_transform);
        }
    }

    public void Respawn(Transform cube_transform, Transform spawn_transform)
    {
        cube_transform.position = spawn_transform.position;
        cube_transform.rotation = spawn_transform.rotation;
        health_point = max_health_point;
        ammo = max_ammo;
    }
}
