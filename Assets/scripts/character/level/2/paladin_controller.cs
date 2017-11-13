using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class paladin_controller : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private Transform position_raycast_jump;
    [SerializeField] private float radius_raycast_jump;
    [SerializeField] private LayerMask layer_mask_jump;

    [Header("UI")]
    [SerializeField] private Text text_life;
    [SerializeField] private Text text_health_point;

    [Header("Object Info")]
    [SerializeField] private SpriteRenderer hero_renderer;
    [SerializeField] private BoxCollider2D hero_collider;

    private float time_touched = 2;

    private Transform spawn_transform;
    private Rigidbody2D rigid;
    private Control control;
    private Paladin paladin_player;
    private Animator anim_controller;
    private Vector3 collider_size;

	// Use this for initialization
	void Start ()
    {
        paladin_player = new Paladin();
        control = new Control();
        hero_renderer = GetComponent<SpriteRenderer>();
        hero_collider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        spawn_transform = GameObject.Find("Spawn").transform;
        rigid.mass = paladin_player.mass;
        collider_size = hero_collider.size;
    }
	
	// Update is called once per frame
	void Update ()
    {
        text_life.text = control.text_life + paladin_player.life.ToString();
        text_health_point.text = control.text_health_point + paladin_player.health_point.ToString();

        float horizontal_input = Input.GetAxis("Horizontal");

        anim_controller.SetFloat("speed_x", Mathf.Abs(horizontal_input));
        anim_controller.SetBool("is_touched", false);
        anim_controller.SetBool("is_attacking", false);
        gameObject.tag = "Player";

        hero_collider.size = collider_size;

        if (Input.GetKey(KeyCode.A) && !hero_renderer.flipX)
        {
            hero_renderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.D) && hero_renderer.flipX)
        {
            hero_renderer.flipX = false;
        }

        Vector2 forceDirection = new Vector2(horizontal_input, 0);
        forceDirection *= paladin_player.force_x;
        if (Mathf.Abs(rigid.velocity.x) <= paladin_player.force_x)
        {
            rigid.AddForce(forceDirection);                     
        }
        
        bool touch_floor = Physics2D.OverlapCircle(position_raycast_jump.position, radius_raycast_jump, layer_mask_jump);

        if (Input.GetAxis("Jump") > 0 && touch_floor)
        {
            if (rigid.velocity.y <= paladin_player.force_y)
            {
                rigid.AddForce(Vector2.up * paladin_player.force_y, ForceMode2D.Impulse);
            }            
            
            anim_controller.SetBool("is_touched", true);
        }

        if (Input.GetAxis("Fire1") > 0)
        {
            gameObject.tag = "PlayerAttack";

            hero_collider.size = new Vector2(collider_size.x * 2, collider_size.y);

            anim_controller.SetBool("is_attacking", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float actual_time = Time.realtimeSinceStartup;

        if (collision.collider.tag == "Ennemy")
        {
            paladin_player.HasBeenTouched(transform, spawn_transform);

            anim_controller.SetBool("is_touched", true);

            /*while (Time.realtimeSinceStartup - actual_time < time_touched)
            {
                anim_controller.SetBool("is_touched", true);
            }

            anim_controller.SetBool("is_touched", false);*/
        }
    }
}
