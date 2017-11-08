using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField] private GameObject ennemy;
    [SerializeField] private SpriteRenderer ennemy_renderer;
    [SerializeField] private SpriteRenderer ennemy_touched;
    [SerializeField] private Transform ennemy_transform;
    [SerializeField] private Transform[] gun_transform_list;
    [SerializeField] private GameObject bullet_prefab;

    [SerializeField] public int ennemy_health_point = 50;

    [SerializeField] private float time_to_fire = 2;
    [SerializeField] private float bullet_velocity = 10;

    [SerializeField] AnimationCurve anim_vibre;

    private Vector3 pos_initial;
    private float time_max = 4.0f;
    private float amp_vibre = 2;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Fire());

        pos_initial = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ennemy_transform.Rotate(Vector3.forward);

        float current_time = Time.timeSinceLevelLoad % time_max;
        current_time /= time_max - 1;
        float pos_y = anim_vibre.Evaluate(current_time);
        pos_y *= amp_vibre;
        Vector3 new_position = new Vector3(pos_initial.x, pos_y + pos_initial.y, pos_initial.z);
        transform.position = new_position;
	}

    private IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(time_to_fire);
            foreach (Transform t in gun_transform_list)
            {
                GameObject bullet = Instantiate(bullet_prefab, t.position, t.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = t.right * bullet_velocity;
                Destroy(bullet, 5);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "PlayerAmmo")
        {
            EnnemyTouched();

            Destroy(collision.gameObject);
        }
    }

    private void EnnemyDie()
    {
        Destroy(ennemy);
    }

    private void EnnemyTouched()
    {
        ennemy_health_point--;

        if(ennemy_health_point <= 0)
        {
            EnnemyDie();
        }
    }
}
