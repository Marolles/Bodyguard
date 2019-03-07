using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject menuPanel;
    public Slider progressionSlider;
    public GameObject progressionUI;
    [HideInInspector] public FinishlineGenerator finishLineGenerator;

    [Range(100,2000)]
    public float carpetLength;

    [Range(0,10)]
    public float enemyspawnRate;

    public float timeBeforeFirstEnemy = 3f;


    private void Start()
    {
        i = this;
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        canvas = FindObjectOfType<Canvas>();
        starBehaviour = FindObjectOfType<StarBehaviour>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        finishLineGenerator = FindObjectOfType<FinishlineGenerator>();
        OpenMenu();
    }

    private void Update()
    {
        UpdateProgression();
    }

    void UpdateProgression()
    {
        progressionSlider.value = starBehaviour.transform.position.z / carpetLength;
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

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        progressionUI.SetActive(false);
        cameraController.target = starBehaviour.gameObject;
        cameraController.GetComponent<Animator>().SetTrigger("reset");
    }
    
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        cameraController.GetComponent<Animator>().SetTrigger("start");
    }

    public void StopGame()
    {
        enemySpawner.ToggleSpawn(false);
        starBehaviour.ToggleStar(false);
    }

    public void StartGame()
    {
        cameraController.target = playerController.gameObject;
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        enemySpawner.ToggleSpawn(true);
        starBehaviour.ToggleStar(false);
        starBehaviour.ToggleStar(true);
        playerController.ResetPlayer();
        finishLineGenerator.GenerateLine();
        progressionUI.SetActive(true);
    }
}
