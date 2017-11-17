using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private CircleCollider2D attack_collider;

    private Transform spawn_transform;
    private Rigidbody2D rigid;
    private Control control;
    private Paladin paladin_player;
    private Animator anim_controller;

	// Use this for initialization
	void Start ()
    {
        paladin_player = new Paladin();
        control = new Control();
        hero_renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        spawn_transform = GameObject.Find("Spawn").transform;
        rigid.mass = paladin_player.mass;
    }
	
	// Update is called once per frame
	void Update ()
    {
        text_life.text = control.text_life + paladin_player.life.ToString();
        text_health_point.text = control.text_health_point + paladin_player.health_point.ToString();

        float horizontal_input = Input.GetAxis("Horizontal");

        anim_controller.SetFloat("speed_x", Mathf.Abs(horizontal_input));
        
        anim_controller.SetBool("is_attacking", false);
        gameObject.tag = "Player";

        if (Input.GetKey(KeyCode.A) && !hero_renderer.flipX)
        {
            rigid.velocity = rigid.velocity.normalized;
            hero_renderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.D) && hero_renderer.flipX)
        {
            rigid.velocity = rigid.velocity.normalized;
            hero_renderer.flipX = false;
        }

        Vector2 forceDirection = new Vector2(horizontal_input, 0);
        forceDirection *= paladin_player.force_x;

        rigid.AddForce(forceDirection);

        if (Mathf.Abs(rigid.velocity.magnitude) > paladin_player.max_speed)
        {
            rigid.velocity = rigid.velocity.normalized * paladin_player.max_speed;
        }
        
        bool touch_floor = Physics2D.OverlapCircle(position_raycast_jump.position, radius_raycast_jump, layer_mask_jump);

        if (touch_floor)
        {
            anim_controller.SetBool("is_jumping", false);
        }

        if (Input.GetButton("Jump") && touch_floor)
        {
            if (rigid.velocity.y <= paladin_player.force_y)
            {
                rigid.AddForce(Vector2.up * paladin_player.force_y, ForceMode2D.Impulse);
            }            
            
            anim_controller.SetBool("is_jumping", true);
        }        

        if (!touch_floor)
        {
            anim_controller.SetBool("is_jumping", true);
        }

        if (Input.GetAxis("Fire1") > 0 && !anim_controller.GetBool("is_attacking"))
        {
            attack_collider.enabled = true;
            anim_controller.SetBool("is_attacking", true);            
        }
    }

    private void Attack()
    {
        anim_controller.SetBool("is_attacking", false);

        attack_collider.enabled = false;
    }

    private void Touched()
    {
        anim_controller.SetBool("is_touched", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Win")
        {
            SceneManager.LoadScene("WinMenu");
        }

        if (collision.tag == "EnnemyAttack")
        {
            paladin_player.HasBeenTouched(transform, spawn_transform);

            anim_controller.SetBool("is_touched", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ennemy")
        {
            paladin_player.HasBeenTouched(transform, spawn_transform);

            anim_controller.SetBool("is_touched", true);
        }

        if (collision.collider.tag == "EnnemyAmmo")
        {
            paladin_player.HasBeenTouched(transform, spawn_transform);

            anim_controller.SetBool("is_touched", true);

            Destroy(collision.collider.gameObject);
        }
    }
}
