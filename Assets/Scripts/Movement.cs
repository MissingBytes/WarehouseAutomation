using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    Rigidbody cart;
    // Transform Package;
    Vector3 [] dest = { new Vector3(-2, 0.75f, 6), new Vector3(-12, 0.75f, 3) };
    Vector3 current;
    Vector3[] WayPoints = new Vector3[2];

    bool cart_loaded = false;
    bool move_z = true;
    bool move_x = true;

    public Transform [] packages;



    int WayPoint_ctr = 0;
    int place_ctr = 0;
    int package_ctr = 0;


    // Use this for initialization
    void Start () {
        
        WayPoints[0] = packages[0].position;
        Debug.Log("WP:"+WayPoints[0]);

        //WayPoints[1] =  dest[0];//new Vector3(-2, 0.75f , 5);//CreateRandomBoxes.dest;
        Debug.Log("DEST:" + WayPoints[1]);

        //WayPoints[2] = packages[1].position;
        //WayPoints[3] = dest[1];

        cart = GetComponent<Rigidbody>();

 	}
	
	// Update is called once per frame
	void Update () {
        dest[0] = CreateRandomBoxes.dest;
        WayPoints[1] = dest[0];
        
        current = WayPoints[WayPoint_ctr];

        MoveTowardsXY(current);
        Pick_object();
        Place_object();

    }

    void MoveTowardsXY(Vector3 destination)
    {
        float step = 4 * Time.deltaTime;

        if ( move_z )
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0.75f, destination.z), step);


        else if( move_x)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, 0.75f, destination.z), step);


        Debug.Log(destination);
        if ((transform.position.z) == (destination.z))
        {
            move_z = false;
        }

        if ((transform.position.x) == (destination.x))
        {
            WayPoint_ctr++;
            move_x = move_z = true;
        }
    }

    void Pick_object()
    {
        if (!cart_loaded)
        {
            // Collider[] nearby_object = Physics.OverlapSphere(cart.transform.position, 1.5f, 1 << 8);
            //    if (nearby_object.Length != 0)

            if (Vector3.Distance(cart.position, packages[package_ctr].position) < 3f)
            {
                packages[package_ctr].GetComponent<Rigidbody>().useGravity = false;
                packages[package_ctr].transform.parent = cart.transform;
                if(!CreateRandomBoxes.rotated)
                    packages[package_ctr].transform.position = cart.transform.position + new Vector3(0, 0.25f+ packages[package_ctr].transform.localScale.y/2, 0);
                else
                    packages[package_ctr].transform.position = cart.transform.position + new Vector3(0, 0.25f + packages[package_ctr].transform.localScale.x / 2, 0);

                Debug.Log(packages[package_ctr].name);
                cart_loaded = true;

            }
        }
    }


    void Place_object()
    {

        //if (cart_loaded && cart.position == dest[place_ctr])
        if (cart_loaded && (cart.position.x == dest[place_ctr].x && cart.position.z == dest[place_ctr].z))
        {
            packages[package_ctr].transform.parent = null;
            //packages[package_ctr].transform.position = cart.transform.position + new Vector3(0, 7, 3);
            packages[package_ctr].transform.position = dest[place_ctr] + new Vector3(0, 0, 3);
            packages[package_ctr].GetComponent<Rigidbody>().useGravity = true;
            cart_loaded = false;
            Debug.Log("Placed object:"+ dest[place_ctr] + new Vector3(0, 0, 3));
            place_ctr++;
            package_ctr++;
        }
    }

}
