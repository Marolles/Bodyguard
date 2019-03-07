using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager i;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public CameraController cameraController;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public StarBehaviour starBehaviour;
    [HideInInspector] public EnemySpawner enemySpawner;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    [HideInInspector] public FinishlineGenerator finishLineGenerator;

    [Range(100,2000)]
    public float carpetLength;

    [Range(0,10)]
    public float enemyspawnRate;


    private void Start()
    {
        i = this;
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        canvas = FindObjectOfType<Canvas>();
        starBehaviour = FindObjectOfType<StarBehaviour>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        finishLineGenerator = FindObjectOfType<FinishlineGenerator>();

        StartGame();
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        StopGame();
    }

    public void LoseGame()
    {
        gameOverPanel.SetActive(true);
        StopGame();
    }

    public void StopGame()
    {
        enemySpawner.ToggleSpawn(false);
        starBehaviour.ToggleStar(false);
    }

    public void StartGame()
    {
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        enemySpawner.ToggleSpawn(true);
        starBehaviour.ToggleStar(true);
        starBehaviour.transform.position = new Vector3(0, 1, 0);
        playerController.transform.position = new Vector3(0, 1, -5);
        playerController.ResetPlayer();
        finishLineGenerator.GenerateLine();
    }
}
