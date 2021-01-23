using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ANNDrive : MonoBehaviour
{
    ANN ann; //Introducing Neural Network.
    public float visibleDistance = 50;
    public int epochs = 1000; //The times we are going to run the training data through, before testing to see how good we have done. 
    public float speed = 50.0f;
    public float rotationSpeed = 100.0f;

    bool trainingDone = false;
    float trainingProgress = 0; //On our screen to see how far in our epochs we are. 
    double sse = 0; //Sum of squared errors. 
    double lastSSE = 1; //Last sum of squared errors. 

    public float translation; //SO we can see them in the inspector when the game is running. 
    public float rotation; //Used them for demonstration purposes. 

    public bool loadFromFile = true; 

    // Start is called before the first frame update
    void Start()
    {
        ann = new ANN(5, 2, 1, 10, 0.5); //5 inputs, 2 outputs, hidden layers = 1, number of neurons in layer = 10, alpha value = 0.5.
        if (loadFromFile) //If it`s true than we are going to load trained kart from the file O:) ARCADE AI :) 
        {
            LoadWeightsFromFile();
            trainingDone = true; //Will go straight into our update for us. 
        }
        else
        {
            StartCoroutine(LoadTrainingSet());
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 250, 30), "SSE: " + lastSSE);
        GUI.Label(new Rect(25, 40, 250, 30), "Alpha: " + ann.alpha);
        GUI.Label(new Rect(25, 55, 250, 30), "Trained: " + trainingProgress);
    }

    IEnumerator LoadTrainingSet()
    {
        string path = Application.dataPath + "/trainingData.txt";
        string line;
        if (File.Exists(path))
        {
            int lineCount = File.ReadAllLines(path).Length;
            StreamReader tdf = File.OpenText(path); //Going to open our training data.
            List<double> calcOutputs = new List<double>(); //Gets what the neural network is calculating to send back.
            List<double> inputs = new List<double>(); //Input values.
            List<double> outputs = new List<double>(); //Output values. 

            for (int i = 0; i < epochs; i++) //Going to go through this code 1000 of times. 
            {
                //Setting file pointer to the beggining of the file. 
                sse = 0;
                tdf.BaseStream.Position = 0; //This is so when we loop to the end, we can come back to the start. 

                string currentWeigths = ann.PrintWeights(); //Going to store current weights from current state of neural networks. 

                while ((line = tdf.ReadLine()) != null) //Reading 1 line at a time until it runs out of lines. 
                {
                    string[] data = line.Split(','); //Taking line from the file and going to split it on the comma. 
                    //If nothing to be learned, we going to ignore this line.
                    float thisError = 0; //Calculating error that we get from our training. 
                    if (System.Convert.ToDouble(data[5]) != 0 && System.Convert.ToDouble(data[6]) != 0) //Data in position 5 and 6 == 0 that is translation and rotation.
                    {
                        inputs.Clear();
                        outputs.Clear();
                        inputs.Add(System.Convert.ToDouble(data[0])); //Adding values for our distance. Straight. 5 input values coming out from our training data. 
                        inputs.Add(System.Convert.ToDouble(data[1])); //Right
                        inputs.Add(System.Convert.ToDouble(data[2])); //Left
                        inputs.Add(System.Convert.ToDouble(data[3])); //Right45
                        inputs.Add(System.Convert.ToDouble(data[4])); //Left45

                        double o1 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[5])); //Output values that is translation. 
                        outputs.Add(o1);
                        double o2 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[6])); //Output values that is rotation.
                        outputs.Add(o2);

                        calcOutputs = ann.Train(inputs, outputs); //training with inputs and outputs. We are getting 2 outputs (Translation and rotation). 
                        thisError = ((Mathf.Pow((float)(outputs[0] - calcOutputs[0]), 2) + Mathf.Pow((float)(outputs[1] - calcOutputs[1]), 2))) / 2.0f;
                        //Calculating sum of squared errors on the 1st output and as well as on the 2nd output. 
                    }
                    sse += thisError;
                }
                trainingProgress = (float)i / (float)epochs; //i is epochs loop counter. Going to give percentage value trained. 
                sse /= lineCount; //Giving us average of errors. 

                //if sse isnt better then reload previous set of weights and declare alpha. 
                if (lastSSE < sse)
                {
                    ann.LoadWeights(currentWeigths);
                    ann.alpha = Mathf.Clamp((float)ann.alpha - 0.001f, 0.01f, 0.9f); //Going to decrease it by 0.001, clamp to make sure it doesnt go negative or more than 1.
                }
                else //Increase alpha
                {
                    ann.alpha = Mathf.Clamp((float)ann.alpha + 0.001f, 0.01f, 0.9f);
                    lastSSE = sse;
                }

                yield return null;
            }
        }

        trainingDone = true;
        SaveWeightsToFile();
    }

    void SaveWeightsToFile() //Writing stuff into file
    {
        string path = Application.dataPath + "/weights.txt"; //Dumping it into file known as weights.txt
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(ann.PrintWeights()); //Using ann.printweights, gives us string with commas between. 
        wf.Close();
    }

    void LoadWeightsFromFile()
    {
        string path = Application.dataPath + "/weights.txt"; //Loading from file. 
        StreamReader wf = File.OpenText(path);

        if (File.Exists(path)) //Check if it exists.
        {
            string line = wf.ReadLine();
            ann.LoadWeights(line);
        }
    }

    float Map(float newfrom, float newto, float origfrom, float origto, float value) //To map our values from negative 1 to 0 and 1.
    {
        if (value <= origfrom)
        {
            return newfrom;
        }
        else if (value >= origto)
        {
            return newto;
        }
        return (newto - newfrom) * ((value - origfrom) / (origto - origfrom)) + newfrom;
    }

    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!trainingDone) //This is just preventing update from starting and trying to drive the car when the neural network hasnt finished training. 
        {
            return;
        }

        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>(); 

        RaycastHit hit;
        float fDist = 0, rDist = 0,
              lDist = 0, r45Dist = 0, l45Dist = 0;
        
        //Foward
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
            fDist = 1 - Round(hit.distance / visibleDistance); //Performing normalization, will give something between 0 and 1. 
        }

        //Right
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
        }

        //Left
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
        }

        //Right 45
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * this.transform.right, out hit, visibleDistance))
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
        }

        //Left 45
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
        }

        inputs.Add(fDist); //Sending these values as input in our neural network. 
        inputs.Add(rDist);
        inputs.Add(lDist);
        inputs.Add(r45Dist);
        inputs.Add(l45Dist);
        outputs.Add(0); //Outputs are just placeholder, not really gonna use them. 
        outputs.Add(0);

        calcOutputs = ann.CalcOutput(inputs, outputs); //calculating outputs, will be in range of 0 and 1 because that is what I wanted. 

        float translationInput = Map(-1, 1, 0, 1, (float)calcOutputs[0]); //Mapping them and converting them back to between -1 and 1. 
        float rotationInput = Map(-1, 1, 0, 1, (float)calcOutputs[1]); //This is same as how bhuman player plays and presses keys.
        //Now we are getting it from neural network for our NPC kart (car). 

        translation = translationInput * speed * Time.deltaTime; //Calculated exactly the same way as they were before. 
        rotation = rotationInput * rotationSpeed * Time.deltaTime;

        this.transform.Translate(0, 0, translation); 
        this.transform.Rotate(0, rotation, 0);
    }
}
