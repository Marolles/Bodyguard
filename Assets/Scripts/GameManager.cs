using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    public PlayerController playerController;
    public CameraController cameraController;
    public Canvas canvas;
    public StarBehaviour starBehaviour;
    public EnemySpawner enemySpawner;
    public GameObject gameOverPanel;

    private void Start()
    {
        i = this;
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        canvas = FindObjectOfType<Canvas>();
        starBehaviour = FindObjectOfType<StarBehaviour>();
        enemySpawner = FindObjectOfType<EnemySpawner>();

        StartGame();
    }

    public void StopGame()
    {
        gameOverPanel.SetActive(true);
        enemySpawner.ToggleSpawn(false);
        starBehaviour.ToggleStar(false);
    }

    public void StartGame()
    {
        gameOverPanel.SetActive(false);
        enemySpawner.ToggleSpawn(true);
        starBehaviour.ToggleStar(true);
        starBehaviour.transform.position = new Vector3(0, 1, 0);
        playerController.transform.position = new Vector3(0, 1, -5);
    }
}
