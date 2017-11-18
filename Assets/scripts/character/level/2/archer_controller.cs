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
    private Physic physic;
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
        physic = new Physic();
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

        /*float speed_y_origin = 50;
        float speed_x = 50;*/

        GameObject arrow = Instantiate(arrow_prefab, arrow_transform.position, arrow_transform.rotation);
        // (float speed_y_origin, float speed_x, float mass, Transform origin, Transform target, GameObject projectile)
        arrow.SendMessage("GetTarget", player_transform);
        //
        // Get initial and constant value
        /*float acceleration = physic.Ballistic_GetAcceleration(speed_y_origin, speed_x, 0.1f);
        float trajectory_time = physic.Ballistic_GetTime(speed_x, arrow_transform, player_transform);
        float angle = physic.Ballistic_GetAngle(speed_x, speed_y_origin);

        // Keep the base rotationof the origin
        //Quaternion origin_base_rotation = origin.rotation;

        // Add the angle needed for the shoot to the origin
        //origin.Rotate(origin.rotation.x, origin.rotation.y + angle, origin.rotation.z);
        arrow.transform.Rotate(0, arrow.transform.rotation.y - angle, 0);
        // Creat the object
        //GameObject projectile = Instantiate(projectile_data, origin.position, origin.rotation);

        // Get the time when the object is shoot
        float start_time = Time.time;

        // Add the constant force of the X axis to the projectile
        arrow.GetComponent<ConstantForce2D>().force = new Vector2(speed_x, 0);

        // Add the variable force of the Y axis to the projectile and give him the impulse
        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed_y_origin), ForceMode2D.Impulse);

        // Needed to manage the trajectory
        float angle_to_apply;
        float actual_time;

        //origin.rotation = origin_base_rotation;

        // Manage the trajectory of the object until it reachs the target
        while (Time.realtimeSinceStartup - start_time < trajectory_time)
        {
            // Get the actual time of the trajectory
            actual_time = Time.realtimeSinceStartup - start_time;

            // Get the new angle of the projectile
            angle = physic.Ballistic_GetAngle(speed_x, physic.Ballistic_GetSpeedY(acceleration, actual_time, speed_y_origin));

            // Define how many degrees we need to substract from the actual angle
            angle_to_apply = arrow.transform.rotation.y - angle;

            // Apply the new angle to the object
            arrow.transform.Rotate(new Vector3(arrow.transform.rotation.x, arrow.transform.rotation.y - angle_to_apply));

            // Update the 
            // projectile.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, Ballistic_GetSpeedY(acceleration, actual_time, speed_y_origin)));
        }

        Destroy(arrow);*/
        //
        anim_controller.SetBool("is_shooting", false);

        //physic.ApplyBallistic(50, 50, 1, arrow_transform, player_transform, arrow);    
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
