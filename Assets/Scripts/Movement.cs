using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    Rigidbody cart;
    // Transform Package;
    Vector3[] dest = new Vector3[5];
    Vector3 current;
    Vector3[] WayPoints = new Vector3[10];

    bool cart_loaded = false;
    bool move_z = true;
    bool move_x = true;

    public Transform [] packages;



    int WayPoint_ctr = 0;
    int place_ctr = 0;
    int package_ctr = 0;


    // Use this for initialization
    void Start () {

        for (int i = 0; i < packages.Length; i++)
            WayPoints[i*2] = packages[i].position;      
        

        Debug.Log("WP:"+WayPoints[0]);

        
        Debug.Log("DEST:" + WayPoints[1]);


        cart = GetComponent<Rigidbody>();

 	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < packages.Length; i++)
            dest[i] = CreateRandomBoxes.dest[i];
        //dest[1] = CreateRandomBoxes.dest[1];

        for (int i = 0; i < packages.Length; i++)
            WayPoints[i*2+1] = dest[i];
        //WayPoints[1] = dest[0];
        //WayPoints[3] = dest[1];
        //WayPoints[3] = packages[0].position- new Vector3(0,0,2f);

        Debug.Log("WP ctr:"+WayPoint_ctr+" WP="+WayPoints[WayPoint_ctr]);
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


        Debug.Log("Moving Towards:"+destination);
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
        int wc_save = WayPoint_ctr;
        if (!cart_loaded)
        {
            // Collider[] nearby_object = Physics.OverlapSphere(cart.transform.position, 1.5f, 1 << 8);
            //    if (nearby_object.Length != 0)
            Vector2 a = new Vector2(cart.position.x, cart.position.z);
            Vector2 b = new Vector2(packages[package_ctr].position.x, packages[package_ctr].position.z);
            //if (Vector2.Distance(cart.position, packages[package_ctr].position) < 3f)
            if (Vector2.Distance(a,b) < 3f)
            {
                packages[package_ctr].GetComponent<Rigidbody>().useGravity = false;
                packages[package_ctr].transform.parent = cart.transform;
                if(!CreateRandomBoxes.rotated)
                    packages[package_ctr].transform.position = cart.transform.position + new Vector3(0, 1+ packages[package_ctr].transform.localScale.y/2f, 0);
                else
                    packages[package_ctr].transform.position = cart.transform.position + new Vector3(0, 1 + packages[package_ctr].transform.localScale.x / 2f, 0);

                Debug.Log("Picked up:"+packages[package_ctr].name);
                cart_loaded = true;
                //if (wc_save == WayPoint_ctr)
                 //   WayPoint_ctr++;

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
            Debug.Log("Placed object:"+ dest[place_ctr]);
            place_ctr++;
            package_ctr++;
        }
    }

}
