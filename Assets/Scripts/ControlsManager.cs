using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JackSParrot.Services.Localization;
using JackSParrot.Utils;
using TMPro;

public class ControlsManager : MonoBehaviour
{
    [SerializeField] UIHighlightGroup menuManager;
    [SerializeField] GameObject loadingBar;
    [SerializeField] GameObject loadingBarBack;
    [SerializeField] GameObject button;
    [SerializeField] TextMeshProUGUI loadingText;

    ILocalizationService service;

    bool canHide;

    // Start is called before the first frame update
    void Awake()
    {
        menuManager.IsVisible = false;
        StartCoroutine(InitializeLocalizationService());
        StartCoroutine(LoadBar());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("XboxA") || Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(HideControls());
        }
    }

    IEnumerator LoadBar()
    {
        loadingText.text = service.GetLocalizedString("UI_LOADING_SENTENCE_" + Random.Range(0, 5));
        yield return new WaitForSecondsRealtime(3.5f);
        loadingText.gameObject.SetActive(false);
        loadingBar.SetActive(false);
        loadingBarBack.SetActive(false);
        button.SetActive(true);
        canHide = true;
    }

    IEnumerator HideControls()
    {
        if (canHide)
        {
            canHide = false;
            GetComponent<Animator>().SetTrigger("hide");
            yield return new WaitForSecondsRealtime(1.2f);
            menuManager.IsVisible = true;
            gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        StartCoroutine(HideControls());
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
