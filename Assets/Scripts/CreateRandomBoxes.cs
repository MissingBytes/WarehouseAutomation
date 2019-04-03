using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateRandomBoxes : MonoBehaviour {
    public Material cube_color;
    public Material RPkg_color;

    public GameObject [] Random_Package;
    public static bool [] rotated = new bool[4];
    public static Vector3 [] dest= new  Vector3[5];    // Use this for initialization



    void Start () {
        System.Random rand = new System.Random();
        int [] Boxes_heights = new int[20];

        int[,] Boxes_matrix = new int[10, 20];

        int[] fixed_heights = new int[20] {2,3,5,2,6,5,3,3,4,5,6,5,5,3,2,2,5,4,5,3 };

        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // obj.AddComponent<Rigidbody>();

                int height = rand.Next(2, 6);//fixed_heights[i];//
                Boxes_heights[i] = height;
                obj.transform.localScale = new Vector3(1, height, 2);//breadth is const =2
                obj.gameObject.name = "Exisiting_Pacakge:"+(i + 1).ToString();

                obj.transform.position = new Vector3(i - 10, (height / 2f)+0.25f, 9+j*-10);
                obj.GetComponent<Renderer>().material = cube_color;
            }
        }

        //LOOP for next box placement
        for(int n=0;n<4;n++)
        {

            for (int j = 0; j < 20; j++)
            {
                int k = Boxes_heights[j];
                for (int i = 0; i < 10; i++)
                {
                    if (i >= 10 - k)
                        Boxes_matrix[i, j] = 1; //1 if a block is present 0 if not.
                }
            }


            //Random_Package = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Random_Package[n].SetActive(true);
            // obj.AddComponent<Rigidbody>();

            int RPkg_height = rand.Next(1, 4);
            int RPkg_width = rand.Next(1, 4);

            //int RPkg_height = 3;
            //int RPkg_width = 1;

            Random_Package[n].transform.localScale = new Vector3(RPkg_width, RPkg_height, 2);
            Random_Package[n].transform.position = new Vector3(-5+5*n, (RPkg_height / 2f) + 0.25f, -10);
            //Random_Package.gameObject.name = "Random_Package";


            int[,] afterPlacing;

            float[] Scores = new float[40];

            for (int i = 0; i < Scores.Length; i++)
                Scores[i] = float.MinValue;

            for (int i = 0; i < 20 - RPkg_width; i++)
            {
                afterPlacing = Placed_matrix(Boxes_matrix, Boxes_heights, RPkg_height, RPkg_width, i);

                Scores[i] = -0.8988208556147149f * Aggregate_height(afterPlacing)
                               + 0.8596853874489735f * Complete_lines(afterPlacing)
                               - 0.814546246566033f * Holes(afterPlacing)
                               - 0.4027417007122302f * Bumpiness(afterPlacing);
               // Debug.Log("SCORE::" + i + ":" + Scores[i]);


            }

            if (RPkg_width != RPkg_height)
                for (int i = 0; i < 20 - RPkg_height; i++)
                {
                    afterPlacing = Placed_matrix(Boxes_matrix, Boxes_heights, RPkg_width, RPkg_height, i);

                    Scores[i + 20] = -0.8988208556147149f * Aggregate_height(afterPlacing)
                                   + 0.8596853874489735f * Complete_lines(afterPlacing)
                                   - 0.814546246566033f * Holes(afterPlacing)
                                   - 0.4027417007122302f * Bumpiness(afterPlacing);
                   // Debug.Log("SCORE::" + (i + 20) + ":" + Scores[i + 20]);

                }

            int position = Max_index(Scores);

            // position = 21;

            if (position < 20)
            {
                int[] Slice = new int[RPkg_width];

                int m = 0;
                for (int i = position; i < position + RPkg_width; i++)
                {
                    Slice[m++] = Boxes_heights[i];

                }
                int max_slice = Slice.Max();

                for (int i = position; i < position + RPkg_width; i++)
                {
                    Boxes_heights[i] = max_slice + RPkg_height;

                }


                //Random_Package.transform.position = new Vector3(-10 + position + RPkg_width / 2f - 0.5f, (RPkg_height / 2f) + 0.25f + max_slice, 9);
                dest[n] = new Vector3(-10 + position + RPkg_width / 2f - 0.5f, (RPkg_height / 2f) + 0.25f + max_slice, 6);
            }

            else
            {
                rotated[n] = true;
                position -= 20;
                int[] Slice = new int[RPkg_height];

                int m = 0;
                for (int i = position; i < position + RPkg_height; i++)
                {
                    Slice[m++] = Boxes_heights[i];
                }
                int max_slice = Slice.Max();

                for (int i = position; i < position + RPkg_height; i++)
                {
                    Boxes_heights[i] = max_slice + RPkg_width;
                }
                Random_Package[n].transform.Rotate(0, 0, 90);
                //Random_Package.transform.position = new Vector3(-10 + position + RPkg_height / 2f - 0.5f, (RPkg_width / 2f) + 0.25f + max_slice, 9);
                dest[n] = new Vector3(-10 + position + RPkg_height / 2f - 0.5f, (RPkg_width / 2f) + 0.25f + max_slice, 6);
            }
            Debug.Log("POSITION:" + (position + 1) + " Rotated:" + rotated);

        }//END OF PLACEMENT

    }




    //How the matrix will look like after placing the box onto the position
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

    int Holes(int[,] Placed_matrix)
    {
        int result = 0;

        for (int j = 0; j < 20; j++)
            for (int i = 1; i < 8; i++)
            {
                if (Placed_matrix[i, j] == 0)
                {   for (int k = i - 1; k > 0; k--)
                    {
                        if (Placed_matrix[k, j] == 1)
                        {
                            result++;
                            break;
                        }
                    }
                }
            }

        return result;
    }


    int Bumpiness(int[,] Placed_matrix)
    {

        int[] Boxes_heights = new int[20];

        for (int j = 0; j < 20; j++)
            for (int i = 0; i < 10; i++)
            {
                if (Placed_matrix[i, j] == 1)
                {
                    Boxes_heights[j]= 10 - i;
                    break;
                }
            }


        int result=0;
        for(int i = 1; i < Boxes_heights.Length; i++)
        {
            result += Math.Abs(Boxes_heights[i] - Boxes_heights[i - 1]);
        }

        return result;
    }

    int Max_index(float [] arr)
    {
        float maxValue = arr.Max();
        int maxIndex = arr.ToList().IndexOf(maxValue);

        return maxIndex;
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
