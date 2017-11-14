using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archer_controller : MonoBehaviour
{
    [Header("Some cool stuff")]
    [SerializeField] private Transform left_check;
    [SerializeField] private Transform right_check;
    [SerializeField] private LayerMask layer_check;
    [SerializeField] private float radius_check = 0.2f;

    [Header("Fire")]
    [SerializeField] private GameObject arrow_prefab;
    [SerializeField] private Transform arrow_transform;
    [SerializeField] private float arrow_velocity = 500;
    [SerializeField] private float time_to_fire = 10;
    [SerializeField] private float last_time_fire = 0;

    private Rigidbody2D rigid;
    private Control control;
    private Archer archer;
    private SpriteRenderer archer_renderer;
    private Animator anim_controller;
    private Transform player_transform;
    private float view_sight = 10;
    private float shoot_range = 5;
    private Vector3 rotate_around = new Vector3(0, 180, 0);
    private bool rotate_used = false;
    private Vector3 archer_bounds;
    private Vector3 player_bounds;

    // Use this for initialization
    void Start()
    {
        archer = new Archer();
        control = new Control();
        archer_renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        archer_bounds = GetComponent<Renderer>().bounds.size;
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        player_bounds = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().bounds.size;
        rigid.mass = archer.mass;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move_vector = new Vector2(0, 0);
        
        if (player_transform.position.x > transform.position.x && GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            arrow_transform.position = new Vector2(transform.position.x + archer_bounds.x, arrow_transform.position.y);
            rotate_used = false;
        }
        else if (player_transform.position.x < transform.position.x && !GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            arrow_transform.position = new Vector2(transform.position.x - archer_bounds.x, arrow_transform.position.y);
            rotate_used = false;
        }

        bool is_left_ok = Physics2D.OverlapCircle(left_check.position, radius_check, layer_check);
        bool is_right_ok = Physics2D.OverlapCircle(right_check.position, radius_check, layer_check);

        if (player_transform.position.x > transform.position.x + view_sight && is_right_ok)
        {
            move_vector.x = 1;
        }
        else if (player_transform.position.x < transform.position.x - view_sight && is_left_ok)
        {
            move_vector.x = -1;
        }
        else
        {
            if (Time.realtimeSinceStartup - last_time_fire > time_to_fire )
            {
                anim_controller.SetBool("is_shooting", true);
                last_time_fire = Time.realtimeSinceStartup;
            }            
        }

        anim_controller.SetFloat("speed_x", Mathf.Abs(move_vector.x));

        archer.Move(rigid, move_vector);

        if (archer.life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {   
        if(!rotate_used)
        {
            arrow_transform.Rotate(rotate_around);
            arrow_velocity = -arrow_velocity;
            rotate_used = true;
        }

        GameObject arrow = Instantiate(arrow_prefab, arrow_transform.position, arrow_transform.rotation);

        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(arrow_velocity, 0), ForceMode2D.Impulse);

        Destroy(arrow, 5);
        anim_controller.SetBool("is_shooting", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "PlayerAttack")
        {
            archer.HasBeenTouched();
        }
    }
}
