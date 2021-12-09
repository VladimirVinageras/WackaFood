using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI wellDoneText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public Slider timeSlider;
    public Slider volumeSlider;

    public List<GameObject> targetPrefabs;

    private int score;
    private int time;
    private float spawnRate = 1.5f;
    private bool isGameActive;
    public bool IsGameActive
    {
        get => isGameActive;
        set => isGameActive = value;
    }

    private bool isGamePaused;
    public bool IsGamePaused{
        get => isGamePaused;
        set => isGamePaused = value;
    }
    

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked


    public void Start()
    {
        titleScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        timeSlider.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GamePauseManage();
        }

        if (titleScreen)
        {
            VolumeManage();
        }
    }
    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        time = 60;
        UpdateScore(0);
        titleScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        timeSlider.gameObject.SetActive(true);
        StartCoroutine(Countdown());
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
            
        }
    }

    IEnumerator Countdown()
    {
        while (time>=0)
        {
            timeText.text = "Time: " + time;
            timeSlider.value--;
            yield return new WaitForSeconds(1);
           if (time == 0)
            {
                isGameActive = false;
                StartCoroutine(Congratulation());
            } 
           time--;
        }
    }

    IEnumerator Congratulation()
    {
      
        wellDoneText.gameObject.SetActive(true);
        Debug.Log("se debe mostrar el texto");
       yield return new WaitForSeconds(2);
       Debug.Log("debe mostarse el game over" );
       GameOver();
       
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        wellDoneText.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void GamePauseManage()
    {
        if (isGamePaused && isGameActive)
        {
            pauseScreen.gameObject.SetActive(false);
            isGamePaused = false;
            Time.timeScale = 1;
        }
        else if(isGameActive)
        {
            pauseScreen.gameObject.SetActive(true);
            isGamePaused = true;
            Time.timeScale = 0;
        }
    }

    void VolumeManage()
    {
        AudioSource _audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        _audioSource.volume= volumeSlider.value;
    }

}
