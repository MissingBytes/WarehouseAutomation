using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    Rigidbody cart;
	// Use this for initialization
	void Start () {
        cart = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        cart.velocity = new Vector3(h*3, 0, v*3);
		
	}
}
