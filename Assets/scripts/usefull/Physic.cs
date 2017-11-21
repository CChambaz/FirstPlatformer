using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physic : MonoBehaviour
{
    [Header("Projectile info")]
    [SerializeField] private Transform origin;
    [SerializeField] private Transform final;
    [SerializeField] private Rigidbody2D rigid;
    //[SerializeField] private ConstantForce2D force_applied;

    [Header("Force applied")]
    [SerializeField] private float acceleration = -9.81f;
    [SerializeField] private float speed_y_origin;
    [SerializeField] private float speed_x = 30;

    private float angle;
    private float start_time;
    private float angle_to_apply;
    private float actual_time;
    private bool target_on_right;    

    public void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        origin = gameObject.GetComponent<Transform>();
        final = GameObject.FindGameObjectWithTag("Player").transform;
        target_on_right = final.position.x > origin.position.x;

        angle = GetBaseAngle(origin, final);

        if (!target_on_right)
        {
            speed_x = -speed_x;
            angle = -angle;
        }

        speed_y_origin = GetBaseSpeedY(acceleration, speed_x, origin, final);
        rigid.velocity = new Vector2(speed_x, speed_y_origin);

        // Get the time when the object is shoot
        start_time = Time.time;

        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Ennemy").GetComponent<Collider2D>());
    }

    // Note
    // Appliquer une diminution de la force relative y sur le temps

    public void Update()
    {
        // Get the actual time of the trajectory
        actual_time = Time.realtimeSinceStartup - start_time;
    }

    public void FixedUpdate()
    {
        // Apply the new y force
        rigid.velocity = new Vector2(rigid.velocity.x, GetSpeedY(acceleration, actual_time, speed_y_origin, origin, final));
    }

    public bool PerpendicularVector2(Vector2 vector_1, Vector2 vector_2)
    {
        float composant_a = vector_1.x * vector_2.x;
        float composant_b = vector_1.y * vector_2.y;

        if (composant_a - composant_b == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetSpeedX(Transform origin, Transform target, float time)
    {
        // Vx = (X - Xo) / t
        return (target.position.x - origin.position.x) / time;
    }

    public float GetTime(float speed_x, Transform origin, Transform target)
    {
        // Xf = Vx * t + Xo
        // t = (Xf - Xo) / Vx
        return (target.position.x - origin.position.x) / speed_x;
    }

    public float GetAngle(float speed_x, float speed_y)
    {
        // alpha = arc-tan(Vy / Vx)
        float angle_rad = Mathf.Atan(speed_y / speed_x);

        return angle_rad * (180 / Mathf.PI);
    }

    public float GetAcceleration(float speed_y_origin, float speed_x, float mass)
    {
        // Sum(F) = Vx + Vy = m * a 
        // a = (Vx + Vy) / m
        return (speed_y_origin + speed_x) / mass;
    }

    public float GetSpeedY(float acceleration, float time, float speed_y_origin, Transform origin, Transform target)
    {
        // Vy = a * t + Voy
        return acceleration * time + speed_y_origin;
        // Vy = a * ((X - Xo) / Vx) + Voy
        //return acceleration * ((transform.position.x - origin.position.y) / speed_x) + speed_y_origin;
    }

    public float GetBaseAngle(Transform origin, Transform target)
    {
        // AB(Bx - Ax, By - Ay)
        Vector2 origin_to_target = new Vector2(target.position.x - origin.position.x, target.position.y - origin.position.y);        

        // alpha_deg = alpha_rad * (180 / PI)
        return Mathf.Asin(origin_to_target.y / origin_to_target.x) * (180 / Mathf.PI);
    }

    public float GetBaseSpeedY(float acceleration, float speed_x, Transform origin, Transform target)
    {

        return Mathf.Abs(acceleration * (((target.position.x) - origin.position.x) / speed_x));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Ennemy")
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(origin.position.x, origin.position.y, origin.position.z), new Vector3(final.position.x, final.position.y, final.position.z));
    }
}
