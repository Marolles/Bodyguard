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
    [HideInInspector] public EmojiController emojiController;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject menuPanel;
    public Slider progressionSlider;
    public GameObject progressionUI;
    public int paparazziKilled;
    public int interviewerKilled;
    [HideInInspector] public FinishlineGenerator finishLineGenerator;

    [Range(10,2000)]
    public float carpetLength;

    [Range(0,10)]
    public float enemyspawnRate;

    public float timeBeforeFirstEnemy = 3f;


    private void Start()
    {
        i = this;
        emojiController = FindObjectOfType<EmojiController>();
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        canvas = FindObjectOfType<Canvas>();
        starBehaviour = FindObjectOfType<StarBehaviour>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        finishLineGenerator = FindObjectOfType<FinishlineGenerator>();
        playerController.canMove = false;
        OpenMenu();
    }

    private void Update()
    {
        UpdateProgression();
    }

    void ResetScore()
    {
        paparazziKilled = 0;
        interviewerKilled = 0;
    }

    void UpdateProgression()
    {
        progressionSlider.value = starBehaviour.transform.position.z / carpetLength;
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        winPanel.GetComponent<WinPanelScript>().AnimatePanel();
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
        playerController.canMove = false;
        enemySpawner.ToggleSpawn(false);
        starBehaviour.ToggleStar(false);
        cameraController.InCinematic();
    }

    public void StartGame()
    {
        cameraController.NotInCinematic();
        ResetScore();
        playerController.canMove = true;
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
