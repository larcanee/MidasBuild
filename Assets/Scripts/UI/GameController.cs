using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject winScreen;
    public GameObject loseScreen;
    public SpawnManager spawnManager;
    public int maxGear = 1;
    public int gearsCollected = 0;
    // Start is called before the first frame update
    void Start()
    {
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

    }

    public void Easy() 
    {
        startScreen.SetActive(false);
        maxGear = 1;
        spawnManager.StartSpawn();
    }

    public void Hard() 
    {
        startScreen.SetActive(false);
        maxGear = 10;
        spawnManager.StartSpawn();
    }   

    // Update is called once per frame
    void Update()
    {

    }

    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
