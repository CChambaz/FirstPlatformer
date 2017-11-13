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

        hero_renderer.flipX = horizontal_input < 0 && horizontal_input != 0;

        Vector2 forceDirection = new Vector2(horizontal_input, 0);
        forceDirection *= paladin_player.force_x;
        rigid.AddForce(forceDirection);
        bool touch_floor = Physics2D.OverlapCircle(position_raycast_jump.position, radius_raycast_jump, layer_mask_jump);

        if (Input.GetAxis("Jump") > 0 && touch_floor)
        {
            rigid.AddForce(Vector2.up * paladin_player.force_y, ForceMode2D.Impulse);

            anim_controller.SetBool("is_touched", true);
        }

        if (Input.GetAxis("Fire1") > 0)
        {
            anim_controller.SetBool("is_attacking", true);
        }
    }
}
