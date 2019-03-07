using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishlineGenerator : MonoBehaviour
{
    private GameObject finishLine;
    public GameObject finishLinePrefab;

    public void GenerateLine()
    {
        if (finishLine != null)
        {
            Destroy(finishLine);
        }
        finishLine = Instantiate(finishLinePrefab);
        finishLine.transform.position = new Vector3(0, 0, GameManager.i.carpetLength);
    }
}
