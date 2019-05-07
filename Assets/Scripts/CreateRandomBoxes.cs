using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateRandomBoxes : MonoBehaviour {
    public Material [] cube_color;
     Color[] RBox_colors = new Color[3] { Color.blue, Color.yellow, Color.green};
    public Material RPkg_color;

    public GameObject [] Random_Package;
    public static bool [] rotated = new bool[4];
    public static Vector3 [] dest= new  Vector3[5];    // Use this for initialization



    void Start () {
        System.Random rand = new System.Random();
        int [] Boxes_heights = new int[60];

        int[,] Boxes_matrix = new int[10, 60];

        int[] fixed_heights = new int[60] { 2, 2, 5, 4, 5, 3, 5, 3, 3, 4, 5, 6, 5, 5, 3, 2, 3, 5, 2, 6,
                                            5, 3, 3, 2, 3, 5, 2, 6, 4, 5, 2, 6, 5, 3, 3, 4, 5, 6, 5, 5,
                                            2,2,2,5,2,2,3,3,2,2  ,5,5,5,3,4,4,5,5,2,3};

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // obj.AddComponent<Rigidbody>();

                int height = rand.Next(2, 6);//fixed_heights[i + j * 20];//
                Boxes_heights[j*20+i] = height;
                obj.transform.localScale = new Vector3(1, height, 2);//breadth is const =2
                obj.gameObject.name = "Exisiting_Pacakge:"+(i + 1).ToString();

                obj.transform.position = new Vector3(i - 10, (height / 2f)+0.25f, 9+j*-10);
                obj.GetComponent<Renderer>().material = cube_color[j];
            }
        }

        //LOOP for next box placement
        for(int n=0;n<4;n++)
        {

            for (int j = 0; j < 60; j++) //j columns
            {
               // Debug.Log(n+" "+j+":"+Boxes_heights[j]);
                int k = Boxes_heights[j];
                for (int i = 0; i < 10; i++) // i th row
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
            //int RPkg_width = 2;


            Random_Package[n].transform.localScale = new Vector3(RPkg_width, RPkg_height, 2);
            Random_Package[n].transform.position = new Vector3(-10+7*n, (RPkg_height / 2f) + 0.25f, -25);
            int BYG_color = rand.Next(0, 3);//2
            Random_Package[n].GetComponent<Renderer>().material.color = RBox_colors[BYG_color];
            
            //Random_Package.gameObject.name = "Random_Package";


            int[,] afterPlacing;

            float[] Scores = new float[40];

            int Shelf = BYG_color+1;

            for (int i = 0; i < Scores.Length; i++)
                Scores[i] = float.MinValue;

            for (int i = 0; i <= 20 - RPkg_width; i++)
            {
                //Debug.Log("pos:"+i + 20 * (Shelf - 1));
                try
                {
                    afterPlacing = Placed_matrix(Boxes_matrix, Boxes_heights, RPkg_height, RPkg_width, i + 20 * (Shelf - 1));
                }

                catch
                {
                    continue;
                }
                Scores[i] = -0.8988208556147149f * Aggregate_height(afterPlacing, Shelf)
                               + 0.8596853874489735f * Complete_lines(afterPlacing, Shelf)
                               - 0.814546246566033f * Holes(afterPlacing, Shelf)
                               - 0.4027417007122302f * Bumpiness(afterPlacing, Shelf);
               // Debug.Log("SCORE:"+n+":" + i + ":" + Scores[i]);


            }


            if (RPkg_width != RPkg_height)
                for (int i = 0; i <= 20 - RPkg_height; i++)
                {
                    try
                    {
                        afterPlacing = Placed_matrix(Boxes_matrix, Boxes_heights, RPkg_width, RPkg_height, i + 20 * (Shelf - 1));
                    }

                    catch
                    {
                        continue;
                    }


                    Scores[i + 20] = -0.8988208556147149f * Aggregate_height(afterPlacing,Shelf)
                                   + 0.8596853874489735f * Complete_lines(afterPlacing, Shelf)
                                   - 0.814546246566033f * Holes(afterPlacing, Shelf)
                                   - 0.4027417007122302f * Bumpiness(afterPlacing, Shelf);
                    //Debug.Log("SCORE::" + (i + 20) + ":" + Scores[i + 20]);

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

                for (int i = position + 20 * (Shelf - 1); i < position + 20 * (Shelf - 1) + RPkg_width; i++)
                {
                    Boxes_heights[i] = max_slice + RPkg_height;

                }


                //Random_Package.transform.position = new Vector3(-10 + position + RPkg_width / 2f - 0.5f, (RPkg_height / 2f) + 0.25f + max_slice, 9);
                dest[n] = new Vector3(-10 + position + RPkg_width / 2f - 0.5f, (RPkg_height / 2f) + 0.25f + max_slice + 0.5f, 6 - 10 * BYG_color);
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

                for (int i = position + 20*(Shelf-1); i < position+ 20*(Shelf-1) + RPkg_height; i++)
                {
                    Boxes_heights[i] = max_slice + RPkg_width;
                }
                Random_Package[n].transform.Rotate(0, 0, 90);
                //Random_Package.transform.position = new Vector3(-10 + position + RPkg_height / 2f - 0.5f, (RPkg_width / 2f) + 0.25f + max_slice, 9);
                dest[n] = new Vector3(-10 + position + RPkg_height / 2f - 0.5f, (RPkg_width / 2f) + 0.25f + max_slice + 0.5f, 6 - 10 * BYG_color);
            }
            //Debug.Log("======================================================================");
            Debug.Log(" Shelf:" + (BYG_color + 1).ToString()+"    Position:" + (position + 1) + "    Rotated:" + rotated[n]);

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
        //Debug.Log("INSIDEPLACEDMAT:" + (position + RPkg_width -1));
       // Debug.Log("J:" + (10 - max_slice));
        for (int j = 10 - RPkg_height -max_slice; j < 10-max_slice; j++)
        {
            for (int i = position; i < position+RPkg_width; i++)
            {
                 after_placing[j,i] = 1; 
            }
        }

        return after_placing;
    }

    int Aggregate_height(int[,] Placed_matrix, int Shelf)
    {
        int Aggregate = 0;
        for(int j=20*(Shelf-1);j< 20 * (Shelf); j++)
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

    int Complete_lines(int[,] Placed_matrix, int Shelf)
    {
        int lines = 0;
        for (int i = 0; i < 10; i++)
        {
            int flag = 1;
            for (int j = 20 * (Shelf-1); j < 20 * (Shelf); j++)
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

    int Holes(int[,] Placed_matrix, int Shelf)
    {
        int result = 0;

        for (int j = 20 * (Shelf - 1); j < 20 * (Shelf); j++)
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


    int Bumpiness(int[,] Placed_matrix, int Shelf)
    {

        int[] Boxes_heights = new int[20];

       // for (int j = 20 * (Shelf - 1); j < 20 * (Shelf); j++)
        for (int j = 0; j < 20 ; j++)
            for (int i = 0; i < 10; i++)
            {
                if (Placed_matrix[i, 20 * (Shelf - 1)+j] == 1)
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
