using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexDirectionMove : MonoBehaviour
{
    private GameObject controllerObj;
    private PlayerController controller;

    public float bulletSpeed = 10;

    private Rigidbody rb;
    void Start()
    {
        controllerObj = GameObject.Find("PlayerControllerL");
        controller = controllerObj.GetComponent<PlayerController>();
    
        rb = this.GetComponent<Rigidbody>();

        Vector3 IndexDirection = controller.indexDirection;
        rb.AddForce(IndexDirection * bulletSpeed, ForceMode.Impulse);

    }


    void Update()
    {


    }
}
