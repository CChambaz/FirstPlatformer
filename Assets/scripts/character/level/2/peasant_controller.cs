﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class peasant_controller : MonoBehaviour
{
    [Header("Object Info")]
    [SerializeField] private SpriteRenderer peasant_renderer;
    [SerializeField] private Text text_life;

    private float time_to_attack = 2;
    private float time_touched = 2;
    private Vector2 force_touched = new Vector2(250, 0);
    private Rigidbody2D rigid;
    private Control control;
    private Peasant peasant;
    private Animator anim_controller;
    private Transform player_transform;
    private Vector3 player_bounds;    

    // Use this for initialization
    void Start ()
    {
        peasant = new Peasant();
        control = new Control();
        peasant_renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        player_bounds = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().bounds.size;
        rigid.mass = peasant.mass;
	}
	
	// Update is called once per frame
	void Update ()
    {
        text_life.text = control.text_health_point + peasant.health_point.ToString();

        anim_controller.SetBool("is_attacking", false);
        anim_controller.SetBool("is_attacking_up", false);
        anim_controller.SetBool("is_attacking_down", false);

        Vector2 move_vector;

        if (player_transform.position.x - player_bounds.x > transform.position.x)
        {
            if (GetComponent<SpriteRenderer>().flipX)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            move_vector = new Vector2(1, 0);
        }
        else if (player_transform.position.x + player_bounds.x < transform.position.x)
        {
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            move_vector = new Vector2(-1, 0);
        }
        else
        {
            move_vector = new Vector2(0, 0);

            /*if (player_transform.position.y > 2 * transform.position.y)
            {
                anim_controller.SetBool("is_attacking_up", true);
            }
            else if(player_transform.position.y < 2 * transform.position.y)
            {
                anim_controller.SetBool("is_attacking_down", true);
            }
            else
            {*/
                anim_controller.SetBool("is_attacking", true);
            //}
        }

        anim_controller.SetFloat("speed_x", Mathf.Abs(move_vector.x));

        peasant.Move(rigid, move_vector);

        if(peasant.life <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "PlayerAttack")
        {
            peasant.HasBeenTouched();

            StartCoroutine(Touched());
        }
    }

    private IEnumerator Touched()
    {
        float actual_time = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - actual_time < time_touched)
        {
            rigid.AddForce(force_touched);
            anim_controller.SetBool("is_touched", true);

            yield return new WaitForSeconds(.1f);
        }

        anim_controller.SetBool("is_touched", false);
    }
}