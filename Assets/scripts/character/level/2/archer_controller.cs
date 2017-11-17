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
    [SerializeField] private BoxCollider2D stab_collider;

    [Header("Fire")]
    [SerializeField] private GameObject arrow_prefab;
    [SerializeField] private Transform arrow_transform;
    [SerializeField] private float arrow_velocity = 500;
    [SerializeField] private float time_to_fire = 10;
    [SerializeField] private float last_time_fire = 0;

    private GameObject player;
    private Rigidbody2D rigid;
    private Control control;
    private Archer archer;
    private Animator anim_controller;
    private Transform player_transform;
    private float view_sight = 20;
    private float shoot_range = 15;
    private float stab_range = 2;
    private float last_stab = 0;
    private float time_stab = 2;
    private Vector3 rotate_around = new Vector3(0, 180, 0);
    private bool rotate_used = false;
    private Vector3 archer_bounds;
    private Vector3 player_bounds;

    // Use this for initialization
    void Start()
    {
        archer = new Archer();
        control = new Control();
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        archer_bounds = GetComponent<Renderer>().bounds.size;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        player_bounds = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().bounds.size;
        rigid.mass = archer.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if(!anim_controller.GetBool("is_touched"))
        {
            Rotate();

            bool is_left_ok = Physics2D.OverlapCircle(left_check.position, radius_check, layer_check);
            bool is_right_ok = Physics2D.OverlapCircle(right_check.position, radius_check, layer_check);

            archer.Move(gameObject, rigid, player, view_sight, is_left_ok, is_right_ok);

            WillShoot();

            WillStab();

            anim_controller.SetFloat("speed_x", Mathf.Abs(rigid.velocity.x));
        } 
    }

    private void Shoot()
    {
        if (!rotate_used)
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

    private void Rotate()
    {
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
    }

    private void WillShoot()
    {
        if (Time.realtimeSinceStartup - last_time_fire > time_to_fire && !anim_controller.GetBool("is_shooting"))
        {
            if ((Mathf.Abs(transform.position.x - player_transform.position.x) <= shoot_range || Mathf.Abs(player_transform.position.x - transform.position.x) <= shoot_range)
                && (Mathf.Abs(transform.position.x - player_transform.position.x) > stab_range || Mathf.Abs(player_transform.position.x - transform.position.x) > stab_range))
            {
                anim_controller.SetBool("is_shooting", true);
            }
            last_time_fire = Time.realtimeSinceStartup;
        }
    }

    private void WillStab()
    {
        if (!anim_controller.GetBool("is_stabbing"))
        {
            if (Mathf.Abs(transform.position.x - player_transform.position.x + player_bounds.x) <= stab_range
                || Mathf.Abs(player_transform.position.x - player_bounds.x - transform.position.x) <= stab_range)
            {
                anim_controller.SetBool("is_stabbing", true);
                stab_collider.enabled = true;
            }
        }
    }

    private void Stab()
    {
        stab_collider.enabled = false;
        anim_controller.SetBool("is_stabbing", false);
    }

    private void Touched()
    {
        if(archer.health_point > 0)
        {
            anim_controller.SetBool("is_touched", false);
        }
        else
        {
            Destroy(gameObject, 1);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && !anim_controller.GetBool("is_touched"))
        {
            archer.HasBeenTouched(1);

            anim_controller.SetBool("is_touched", true);
        }
    }
}
