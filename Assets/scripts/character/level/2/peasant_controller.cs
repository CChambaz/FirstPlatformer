using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class peasant_controller : MonoBehaviour
{
    [SerializeField] private BoxCollider2D attack_collider;

    private GameObject player;
    private float attack_range = 2;
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
        rigid = GetComponent<Rigidbody2D>();
        anim_controller = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        player_bounds = GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().bounds.size;
        rigid.mass = peasant.mass;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!anim_controller.GetBool("is_touched"))
        {
            peasant.Move(gameObject, rigid, player);

            WillAttack();

            anim_controller.SetFloat("speed_x", Mathf.Abs(rigid.velocity.x));
        }        
    }

    private void WillAttack()
    {
        if(!anim_controller.GetBool("is_attacking"))
        {
            if (Mathf.Abs(transform.position.x - player_transform.position.x + player_bounds.x) <= attack_range
                || Mathf.Abs(player_transform.position.x - player_bounds.x - transform.position.x) <= attack_range)
            {
                anim_controller.SetBool("is_attacking", true);
                attack_collider.enabled = true;
            }      
        }
    }

    private void Attack()
    {
        attack_collider.enabled = false;
        anim_controller.SetBool("is_attacking", false);
    }

    private void Touched()
    {
        if(peasant.health_point > 0)
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
            peasant.HasBeenTouched(1);

            anim_controller.SetBool("is_touched", true);            
        }
    }
}
