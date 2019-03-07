﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.i.CloseMenu();
        GameManager.i.StartGame();
    }
}
