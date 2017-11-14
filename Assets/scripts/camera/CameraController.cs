using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public GameObject object_to_follow;

    private float base_cam_x_position;
    private float base_player_x_position;
    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - object_to_follow.transform.position;
        base_cam_x_position = GetComponent<Transform>().position.x;
        base_player_x_position = object_to_follow.transform.position.x;
    }

    void LateUpdate()
    {
        if(transform.position.x >= base_cam_x_position || object_to_follow.transform.position.x >= base_player_x_position)
        {
            transform.position = new Vector3(object_to_follow.transform.position.x + offset.x, transform.position.y, transform.position.z);
        }
    }
}
