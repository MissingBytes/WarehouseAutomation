using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateRandomBoxes : MonoBehaviour {
    public Material cude_color;
    public Material RPkg_color;
    // Use this for initialization
    void Start () {
        System.Random rand = new System.Random();
        int [] Boxes_heights = new int[20];

        int[,] Boxes_matrix = new int[10, 20];


        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // obj.AddComponent<Rigidbody>();

                int height = rand.Next(3, 6);
                Boxes_heights[i] = height;
                obj.transform.localScale = new Vector3(1, height, 2);//breadth is const =2

                obj.transform.position = new Vector3(i - 10, (height / 2f)+0.25f, 9+j*-10);
                obj.GetComponent<Renderer>().material = cude_color;
            }
        }

       // Boxes_heights =new int [20]{ 5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5};
        for (int j = 0; j < 20; j++)
        {
            int k = Boxes_heights[j];
            for (int i = 0; i < 10; i++)
            {
                if (i >= 10 - k)
                    Boxes_matrix[i,j] = 1; //1 if a block is present 0 if not.
            }
        }


        GameObject Random_Package = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // obj.AddComponent<Rigidbody>();

        int RPkg_height = rand.Next(1, 4);
        int RPkg_width = rand.Next(1, 4);
        //RPkg_height = 2;
        //RPkg_width = 4;
        Random_Package.transform.localScale = new Vector3(RPkg_width, RPkg_height, 2);
        Random_Package.transform.position = new Vector3(-5, (RPkg_height / 2f)+0.25f, -10);

        int[,] afterPlacing;

        afterPlacing=Placed_matrix(Boxes_matrix, Boxes_heights, RPkg_height, RPkg_width, 0);

        //Debug.Log(Placed_matrix(Boxes_matrix, Boxes_heights, RPkg_width, RPkg_width,0)[0, 0]);
        //Debug.Log(Boxes_matrix[0, 0]);
       // Debug.Log("AGGREGATE HEIGHT:"+Aggregate_height(Boxes_heights,RPkg_height,RPkg_width,0));
        Debug.Log("AGGREGATE HEIGHT:"+ Aggregate_height(afterPlacing));
       // Debug.Log("COMPLETE LINES:" + Complete_lines(Boxes_matrix));

    }


    //How matrix will look like after placing the box onto the position
    int [,] Placed_matrix(int [,] Boxes_matrix,int [] Boxes_heights, int RPkg_height, int RPkg_width, int position)
    {
        int[,] after_placing = Boxes_matrix.Clone() as int[,];

        int[] Slice = new int[RPkg_width];

        int m = 0;
        for (int i = position; i < position + RPkg_width; i++)
        {
            Slice[m++] = Boxes_heights[i];
        }

        int max_slice = Slice.Max();

        for (int j = 10 - RPkg_height -max_slice; j < 10-max_slice; j++)
        {
            for (int i = position; i < position+RPkg_width; i++)
            {
                 after_placing[j,i] = 1; 
            }
        }

        //Debug.Log("1:" + after_placing[2,0]);
        //Debug.Log("2:" + after_placing[0,3]);
        //Debug.Log("3:" + after_placing[3,3]);
        //Debug.Log("4:" + after_placing[3,4]);

        return after_placing;
    }

    int Aggregate_height(int[,] Placed_matrix)
    {
        int Aggregate = 0;
        for(int j=0;j<20; j++)
            for (int i = 0; i< 10; i++)
            {
                if (Placed_matrix[i, j] == 1)
                {
                    Aggregate += 10-i;
                    //Debug.Log("INSIDE AGG:" + i);
                    break;
                }
            }
        return Aggregate;
    }

    int Complete_lines(int[,] Placed_matrix)
    {
        int lines = 0;
        for (int i = 0; i < 10; i++)
        {
            int flag = 1;
            for (int j = 0; j < 20; j++)
            {
                if (Placed_matrix[i, j] == 0)
                {
                    flag = 0;
                    break;
                }

            }
            if (flag == 1)
                lines++;
        }
        return lines;
    }




    //Bad aggregate
    //int Aggregate_height(int [] Boxes_heights, int RPkg_height, int RPkg_width,int position)
    //{
    //    int raw_sum = Boxes_heights.Sum();
    //    int[] Slice=new int[4];
    //    int j = 0;
    //    for(int i = position; i < position + RPkg_width; i++)
    //    {
    //        Slice[j++] = Boxes_heights[i];
    //    }
    //    int max_slice = Slice.Max();
    //    int Aggregate = (max_slice+RPkg_height) * RPkg_width + raw_sum - Slice.Sum();
    //    return Aggregate;     
    // }




    // Update is called once per frame
    void Update () {
		
	}
}
