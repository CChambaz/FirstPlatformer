using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float background_size;
    public float background_high;

    private Transform camera_transform;
    private Transform[] layers;
    private float view_zone = 25;
    private int left_index;
    private int right_index;

    private void Start()
    {
        camera_transform = Camera.main.transform;
        layers = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        left_index = 0;
        right_index = layers.Length - 1;
    }

    private void Update()
    {
        if(camera_transform.position.x < (layers[left_index].transform.position.x + view_zone))
        {
            ScrollLeft();
        }

        if (camera_transform.position.x > (layers[right_index].transform.position.x) - view_zone/2)
        {
            ScrollRight();
        }
    }
    private void ScrollLeft()
    {
        int last_right = right_index;
        
        layers[right_index].position = new Vector3(layers[left_index].position.x - background_size, background_high, 0);
        left_index = right_index;
        right_index--;

        if(right_index < 0)
        {
            right_index = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        int last_left = left_index;

        layers[left_index].position = new Vector3(layers[right_index].position.x + background_size, background_high, 0);
        right_index = left_index;
        left_index++;

        if (left_index == layers.Length)
        {
            left_index = 0;
        }
    }
}
