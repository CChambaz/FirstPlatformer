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
    public float max_speed;

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
        max_speed = -1;

        is_grav_applied = false;
        is_invicible = true;

        max_life = -1;
        max_health_point = -1;
        max_ammo = -1;

        life = max_life;
        health_point = max_health_point;
        ammo = max_ammo;
    }

    public void Move(GameObject game_object, Rigidbody2D rigid, GameObject move_to, float range = 0, bool is_left_ok = true, bool is_right_ok = true)
    {
        Vector2 force_vector;

        if (move_to.transform.position.x - move_to.GetComponent<SpriteRenderer>().bounds.size.x > game_object.transform.position.x + range && is_right_ok)
        {
            if (game_object.GetComponent<SpriteRenderer>().flipX)
            {
                game_object.GetComponent<SpriteRenderer>().flipX = false;
            }

            force_vector = new Vector2(1, 0);
        }
        else if (move_to.transform.position.x + move_to.GetComponent<SpriteRenderer>().bounds.size.x < game_object.transform.position.x - range && is_left_ok)
        {
            if (!game_object.GetComponent<SpriteRenderer>().flipX)
            {
                game_object.GetComponent<SpriteRenderer>().flipX = true;
            }

            force_vector = new Vector2(-1, 0);
        }
        else
        {
            force_vector = new Vector2(0, 0);
        }

        rigid.velocity = force_vector;
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
