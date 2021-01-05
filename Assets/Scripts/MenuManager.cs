using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public bool inGame;
    [SerializeField] GameplayManager manager;

    [Space(20)]
    [SerializeField] GameObject buttonsPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;

    UIHighlightGroup highlightGroup;

    public UIHighlightGroup HighlightGroup { get => highlightGroup; set => highlightGroup = value; }

    private void Start()
    {
        HighlightGroup = GetComponent<UIHighlightGroup>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        HighlightGroup.SelectedItem = -1;
        //highlightGroup.IsVisible = true;

        if (!inGame)
		{
            SoundManager.instance.PlayMusic("menu");
        }
    }

    private void Update()
    {
        if (HighlightGroup.IsVisible && HighlightGroup.SelectedItem > -1)
        {
            if (Input.GetButtonDown("XboxA") || Input.GetKeyDown(KeyCode.Return))
            {
                HighlightGroup.highlightItems[HighlightGroup.SelectedItem].GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void Play()
    {
        if (inGame)
            manager.RealResume();
        else
            SceneManager.LoadScene("Level_00");
    }

    public void Exit()
    {
        if (inGame)
        {
            Time.timeScale = 1;
            SoundManager.instance.StopEffect();
            SceneManager.LoadScene("Menu");
        }else
            Application.Quit();
    }

    public void Credits()
    {
        HighlightGroup.IsVisible = false;
        creditsPanel.GetComponent<Animator>().SetBool("showing", true);
        StartCoroutine(SetCreditsVisible());
    }
    public void Back()
    {
        creditsPanel.GetComponent<Animator>().SetBool("showing", false);
        creditsPanel.GetComponentInChildren<UIHighlightGroup>().IsVisible = false;
        StartCoroutine(SetVisible());
    }

    public void Settings()
    {
        HighlightGroup.IsVisible = false;
        settingsPanel.GetComponent<Animator>().SetBool("showing", true);
        settingsPanel.GetComponentInChildren<UIHighlightGroup>().IsVisible = true;
        settingsPanel.GetComponentInChildren<UIHighlightGroup>().SelectedItem = -1;
    }

    public void Save()
    {
        settingsPanel.GetComponent<Animator>().SetBool("showing", false);
        settingsPanel.GetComponentInChildren<UIHighlightGroup>().IsVisible = false;
        StartCoroutine(SetVisible());
    }

    IEnumerator SetVisible()
    {
        yield return new WaitForSecondsRealtime(0.65f);
        HighlightGroup.IsVisible = true;
    }

    IEnumerator SetCreditsVisible()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        creditsPanel.GetComponentInChildren<UIHighlightGroup>().IsVisible = true;
    }

}
