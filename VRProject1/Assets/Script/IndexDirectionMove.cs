using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexDirectionMove : MonoBehaviour
{
    public bool onROffL = true;
    private char RorL = 'R';
    public GameObject controllerObj;
    public PlayerController controller;

    public float bulletSpeed = 10;

    private Rigidbody rb;
    void Start()
    {
        if (onROffL)
            RorL = 'R';
        else
            RorL = 'L';
        controllerObj = GameObject.Find("OVRCustomHandPrefab_" + RorL);
        controller = controllerObj.GetComponent<PlayerController>();
    
        rb = controller.GetComponent<Rigidbody>();

        Vector3 IndexDirection = controller.indexDirection;
        rb.AddForce(IndexDirection * bulletSpeed, ForceMode.Impulse);

    }


    void Update()
    {


    }
}
