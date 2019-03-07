using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteProp : MonoBehaviour
{
    public Vector3 forwardVector;
    float propAmount;
    public float maxRange;
    List<GameObject> propList;
    public GameObject referenceProp;

    private int propCount;

    private void Start()
    {
        propList = new List<GameObject>();
        propList.Add(referenceProp);
    }
    private void Update()
    {
        if (Vector3.Distance(propList[propList.Count-1].transform.position, GameManager.i.playerController.transform.position) < maxRange)
        {
            GameObject newProp = Instantiate(referenceProp,this.transform);
            newProp.transform.position = propList[propList.Count-1].transform.position + forwardVector;
            propList.Add(newProp);
        } else if (Vector3.Distance(propList[propList.Count - 1].transform.position, GameManager.i.playerController.transform.position) > maxRange + forwardVector.magnitude)
        {
            GameObject propFound = propList[propList.Count -1];
            propList.RemoveAt(propList.Count - 1);
            if (propFound != referenceProp) { Destroy(propFound); }
        }

        
        if (Vector3.Distance(propList[0].transform.position, GameManager.i.playerController.transform.position) < maxRange && (propList[0].transform.position - forwardVector/2).z > 0)
        {
            GameObject newProp = Instantiate(referenceProp, this.transform);
            newProp.transform.position = propList[0].transform.position - forwardVector;
            propList.Insert(0, newProp);
        }
        else if(Vector3.Distance(propList[0].transform.position, GameManager.i.playerController.transform.position) > maxRange + forwardVector.magnitude)
        {
            GameObject propFound = propList[0];
            propList.RemoveAt(0);
            if (propFound != referenceProp) { Destroy(propFound); }
        }
    }
}
