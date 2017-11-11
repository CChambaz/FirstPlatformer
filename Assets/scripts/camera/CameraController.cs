using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public GameObject object_to_follow;

    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - object_to_follow.transform.position;
    }

    void LateUpdate()
    {
        transform.position = object_to_follow.transform.position + offset;
    }
}
