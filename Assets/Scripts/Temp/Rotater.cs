using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    private float speed = 50.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(-1 * speed * Time.deltaTime, 2 * speed * Time.deltaTime, 1 * speed * Time.deltaTime));
    }
}
