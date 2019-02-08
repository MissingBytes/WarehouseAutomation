using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    Rigidbody cart;
    Transform Package;
    bool cart_loaded = false;
	// Use this for initialization
	void Start () {
        cart = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        cart.velocity = new Vector3(h*3, 0, v*3);
        if (!cart_loaded)
        {
            Collider[] nearby_object = Physics.OverlapSphere(cart.transform.position, 1.5f, 1 << 8);
            if (nearby_object.Length != 0)
                //for (int i = 0; i < nearby_object.Length; i++)
                {
                    nearby_object[0].GetComponent<Rigidbody>().useGravity = false;
                    nearby_object[0].transform.position = cart.transform.position + new Vector3(0, 0.8f, 0);
                    nearby_object[0].transform.parent = cart.transform;
                    Debug.Log(nearby_object[0].name);
                    cart_loaded = true;
                }
        }
		
	}
}
