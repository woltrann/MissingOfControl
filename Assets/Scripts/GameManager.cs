using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameUI;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject finishMenu;
    public bool isPaused = false;
    public bool isFinished = false;

    public bool hasE = false;
    public bool hasA = false;
    public bool hasS = false;
    public bool hasD = false;
    public bool hasEnter = false;

    public AudioSource congrats;
    public Animator gateAnimator;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 1f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest b�rak
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenu.activeSelf && !finishMenu.activeSelf && !gameOverMenu.activeSelf)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused= false;
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false;

    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest b�rak
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false;
    }
    public void GameOver()
    {
        gameUI.SetActive(false);
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest b�rak
        Cursor.visible = true;
        //Time.timeScale = 0f;
    }
    public void Finish()
    {
        gateAnimator.SetTrigger("finish");
    }
    public void End()
    {
        if (congrats != null && congrats.clip != null)
        {
            congrats.Play();
        }
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest b�rak
        Cursor.visible = true;
    }
    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
