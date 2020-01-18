using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    
    List<GameObject> listOfObjects;
    List<List<GameObject>> masterList;
    public GameObject myPrefab;
    private GameObject center;
    Camera cam;

    // Public settings
    [SerializeField] public int nbObjPerLayer = 17;
    [SerializeField] public float distFromCenter = 0.4f;
    [SerializeField] public float rotSpeed = 0.72f;
    [SerializeField] public int nbLayers = 33;
    [SerializeField] public float randomNoise = 11f;

    // We have alternate variables in the code to test if user changes them
    // (in the Update() method)
    private int nbObjPerLayerInternal = -1;
    private float distFromCenterInternal = -1f;
    private float nbLayersInternal = -1f;
    private float randomNoiseInternal = -1f;



    void Start()
    {
        Application.targetFrameRate = 144;
        // initalize variables
        listOfObjects = new List<GameObject>();
        masterList = new List<List<GameObject>>();
        center = new GameObject("Center");
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure camera look at center of galaxy
        cam.transform.LookAt(center.transform);

        // Make galaxy rotate
        center.transform.Rotate(0f, rotSpeed * 4f * Time.deltaTime, rotSpeed * 4f *Time.deltaTime);

        // Test if changes occur in Inspector for the corresponding settings, and execute necessary methods
        if(nbObjPerLayer != nbObjPerLayerInternal || nbLayers != nbLayersInternal || randomNoise != randomNoiseInternal)
        {
            randomNoiseInternal = randomNoise;
            nbObjPerLayerInternal = nbObjPerLayer;
            nbLayersInternal = nbLayers;
            updateSphereList();
        }

        if(distFromCenter != distFromCenterInternal)
        {
            distFromCenterInternal = distFromCenter;
            updateDistances();

        }



    }

    void updateDistances()
    {
        int j = 0;
        foreach (var list in masterList)
        {
            int i = 0;
            float rad0 = 2 * Mathf.PI * j / nbLayersInternal;
            foreach(var obj in list)
            {
                float rad = 2 * Mathf.PI * i / nbObjPerLayerInternal;
                obj.transform.position = new Vector3(
                    (center.transform.position.x + distFromCenter * Mathf.Cos(rad)) * distFromCenter * Mathf.Tan(rad0) + Random.Range(-randomNoise, randomNoise),
                    (center.transform.position.y + distFromCenter * Mathf.Sin(rad)) * distFromCenter * Mathf.Tan(rad0) + Random.Range(-randomNoise, randomNoise),
                    center.transform.position.z + Random.Range(-randomNoise, randomNoise)) ;
                
                i++;
            }
            j++;
        }


    }
    
    void updateSphereList()
    {

        foreach (var list in masterList)
        {
            foreach (var obj in list)
            {
                Destroy(obj);
            }
        }
        masterList.Clear();


        for(int j = 0; j< nbLayersInternal; j++)
        {
            List<GameObject> newList = new List<GameObject>();
            for (int i = 0; i < nbObjPerLayerInternal; i++)
            {
                GameObject obj = Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity, center.transform);
                obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                newList.Add(obj);
            }
            masterList.Add(newList);
        }
        updateDistances();

    }

}
