using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class peasant_controller : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer peasant_renderer;
    private float time_to_attack = 3.5f;
    private float last_time_attack = 0;
    private float time_touched = 2;
    private float attack_range = 2;
    private Vector2 force_touched = new Vector2(250, 0);
    private Rigidbody2D rigid;
    private Control control;
    private Peasant peasant;
    private Animator anim_controller;
    private Transform player_transform;
    private Vector3 player_bounds;
    private Vector3 base_collider;
    private BoxCollider2D peasant_collider;

    // Use this for initialization
    void Start ()
    {
        peasant = new Peasant();
        control = new Control();
        peasant_renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        peasant_collider = GetComponent<BoxCollider2D>();
        base_collider = GetComponent<BoxCollider2D>().size;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        player_bounds = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().bounds.size;
        rigid.mass = peasant.mass;
	}
	
	// Update is called once per frame
	void Update ()
    {
        peasant.Move(gameObject, rigid, player);

        WillAttack();

        anim_controller.SetFloat("speed_x", Mathf.Abs(rigid.velocity.x));

        if (peasant.life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        if(transform.position.x > player_transform.position.x)
        {
            peasant_collider.size = new Vector2(peasant_collider.size.x - attack_range, peasant_collider.size.y);
        }
        else
        {
            peasant_collider.size = new Vector2(peasant_collider.size.x + attack_range, peasant_collider.size.y);
        }

        peasant_collider.size = base_collider;

        anim_controller.SetBool("is_attacking", false);        
    }

    private void WillAttack()
    {
        if (Time.realtimeSinceStartup - last_time_attack > time_to_attack)
        {
            if (Mathf.Abs(transform.position.x - player_transform.position.x + player_bounds.x) <= attack_range
                || Mathf.Abs(player_transform.position.x - player_bounds.x - transform.position.x) <= attack_range)
            {
                anim_controller.SetBool("is_attacking", true);
                last_time_attack = Time.realtimeSinceStartup;
            }      
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "PlayerAttack")
        {
            peasant.HasBeenTouched();
        }
    }
}
