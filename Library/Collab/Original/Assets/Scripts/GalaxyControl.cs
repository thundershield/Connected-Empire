using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyControl : MonoBehaviour
{
    private int numberOfStars = 50;
    private GameObject[] stars = new GameObject[50];
    private GameObject[,] node = new GameObject[50, 50];
    public GameObject star;
    public GameObject nodeLine;
    private bool toClose = false;
    private bool connected = false;
    private int limit = 0;
    private Color[] starTypes = new Color[4];

public GameObject[] Stars
    {
        get
        {
            return stars;
        }

        set
        {
            stars = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        starTypes[0] = new Color(0.34f,0.53f,.76f,1); //blue-white changed from .37, .31, 1
        starTypes[1] = new Color(0.55f,0.25f,0.09f,1); //dark red changed from .27, .01, .01, 
        starTypes[2] = new Color(1f,0.85f,0.26f,1); //sun yellow
        starTypes[3] = new Color(1f,0.87f,0.32f); //white

        Generate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Generate()
    {
        //main loop for placing stars
        for (int i = 0; i < stars.Length; i++)
        {
            limit = 0;
            stars[i] = Instantiate(star);
            while (true)
            {
                toClose = false;
                //place a star randomly within a unit sphere
                stars[i].transform.position = Random.insideUnitSphere * 4;
                for (int j = 0; j < i; j++)
                {
                    //break in the event that the placed star is too close to another
                    if (Vector3.Distance(stars[i].transform.position, stars[j].transform.position) < .5)
                    {
                        toClose = true;
                        break;
                    }
                }
                if (toClose == false)
                {
                    break;
                }
                limit++;
                if (limit > 50)
                {
                    break;
                }
            }

            //randomize star quality and name star instance
            stars[i].GetComponent<StarControl>().quality = ((int)(Random.value * 100));
            stars[i].name = "star" + i;

            //get collider size before scale is applied
            float absoluteSize = stars[i].GetComponent<Collider>().bounds.size.x;

            //this variable is used in a few lines
            float starBrightness = Random.Range(0.1f, 50f);

            //weighted randomization of color
            int starColor = Random.Range(0, 10);
            if (starColor < 3)
                starColor = 1;
            else if (starColor < 6)
                starColor = 2;
            else if (starColor < 9)
                starColor = 3;
            else
                starColor = 0;

            //shader properties
            stars[i].GetComponent<Renderer>().material.SetFloat("_Brightness", Random.value * 50 + 1);
            stars[i].GetComponent<Renderer>().material.SetColor("_StarColor", starTypes[(int)(starColor)]);
            stars[i].GetComponent<Renderer>().material.SetFloat("_SunspotSpeed", Random.Range(0.2f,10f));

            float starSize = Random.Range(0.1f,0.15f);//changed from .05-.23
            stars[i].GetComponent<Transform>().localScale = new Vector3(starSize,starSize,starSize);

            //stars that have a high brightness get a slightly larger range;
            //also vice versa
            float brightnessModifier = 1;

            if (starBrightness < 10)
                brightnessModifier = 0.1f;
            else if (starBrightness < 25)
                brightnessModifier = 0.3f;
            else if (starBrightness < 40)
                brightnessModifier = 0.5f;
            else
                brightnessModifier = 0.5f;

            //some stars appear more bright than others
            if (starColor==1)
                stars[i].GetComponent<Light>().range = ((absoluteSize * starSize) + brightnessModifier);
            else if (starColor==2)
                stars[i].GetComponent<Light>().range = ((absoluteSize * starSize) +);
            else if (starColor==3)
                stars[i].GetComponent<Light>().range = ((absoluteSize * starSize) + 0.5f);
            else
                stars[i].GetComponent<Light>().range = ((absoluteSize * starSize) + 0.5f);

            stars[i].GetComponent<Light>().color = starTypes[starColor];
        }

        //this code generates the nodeline placement
        //first for loop goes through all stars
        for (int i = 0; i < stars.Length; i++)
        {
            //closest stores the distances of the closests stars
            float[] closest = new float[(int)(Random.value * 2)+1];
            //tempRef stores the references to the other stars
            int[] tempRef = new int[closest.Length];
            //second for loop goes through and sets closest to max so any star will validate
            for (int j = 0; j < closest.Length; j++)
            {
                closest[j] = int.MaxValue;
            }
            //third loop goes through and runs the code to check the distance for each other star
            for (int j = 0; j < stars.Length; j++)
            {
                connected = false;
                //checks to see if it is the same star
                if (i == j)
                    connected = true;
                //checks wether the stars are already connected
                for (int k = 0; k < stars[j].GetComponent<StarControl>().primaryNode.Count; k++)
                {
                    if (stars[j].GetComponent<StarControl>().primaryNode[k] == i)
                    {
                        connected = true;
                        break;
                    }
                }
                if (connected == false)
                {
                    //loop that checks if star j is closer than any other star already on the list
                    for (int k = 0; k < closest.Length; k++)
                    {
                        //checks if star j is closer than than star closest k
                        if (closest[k] > Vector3.Distance(stars[i].transform.position, stars[j].transform.position))
                        {
                            //shifts all stars farther than j up one in the array
                            for (int m = closest.Length - 1; m >= k; m--)
                            {
                                if (m < (closest.Length - 1))
                                {
                                    closest[m + 1] = closest[m];
                                    tempRef[m + 1] = tempRef[m];
                                }
                            }
                            closest[k] = Vector3.Distance(stars[i].transform.position, stars[j].transform.position);
                            tempRef[k] = j;
                            //breaks out of loop k so that each star will only be added once
                            break;
                        }
                    }
                }
            }
            //adds tempRef to star i's primary list and adds star i's reference to each star in tempRef's secondary list
            foreach (int j in tempRef)
            {
                stars[i].GetComponent<StarControl>().primaryNode.Add(j);
                stars[j].GetComponent<StarControl>().secondaryNode.Add(i);

                node[i, j]=Instantiate(nodeLine);
                node[i, j].transform.position = (stars[j].transform.position);
                node[i, j].transform.localScale += new Vector3(0, 0, Vector3.Distance(stars[i].transform.position, stars[j].transform.position));
                node[i, j].transform.LookAt(stars[i].transform.position, new Vector3(0,0,0));
            }
        }
    }

    public void Save()
    {

    }
    public void Load()
    {

    }
}
