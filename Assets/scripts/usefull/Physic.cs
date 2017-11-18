using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physic : MonoBehaviour
{
    private float acceleration;
    private float trajectory_time;
    private float angle;
    private float start_time;
    private float angle_to_apply;
    private float actual_time;
    private float speed_y_origin;
    private float speed_x;
    private float mass;
    private Transform origin;
    private Transform final;
    private ConstantForce2D force_applied;

    public void Start()
    {
        origin = gameObject.GetComponent<Transform>();
        force_applied = gameObject.GetComponent<ConstantForce2D>();

        speed_x = force_applied.force.x;
        speed_y_origin = force_applied.relativeForce.y;

        acceleration = Ballistic_GetAcceleration(speed_y_origin, speed_x, mass);
        angle = Ballistic_GetAngle(speed_x, speed_y_origin);
        trajectory_time = Ballistic_GetTime(speed_x, origin, final);

        gameObject.transform.Rotate(0, gameObject.transform.rotation.y - angle, 0);

        // Get the time when the object is shoot
        start_time = Time.time;

        // Add the variable force of the Y axis to the projectile and give him the impulse
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed_y_origin), ForceMode2D.Impulse);
    }

    public void Update()
    {
        // Get the actual time of the trajectory
        actual_time = Time.realtimeSinceStartup - start_time;

        if (start_time != 0 && Time.realtimeSinceStartup - actual_time < trajectory_time)
        {
            // Get the new angle of the projectile
            angle = Ballistic_GetAngle(speed_x, Ballistic_GetSpeedY(acceleration, actual_time, speed_y_origin));

            // Define how many degrees we need to substract from the actual angle
            angle_to_apply = gameObject.transform.rotation.y - angle;

            // Apply the new angle to the object
            gameObject.transform.Rotate(new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y - angle_to_apply));

            // Update the 
            gameObject.GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, Ballistic_GetSpeedY(acceleration, actual_time, speed_y_origin));
        }  
    }


    public void GetTarget(Transform target)
    {
        final = target;        
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

    public float Ballistic_GetTime(float speed_x, Transform origin, Transform target)
    {
        // Xf = Vx * t + Xo
        // t = (Xf - Xo) / Vx
        return (target.position.x - origin.position.x) / speed_x;
    }

    public float Ballistic_GetAngle(float speed_x, float speed_y_origin)
    {
        // alpha = arc-tan(Vy / Vx)
        return Mathf.Atan(speed_y_origin / speed_x);
    }

    public float Ballistic_GetAcceleration(float speed_y_origin, float speed_x, float mass)
    {
        // Sum(F) = Vx + Vy = m * a 
        // a = (Vx + Vy) / m
        return (speed_y_origin + speed_x) / mass;
    }

    public float Ballistic_GetSpeedY(float acceleration, float time, float speed_y_origin)
    {
        // Vy = a * t + Voy
        return acceleration * time + speed_y_origin;
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
        Gizmos.DrawLine(transform.position, force_applied.transform.position);
    }

    /*
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
     */

    // Backup
    /*public void ApplyBallistic(float speed_y_origin, float speed_x, float mass, Transform origin, Transform target, GameObject projectile)
    {
        // Get initial and constant value
        float acceleration = Ballistic_GetAcceleration(speed_y_origin, speed_x, mass);
        float trajectory_time = Ballistic_GetTime(speed_x, origin, target);
        float angle = Ballistic_GetAngle(speed_x, speed_y_origin);

        // Keep the base rotationof the origin
        //Quaternion origin_base_rotation = origin.rotation;

        // Add the angle needed for the shoot to the origin
        //origin.Rotate(origin.rotation.x, origin.rotation.y + angle, origin.rotation.z);
        projectile.transform.Rotate(0, projectile.transform.rotation.y - angle, 0);
        // Creat the object
        //GameObject projectile = Instantiate(projectile_data, origin.position, origin.rotation);

        // Get the time when the object is shoot
        float start_time = Time.time;

        // Add the constant force of the X axis to the projectile
        projectile.AddComponent<ConstantForce2D>().force = new Vector2(speed_x, 0);

        // Add the variable force of the Y axis to the projectile and give him the impulse
        projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed_y_origin), ForceMode2D.Impulse);

        // Needed to manage the trajectory
        float angle_to_apply;
        float actual_time;

        //origin.rotation = origin_base_rotation;

        // Manage the trajectory of the object until it reachs the target
        while (Time.realtimeSinceStartup - start_time > trajectory_time)
        {
            // Get the actual time of the trajectory
            actual_time = Time.realtimeSinceStartup - start_time;

            // Get the new angle of the projectile
            angle = Ballistic_GetAngle(speed_x, Ballistic_GetSpeedY(acceleration, actual_time, speed_y_origin));

            // Define how many degrees we need to substract from the actual angle
            angle_to_apply = projectile.transform.rotation.y - angle;

            // Apply the new angle to the object
            projectile.transform.Rotate(new Vector3(projectile.transform.rotation.x, projectile.transform.rotation.y - angle_to_apply));

            // Update the 
            // projectile.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, Ballistic_GetSpeedY(acceleration, actual_time, speed_y_origin)));
        }

        //Destroy(projectile);
    }*/
}
