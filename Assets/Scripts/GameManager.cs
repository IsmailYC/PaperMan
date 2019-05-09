using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public enum States {Menu, Play, Pause, Over};
    public GameObject menuPanel;
    public GameObject pausePanel;
    public GameObject overPanel;
    public States state = States.Menu;
    public PlayerController player;
    public SpawnSpot boostSpawn;

    void Awake()
    {
        Time.timeScale = 0.0f;
    }
	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
#if UNITY_STANDALONE || UNITY_WEBGL
        Camera.main.orthographicSize = 10f * 1.777251f / Camera.main.aspect;
#else
        Camera.main.aspect= 16f/9f;
#endif
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            switch(state)
            {
                case States.Menu:
                    Application.Quit();
                    break;
                case States.Play:
                    PauseGame();
                    break;
                case States.Pause:
                    ResumeGame();
                    break;
                case States.Over:
                    RestartGame();
                    break;
            }
        }
    }

    public void CloseGame()
    {
        player.SaveScore();
        Application.Quit();
    }

    public void PlayGame()
    {
        state = States.Play;
        menuPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        state = States.Pause;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        state = States.Play;
        Time.timeScale = 1.0f;
    }

    public void EndGame()
    {
        overPanel.SetActive(true);
        state = States.Over;
        Time.timeScale = 0.0f;
    }

    public void RestartGame()
    {
        player.Reset();
        overPanel.SetActive(false);
        state = States.Play;
        Time.timeScale = 1.0f;
    }

    public void SpawnBoost()
    {
        boostSpawn.TriggerSpawn();
    }
}
