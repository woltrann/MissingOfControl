using System.Collections;
using TMPro;
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
    public GameObject startDialog;
    public TextMeshProUGUI startText;
    public GameObject finishDialog;
    public TextMeshProUGUI finishText;
    public bool isPaused = false;
    public bool isFinished = false;

    [TextArea]
    public float typingSpeed = 0.05f; // Harfler arasý süre
    public AudioSource typingSFX;

    public bool hasE = false;
    public bool hasA = false;
    public bool hasS = false;
    public bool hasD = false;
    public bool hasEnter = false;

    public Animator gateAnimator;
    public Animator keyboardAnimator;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 1f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest býrak
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
        StartCoroutine(ShowStartTextCoroutine("Görünüþe bakýlýrsa kontrolünü kaybetmiþsin... Geri kazanman lazým..."));
        mainMenu.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused= false;
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false;

    }

    IEnumerator ShowStartTextCoroutine(string message)
    {
        startDialog.SetActive(true);
        startText.text = "";

        // Harf harf yazý efekti
        foreach (char c in message)
        {
            startText.text += c;

            if (typingSFX != null)
                typingSFX.PlayOneShot(typingSFX.clip);

            yield return new WaitForSeconds(0.06f);
        }

        // Yazý bittikten sonra 2 saniye bekle ve paneli kapat
        yield return new WaitForSeconds(2f);
        startDialog.SetActive(false);
        if (isFinished)
        {
            finishMenu.SetActive(true);
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest býrak
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
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest býrak
        Cursor.visible = true;
        //Time.timeScale = 0f;
    }
    public void Finish()
    {
        gateAnimator.SetTrigger("finish");
    }
    public void End()
    {
        isFinished = true;
        keyboardAnimator.SetTrigger("finish");
        StartCoroutine(ShowStartTextCoroutine("Tebrik ederim yumruk atýp da fýrlayan parçalarýmý bulup geri getirmiþsin. "));
        gameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest býrak
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
