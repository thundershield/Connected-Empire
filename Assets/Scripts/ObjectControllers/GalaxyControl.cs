using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GalaxyControl : MonoBehaviour
{
    private int numberOfStars = 50;
    private int numberOfEmpires = 3;

    private GameObject[] stars = new GameObject[50];
    private GameObject[,] node = new GameObject[50, 50];
    private GameObject[] empires = new GameObject[3];
    private GameObject[] labels = new GameObject[50];

    public GameObject star;
    public GameObject nodeLine;
    public GameObject fleet;
    public GameObject empire;
    public GameObject label;
    public GameObject camera;
    public List<Ship> ships = new List<Ship>();

    public static GalaxyControl control;
    private bool tooClose = false;
    private bool connected = false;
    private int limit = 0;

    private Color[] starTypes = new Color[4];
    private float starSizeMod;

    public GameObject[] Stars{get; set;}

    // Use this for initialization
    private void Awake()
    {
        control = this;
    }

    void Start()
    {
        starSizeMod = star.GetComponent<Renderer>().bounds.size[1];
        starTypes[0] = new Color(0.34f,0.53f,.76f,1); //blue-white changed from .37, .31, 1
        starTypes[1] = new Color(0.55f,0.25f,0.09f,1); //dark red changed from .27, .01, .01, 
        starTypes[2] = new Color(1f,0.85f,0.26f,1); //sun yellow
        starTypes[3] = new Color(1f,0.87f,0.32f); //white

        Generate();

        stars[1].GetComponent<StarControl>().fleets.Add(Instantiate(fleet));
        stars[1].GetComponent<StarControl>().fleets[0].GetComponent<FleetControl>().ships.Add(ships[0]);
        for (int i = 0; i < empires.Length; i++)
        {
            empires[i] = Instantiate(empire);
            empires[i].name = "empire" + i;
            empires[i].GetComponent<EmpireControl>().starSystems.Add(stars[(int)(50 * Random.value)]);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Randomly assigns star attributes such as brightness, color, size, etc.
    /// </summary>
    /// <param name="star"></param>
    private void RandomizeAttributes(GameObject star)
    {
        float starBrightness = Random.Range(0.1f, 50f);

        //weighted randomization of color
        int starColor = Random.Range(0, 10);
        if (starColor < 3) starColor = 1;
        else if (starColor < 6) starColor = 2;
        else if (starColor < 9) starColor = 3;
        else starColor = 0;

        star.GetComponent<Renderer>().material.SetFloat("_Brightness", starBrightness);
        star.GetComponent<Renderer>().material.SetColor("_StarColor", starTypes[starColor]);
        star.GetComponent<Renderer>().material.SetFloat("_SunspotSpeed", Random.Range(0.2f, 10f));

        float starSize = Random.Range(0.1f, 0.15f);//changed from .05-.23
        star.GetComponent<Transform>().localScale = new Vector3(starSize, starSize, starSize);

        //stars that have a high brightness get a slightly larger range;
        //also vice versa
        float brightnessModifier = 1;
        brightnessModifier = starBrightness / 25;
        if (starColor == 1)
        {
            star.GetComponent<Light>().range = (starSize * starSizeMod + (0.2f * brightnessModifier) / 2);
            star.GetComponent<Light>().color = starTypes[starColor] / 2;
        }
        else if (starColor == 2)
        {
            star.GetComponent<Light>().range = (starSize * starSizeMod + (0.3f * brightnessModifier) / 2);
            star.GetComponent<Light>().color = starTypes[starColor] / 3;
        }
        else if (starColor == 3)
        {
            star.GetComponent<Light>().range = (starSize * starSizeMod + (0.4f * brightnessModifier) / 2);
            star.GetComponent<Light>().color = starTypes[starColor] / 3;
        }
        else
        {
            star.GetComponent<Light>().range = (starSize * starSizeMod + (0.5f * brightnessModifier) / 2);
            star.GetComponent<Light>().color = starTypes[starColor] / 3;
        }
    }

    /// <summary>
    /// Responsible for creating the nodeline objects that connect stars together.
    /// </summary>
    private void GenerateNodelines()
    {
        //this code generates the nodeline placement
        //first for loop goes through all stars
        for (int i = 0; i < stars.Length; i++)
        {
            //closest stores the distances of the closests stars
            float[] closest = new float[(int)(Random.value * 2) + 1];
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
                //checks whether the stars are already connected
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

                node[i, j] = Instantiate(nodeLine);
                node[i, j].transform.position = (stars[j].transform.position);
                node[i, j].transform.localScale += new Vector3(0, 0, Vector3.Distance(stars[i].transform.position, stars[j].transform.position));
                node[i, j].transform.LookAt(stars[i].transform.position, new Vector3(0, 0, 0));
            }
        }
    }

    /// <summary>
    /// Main function for creating a galaxy. This will autogenerate a preset number of stars
    /// and call other functions to randomize their attributes, as well as nodeline placement.
    /// </summary>
    public void Generate()
    {
        //main loop for placing stars
        for (int i = 0; i < stars.Length; i++)
        {
            limit = 0;
            stars[i] = Instantiate(star);
            while (true)
            {
                tooClose = false;
                //place a star randomly within a unit sphere
                stars[i].transform.position = Random.insideUnitSphere * 4;
                for (int j = 0; j < i; j++)
                {
                    //break in the event that the placed star is too close to another
                    if (Vector3.Distance(stars[i].transform.position, stars[j].transform.position) < .5)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose == false)
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

            //creates label for star
            labels[i] = Instantiate(label);
            labels[i].transform.position = stars[i].transform.position;

            labels[i].GetComponent<StarNameTesting>().camera=camera;
            stars[i].GetComponent<StarInteraction>().label = labels[i];
            labels[i].GetComponentInChildren<Text>().text = "Star " + i;

            labels[i].transform.Find("Selected").gameObject.SetActive(false);
            labels[i].transform.Find("Clicked").gameObject.SetActive(false);

            RandomizeAttributes(stars[i]);
        }

        GenerateNodelines();
    }

    public void Save()
    {

    }

    public void Load()
    {

    }
}
