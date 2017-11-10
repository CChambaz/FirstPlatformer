using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]     // Demande cet élément, si il n'existe pas, le crée et empêche sa supression
[RequireComponent(typeof(BoxCollider2D))]

public class Player : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private Transform position_raycast_jump;
    [SerializeField] private float radius_raycast_jump;
    [SerializeField] private LayerMask layer_mask_jump;

    [Header("Fire")]
    [SerializeField] private GameObject arrow_prefab;
    [SerializeField] private Transform arrow_transform;
    [SerializeField] private float arrow_velocity = 2;
    [SerializeField] private float time_to_fire = 2;
    [SerializeField] private float last_time_fire = 0;

    [Header("UI")]
    [SerializeField] private Text text_life;
    [SerializeField] private Text text_health_point;
    [SerializeField] private Text text_ammo;

    [Header("Object Info")]
    [SerializeField] private GameObject hero;
    [SerializeField] private SpriteRenderer hero_renderer;
    [SerializeField] [TextArea] public string message;
    [SerializeField] private Color test;

    private const string TEXT_LIFE = "Life : ";
    private const string TEXT_HEALTH_POINT = "Health : ";
    private const string TEXT_AMMO = "Ammo : ";

    private Transform spawn_transform;
    private Rigidbody2D rigid;
    private Control control;
    private Cube cube_player;

    // Use this for initialization
    void Start ()
    {
        cube_player = new Cube();
        hero_renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        spawn_transform = GameObject.Find("Spawn").transform;        
    }
	
	// Update is called once per frame
	void Update ()
    {
        text_life.text = TEXT_LIFE + cube_player.life.ToString();
        text_health_point.text = TEXT_HEALTH_POINT + cube_player.health_point.ToString();
        text_ammo.text = TEXT_AMMO + cube_player.ammo.ToString();

        float horizontal_input = Input.GetAxis("Horizontal");

        hero_renderer.flipX = horizontal_input < 0 && horizontal_input != 0;

        Vector2 forceDirection = new Vector2(horizontal_input, 0);
        forceDirection *= cube_player.force_x;
        rigid.AddForce(forceDirection);
        bool touch_floor = Physics2D.OverlapCircle(position_raycast_jump.position, radius_raycast_jump, layer_mask_jump);

        if (Input.GetAxis("Jump") > 0 && touch_floor)
        {
            rigid.AddForce(Vector2.up * cube_player.force_y, ForceMode2D.Impulse);
        }

        if (Input.GetAxis("Fire1") > 0 && cube_player.ammo > 0)
        {
            if (Time.realtimeSinceStartup - last_time_fire > time_to_fire)
            {
                GameObject arrow = Instantiate(arrow_prefab, arrow_transform.position, arrow_transform.rotation);

                cube_player.Fire(arrow_velocity, arrow, arrow_transform);

                Destroy(arrow, 5);
                last_time_fire = Time.realtimeSinceStartup;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Limit")
        {
            cube_player.IsDead(transform, spawn_transform);
        }

        if (collision.tag == "Win")
        {
            SceneManager.LoadScene("WinMenu");
        }

        if (collision.tag == "NextLevel")
        {
            control.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (collision.tag == "BAmmo")
        {
            cube_player.ammo += 5;
            cube_player.max_ammo += 5;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "BLife")
        {
            cube_player.life += 1;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "BHP")
        {
            cube_player.health_point += 5;
            cube_player.max_health_point += 5;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ennemy")
        {
            cube_player.HasBeenTouched(transform, spawn_transform);

            Destroy(collision.gameObject);
        }

        if (collision.collider.tag == "NextLevel")
        {
            control.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
