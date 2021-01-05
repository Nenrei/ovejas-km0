using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using JackSParrot.Services.Localization;
using JackSParrot.Utils;

public class GameplayManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] string levelName;

    [Space(20)]
    [SerializeField] GameObject timerPanel;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject sheepsPanel;
    [SerializeField] TextMeshProUGUI sheepText;
    [SerializeField] MenuPlayerMovement player;

    [Space(20)]
    [SerializeField] List<Levels> levels;

    [Space(20)]
    [SerializeField] GameObject resultPanel;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI resultSheepsText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] GameObject endPanel;
    [SerializeField] TwitterButton tweetButton;

    [Space(20)]
    [SerializeField] GameObject tutorialAndNarrativePanel;
    [SerializeField] TextMeshProUGUI tutorialAndNarrativeText;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI tutorialText;


    [Space(20)]
    [SerializeField] GameObject pauseCanvas;


    [Space(20)]
    [SerializeField] DeliverZone deliveringZone;

    bool tutorialAndNarrative;

    bool timerIsRunning;
    int currentLevel;

    float currentLevelTime;
    int currentLevelSheeps;

    bool canPause;
    bool isLastLevel;
    bool tutorialIsUp;

    ILocalizationService service;


    // Start is called before the first frame update
    void Awake()
    {
        timerPanel.SetActive(false);
        sheepsPanel.SetActive(false);

        tutorialAndNarrative = true;

        StartCoroutine(InitializeLocalizationService());

        currentLevelSheeps = levels[currentLevel].Sheeps;
        sheepText.text = currentLevelSheeps.ToString();
        GetComponent<SpawnSheeps>().StartSpawnSheeps(currentLevelSheeps);
        currentLevelTime = levels[currentLevel].Time;
        StartCoroutine(ShowTutorialAndNarrative());

        pauseCanvas.SetActive(false);
    }

    IEnumerator ShowTutorialAndNarrative()
    {
        PauseGame();
        tutorialIsUp = true;
        yield return new WaitForSecondsRealtime(0.5f);
        tutorialAndNarrativePanel.GetComponent<Animator>().SetTrigger("show");
        ShowMission(tutorialAndNarrativeText);
    }
    IEnumerator ShowTutorial()
    {
        PauseGame();
        tutorialIsUp = true;
        canPause = false;
        yield return new WaitForSecondsRealtime(0.5f);
        tutorialPanel.GetComponent<Animator>().SetTrigger("show");
        ShowMission(tutorialText);
    }
    void Start()
    {
        SoundManager.instance.PlayMusic("level");
        if (tutorialAndNarrative)
		{
            SoundManager.instance.PlayEffect("narracion");
		}
    }

    void ShowMission(TextMeshProUGUI _tutorialText)
    {
        _tutorialText.text = "<b><line-height=150%>" + service.GetLocalizedString("UI_LEVEL") + " " + (currentLevel + 1) + ":</b> \r\n" + service.GetLocalizedString("UI_MISSION");
        float minutes = Mathf.FloorToInt(levels[currentLevel].Time / 60);
        float seconds = Mathf.FloorToInt(levels[currentLevel].Time % 60);
        _tutorialText.text = _tutorialText.text.Replace("{0}", string.Format("{0:00}:{1:00}", minutes, seconds));
        _tutorialText.text = _tutorialText.text.Replace("{1}", currentLevelSheeps.ToString());
    }

    public void Play()
    {
        SoundManager.instance.SetSheepsVolume(PlayerPrefs.GetFloat("effectsVolume"));

        tutorialIsUp = false;
        if (tutorialAndNarrative)
        {
            tutorialAndNarrativePanel.GetComponent<Animator>().SetTrigger("hide");
            tutorialAndNarrative = false;
        }
        else
            tutorialPanel.GetComponent<Animator>().SetTrigger("hide");


        timerPanel.SetActive(true);
        sheepsPanel.SetActive(true);

        //currentLevelSheeps = levels[currentLevel].Sheeps;
        currentLevelTime = levels[currentLevel].Time + 1;
        timerIsRunning = true;

        canPause = true;

        ResumeGame();

        isLastLevel = currentLevel == levels.Count - 1;

    }

   
    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (currentLevelTime > 1)
            {
                currentLevelTime -= Time.deltaTime;
                DisplayTime(currentLevelTime);
            }
            else
            {
                currentLevelTime = 0;
                timerIsRunning = false;
                StartCoroutine(YouLose());
                SoundManager.instance.PlayEffect("derrota");
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("XboxStart"))
            {
                if (Time.timeScale == 1)
                    RealPause();
                else
                    RealResume();
            }

        }
        else
        {
            if (Input.GetButtonDown("XboxA") || Input.GetKeyDown(KeyCode.Return))
            {
                if (tutorialIsUp)
                {
                    Play();
                }
                else
                {
                    switch (resultPanel.GetComponent<UIHighlightGroup>().SelectedItem)
                    {
                        case 0:
                            GoToMenu();
                            break;
                        case 1:
                            RestartLevel();
                            break;
                        case 2:
                            NextLevel();
                            break;
                    }
                }
            }
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateSheeps()
    {
        currentLevelSheeps--;
        sheepText.text = currentLevelSheeps.ToString();

        if (currentLevelSheeps == 0)
		{
            StartCoroutine(YouWin());
            SoundManager.instance.PlayEffect("victoria");
        }
            
    }

    IEnumerator YouLose()
    {
        yield return new WaitForSecondsRealtime(1f);
        timerIsRunning = false;
        resultText.text = service.GetLocalizedString("UI_LABEL_LOSE");

        if (currentLevelSheeps > 1)
        {
            resultSheepsText.text = service.GetLocalizedString("UI_LABEL_LOSE_DESC");
            resultSheepsText.text = resultSheepsText.text.Replace("{0}", currentLevelSheeps.ToString());
        }
        else
            resultSheepsText.text = service.GetLocalizedString("UI_LABEL_LOSE_DESC_SING");


        bestTimeText.gameObject.SetActive(true);
        if (PlayerPrefs.GetFloat("bestTimeLevel_" + currentLevel) > 0)
            bestTimeText.text = service.GetLocalizedString("UI_LABEL_BEST_TIME") + ": " + PlayerPrefs.GetString("bestTimeTexLevel_" + currentLevel);
        else
            bestTimeText.gameObject.SetActive(false);

        nextLevelButton.SetActive(false);

        tweetButton.gameObject.SetActive(false);

        resultPanel.GetComponent<UIHighlightGroup>().SelectedItem = -1;
        resultPanel.GetComponent<UIHighlightGroup>().IsVisible = true;

        resultPanel.GetComponent<Animator>().SetTrigger("show");
        Invoke("PauseGame", 0.2f);

    }

    IEnumerator YouWin()
    {
        yield return new WaitForSecondsRealtime(1f);

        timerIsRunning = false;
        float wintime = levels[currentLevel].Time - currentLevelTime;
        float minutes = Mathf.FloorToInt(wintime / 60);
        float seconds = Mathf.FloorToInt(wintime % 60);

        resultText.text = service.GetLocalizedString("UI_LABEL_WIN");

        resultSheepsText.text = service.GetLocalizedString("UI_LABEL_WIN_DESC");
        if (minutes == 0)
        {
            resultSheepsText.text = resultSheepsText.text + service.GetLocalizedString("UI_LABEL_WIN_DESC_TIME_S");
            resultSheepsText.text = resultSheepsText.text.Replace("{0}", seconds.ToString());
        }
        else
        {
            resultSheepsText.text = resultSheepsText.text + service.GetLocalizedString("UI_LABEL_WIN_DESC_TIME_M");
            resultSheepsText.text = resultSheepsText.text.Replace("{0}", minutes.ToString());
            resultSheepsText.text = resultSheepsText.text.Replace("{1}", seconds.ToString());
        }

        nextLevelButton.SetActive(true);

        resultPanel.GetComponent<UIHighlightGroup>().SelectedItem = -1;
        resultPanel.GetComponent<UIHighlightGroup>().IsVisible = true;

        if (!PlayerPrefs.HasKey("bestTimeLevel_" + currentLevel) || wintime < PlayerPrefs.GetFloat("bestTimeLevel_" + currentLevel)) {
            PlayerPrefs.SetFloat("bestTimeLevel_" + currentLevel, wintime);
            PlayerPrefs.SetString("bestTimeTexLevel_" + currentLevel, string.Format("{0:00}:{1:00}", minutes, seconds));
        }

        bestTimeText.text = service.GetLocalizedString("UI_LABEL_BEST_TIME") + ": " + PlayerPrefs.GetString("bestTimeTexLevel_" + currentLevel);

        if (isLastLevel)
        {
            nextLevelButton.SetActive(false);
            endPanel.SetActive(true);
        }

        tweetButton.gameObject.SetActive(true);
        string tweetText = service.GetLocalizedString("UI_TWEET") + "\n" + "\n" + service.GetLocalizedString("UI_TWEET_URL");
        tweetText = tweetText.Replace("{0}", string.Format("{0:00}:{1:00}", minutes, seconds));
        tweetText = tweetText.Replace("{1}", (currentLevel + 1).ToString());
        tweetButton.TweetText = tweetText + ".";

        resultPanel.GetComponent<Animator>().SetTrigger("show");
        Invoke("PauseGame", 0.2f);
    }




    void RealPause()
    {
        if (Time.timeScale == 1)
        {
            pauseCanvas.SetActive(true);
            pauseCanvas.GetComponentInChildren<UIHighlightGroup>().IsVisible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }
    public void RealResume()
    {
        pauseCanvas.GetComponentInChildren<UIHighlightGroup>().IsVisible = false;
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }


    public void RestartLevel()
    {
        pauseCanvas.GetComponentInChildren<UIHighlightGroup>().IsVisible = false;
        pauseCanvas.SetActive(false);

        ResumeGame();

        timerPanel.SetActive(false);
        sheepsPanel.SetActive(false);

        player.RestartPlayer();
        GetComponent<SpawnSheeps>().RestartSheeps();
        deliveringZone.RestartDeliverPoints();

        timerIsRunning = false;
        currentLevelSheeps = levels[currentLevel].Sheeps;
        sheepText.text = currentLevelSheeps.ToString();
        GetComponent<SpawnSheeps>().StartSpawnSheeps(currentLevelSheeps);

        resultPanel.GetComponent<UIHighlightGroup>().IsVisible = false;
        resultPanel.GetComponent<Animator>().SetTrigger("hide");

        StartCoroutine(ShowTutorial());
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        currentLevel++;
        RestartLevel();
    }

    IEnumerator InitializeLocalizationService()
    {
        float timer = 0;
        service = SharedServices.GetService<ILocalizationService>();
        if (service == null)
        {
            service = new LocalLocalizationService();
            service.Initialize(() => Debug.Log("LocalizationService Initialized"));
            SharedServices.RegisterService<ILocalizationService>(service);
        }
        while (!service.Initialized || timer < 5)
        {
            yield return new WaitForSeconds(1.0f);
            timer++;
        }
    }
}
