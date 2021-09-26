using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int score = 0;
    TextMesh counter;

    public static bool playerDead;

    public delegate void PlayerDeath();
    public static event PlayerDeath WhenDie;

    public delegate void EnemyKilled();
    public static event EnemyKilled EnemyKill;

    public GameObject deathText;

   
    void Start()
    {
        counter = transform.Find("ScoreCounter").GetComponent<TextMesh>();
    }

    void Update()
    {
      
        // Game reset and exit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene("Scene");
        }


        // Player death event
        if (playerDead && WhenDie != null)
        {
            int highScore =  PlayerPrefs.GetInt("highscore", 0);
            if (score > highScore){
                deathText.transform.Find("High Score").GetComponent<TextMesh>().text = "NEW HIGH SCORE!";
                PlayerPrefs.SetInt("highscore", score);
            }
            else {
                deathText.transform.Find("High Score Counter").GetComponent<TextMesh>().text = highScore.ToString();
            }
            deathText.SetActive(true);
            playerDead = false;
            WhenDie();
        }

    }

    public void OnEnemyKilled()
    {
        score++;
        counter.text = score.ToString();
        EnemyKill();
    }

}
