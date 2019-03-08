using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartPanel : MonoBehaviour
{
    public void RestartButton()
    {
        GameManager.i.StartGame();
    }
}
