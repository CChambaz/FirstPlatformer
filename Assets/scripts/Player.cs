using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]     // Demande cet élément, si il n'existe pas, le crée et empêche sa supression
[RequireComponent(typeof(BoxCollider2D))]

public class Player : MonoBehaviour
{
    [Header("Player stats")]
    [SerializeField] [Range(0.0f,25.0f)] public int max_health_point = 5;
    [SerializeField] public int health_point = 5;
    [SerializeField] public int life = 3;
    [SerializeField] public int max_ammo = 20;
    [SerializeField] public int ammo = 20;    

    [Header("Physics")]
    [SerializeField] private float force = 10;

    [Header("Jump")]
    [SerializeField] private Transform position_raycast_jump;
    [SerializeField] private float radius_raycast_jump;
    [SerializeField] private LayerMask layer_mask_jump;
    [SerializeField] private float force_jump = 2;

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

    // Use this for initialization
    void Start ()
    {
        if (gameObject == null)
        {
            DontDestroyOnLoad(gameObject);
        }

        hero_renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        spawn_transform = GameObject.Find("Spawn").transform;        
    }
	
	// Update is called once per frame
	void Update ()
    {
        text_life.text = TEXT_LIFE + life.ToString();
        text_health_point.text = TEXT_HEALTH_POINT + health_point.ToString();
        text_ammo.text = TEXT_AMMO + ammo.ToString();

        float horizontal_input = Input.GetAxis("Horizontal");
        hero_renderer.flipX = horizontal_input < 0;
        Vector2 forceDirection = new Vector2(horizontal_input, 0);
        forceDirection *= force;
        rigid.AddForce(forceDirection);
        bool touch_floor = Physics2D.OverlapCircle(position_raycast_jump.position, radius_raycast_jump, layer_mask_jump);

        if (Input.GetAxis("Jump") > 0 && touch_floor)
        {
            rigid.AddForce(Vector2.up * force_jump, ForceMode2D.Impulse);
        }

        if (Input.GetAxis("Fire1") > 0 && ammo > 0)
        {
            Fire();
        }

        hero_renderer.flipX = horizontal_input < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Limit")
        {
            PlayerDie();
        }

        if (collision.tag == "Win")
        {
            SceneManager.LoadScene("WinMenu");
        }

        if (collision.tag == "NextLevel")
        {
            control.LoadScene(2);//SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (collision.tag == "BAmmo")
        {
            ammo += 5;
            max_ammo += 5;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "BLife")
        {
            life += 1;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "BHP")
        {
            health_point += 5;
            max_health_point += 5;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ennemy")
        {
            PlayerTouched();

            Destroy(collision.gameObject);
        }        
    }

    private void Fire()
    {
        if (Time.realtimeSinceStartup - last_time_fire > time_to_fire)
        {
            GameObject arrow = Instantiate(arrow_prefab, arrow_transform.position, arrow_transform.rotation);
            arrow.GetComponent<Rigidbody2D>().velocity = arrow_transform.right * arrow_velocity;
            Destroy(arrow, 5);
            ammo -= 1;
            last_time_fire = Time.realtimeSinceStartup;
        }
    }

    private void PlayerDie()
    {
        life--;

        if (life <= 0)
        {
            SceneManager.LoadScene("DieMenu");
        }
        else
        {
            transform.position = spawn_transform.position;
            transform.rotation = spawn_transform.rotation;
            health_point = max_health_point;
            ammo = max_ammo;
        }
    }

    private void PlayerTouched()
    {
        health_point--;

        if (health_point <= 0)
        {
            PlayerDie();
        }
    }
}
