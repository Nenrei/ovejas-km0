using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using JackSParrot.Services.Localization;
using JackSParrot.Utils;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;


    [Space(20)]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] TextMeshProUGUI languageText;
    [SerializeField] TextMeshProUGUI resolutionText;
    [SerializeField] TextMeshProUGUI screenModeText;
    [SerializeField] Button changeLangButton;


    [Space(20)]
    [SerializeField] List<Language> languages;
    [SerializeField] List<Resolutions> resolutions;
    [SerializeField] List<Screens> screenModes;

    UIHighlightGroup highlightGroup;

    float musicVolume;
    float effectsVolume;

    int language;
    int resolution;
    int screenMode;


    ILocalizationService service;
    bool musicVolumeSaved;

    

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 0.3f);

        if (!PlayerPrefs.HasKey("effectsVolume"))
            PlayerPrefs.SetFloat("effectsVolume", 0.3f);

        if (!PlayerPrefs.HasKey("language"))
            PlayerPrefs.SetInt("language", 0);

        if (!PlayerPrefs.HasKey("resolution"))
            PlayerPrefs.SetInt("resolution", 0);

        if (!PlayerPrefs.HasKey("screenMode"))
            PlayerPrefs.SetInt("screenMode", 0);

        if (resolutions.Count == 0)
            GetResolutions();

        highlightGroup = GetComponent<UIHighlightGroup>();
        StartCoroutine(InitializeLocalizationService());
        RestartUI();
    }

    private void Update()
    {
        if (highlightGroup.IsVisible)
        {
            if (Input.GetButtonDown("XboxA") || Input.GetKeyDown(KeyCode.Return))
            {
                if (highlightGroup.SelectedItem == 5)
                    Save();
                if (highlightGroup.SelectedItem == 6)
                    Back();
            }
            if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.Escape)){
                Back();
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if(horizontal  == 0) horizontal = Input.GetAxisRaw("XboxH");
                
            if (!highlightGroup.PulsedButton && horizontal != 0)
            {
                highlightGroup.PulsedButton = true;
                bool isNext = horizontal > 0;
                switch (highlightGroup.SelectedItem)
                {
                    case 1:
                        if (isNext) IncreaseMusicVolume();
                        else DecreaseMusicVolume();
                        break;
                    case 2:
                        if (isNext) IncreaseEffectsVolume();
                        else DecreaseEffectsVolume();
                        break;
                    case 0:
                        ShowNextPrevLanguage(isNext);
                        break;
                    case 3:
                        ShowNextPrevResolutions(isNext);
                        break;
                    case 4:
                        ShowNextPrevScreenMode(isNext);
                        break;
                    case 5:
                        highlightGroup.highlightItems[highlightGroup.SelectedItem].NoHighlight();
                        highlightGroup.SelectedItem = 6;
                        highlightGroup.highlightItems[highlightGroup.SelectedItem].Highlight();
                        break;
                    case 6:
                        highlightGroup.highlightItems[highlightGroup.SelectedItem].NoHighlight();
                        highlightGroup.SelectedItem = 5;
                        highlightGroup.highlightItems[highlightGroup.SelectedItem].Highlight();
                        break;
                }
            }
            
        }
    }



    #region

    public void PrevItemOfList(string list)
    {
        switch (list)
        {
            case "language":
                ShowNextPrevLanguage(false);
                break;
            case "resolution":
                ShowNextPrevResolutions(false);
                break;
            case "screenMode":
                ShowNextPrevScreenMode(false);
                break;
        }
    }

    public void NextItemOfList(string list)
    {
        switch (list)
        {
            case "language":
                ShowNextPrevLanguage(true);
                break;
            case "resolution":
                ShowNextPrevResolutions(true);
                break;
            case "screenMode":
                ShowNextPrevScreenMode(true);
                break;
        }
    }

    void ShowNextPrevLanguage(bool isNext)
    {
        if (isNext)
        {
            if (language >= languages.Count - 1)
                language = 0;
            else
                language++;
        }
        else
        {
            if (language <= 0)
                language = languages.Count - 1;
            else
                language--;
        }

        PlayerPrefs.SetInt("language", languages[language].Id);
        PlayerPrefs.SetString("languageMin", languages[language].Lang);
        changeLangButton.onClick.Invoke();

        UpdateDynamicTexts();

    }
    void ShowNextPrevScreenMode(bool isNext)
    {
        if (isNext)
        {
            if (screenMode >= screenModes.Count - 1)
                screenMode = 0;
            else
                screenMode++;
        }
        else
        {
            if (screenMode <= 0)
                screenMode = screenModes.Count - 1;
            else
                screenMode--;
        }
        UpdateDynamicTexts();
        SetScreenConf();
    }
    void ShowNextPrevResolutions(bool isNext)
    {
        if (isNext)
        {
            if (resolution >= resolutions.Count - 1)
                resolution = 0;
            else
                resolution++;
        }
        else
        {
            if (resolution <= 0)
                resolution = resolutions.Count - 1;
            else
                resolution--;
        }
        resolutionText.text = resolutions[resolution].Description;
        SetScreenConf();
    }



    #endregion


    #region Sound Management

    public void SetMusicVolume(float sliderValue)
    {
        if (musicVolumeSaved)
            musicVolume = sliderValue;
        SoundManager.instance.SetMusicVolume(musicVolume);
    }

    public void SetSfxVolume(float sliderValue)
    {
        if (musicVolumeSaved)
            effectsVolume = sliderValue;

        SoundManager.instance.SetEffectsVolume(effectsVolume);
        if (menuManager.inGame)
        {
            SoundManager.instance.SetSheepsVolume(effectsVolume);
        }
        else
        {
            SoundManager.instance.SetSheepsPackVolume(effectsVolume);
        }
    }

    public void IncreaseMusicVolume()
    {
        if (musicVolume == 1) return;
        musicVolume += 0.1f;
        if (musicVolume > 1)
            musicVolume = 1;
        musicSlider.value = musicVolume;

        SetMusicVolume(musicVolume);
    }
    public void DecreaseMusicVolume()
    {
        if (musicVolume == 0) return;
        musicVolume -= 0.1f;
        if (musicVolume < 0)
            musicVolume = 0;
        musicSlider.value = musicVolume;

        SetMusicVolume(musicVolume);
    }

    public void IncreaseEffectsVolume()
    {
        if (effectsVolume == 1) return;
        effectsVolume += 0.1f;
        if (effectsVolume > 1)
            effectsVolume = 1;
        effectsSlider.value = effectsVolume;

        SetSfxVolume(effectsVolume);
    }
    public void DecreaseEffectsVolume()
    {
        if (effectsVolume == 0) return;
        effectsVolume -= 0.1f;
        if (effectsVolume < 0)
            effectsVolume = 0;
        effectsSlider.value = effectsVolume;

        SetSfxVolume(effectsVolume);
    }

    void SetScreenConf()
    {
        bool fullsCreen = screenModes[screenMode].ScreenMode == FullScreenMode.FullScreenWindow;
        Screen.SetResolution(resolutions[resolution].Width, resolutions[resolution].Height, fullsCreen);
    }

    #endregion



    public void Save()
    {
        //Save Sound
        SoundManager.instance.SetMusicVolume(musicVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);

        SoundManager.instance.SetEffectsVolume(effectsVolume);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);

        if (menuManager.inGame)
        {
            SoundManager.instance.SetSheepsVolume(effectsVolume);
        }
        else
        {
            SoundManager.instance.SetSheepsPackVolume(effectsVolume);
        }


        PlayerPrefs.SetInt("resolution", resolution);
        PlayerPrefs.SetInt("screenMode", screenMode);

        SetScreenConf();

        Back();
    }

    public void Back()
    {
        if (menuManager != null)
        {
            menuManager.Save();
        }

        RestartUI();
    }

    void RestartUI()
    {
        if(highlightGroup.SelectedItem > -1)
            highlightGroup.highlightItems[highlightGroup.SelectedItem].NoHighlight();
        highlightGroup.SelectedItem = -1;

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume");
        musicVolumeSaved = true;
        SetMusicVolume(musicSlider.value);
        SetSfxVolume(effectsSlider.value);

        language = PlayerPrefs.GetInt("language");
        languageText.text = languages[language].LanguageDesc;

        screenMode = PlayerPrefs.GetInt("screenMode");
        screenModeText.text = screenModes[screenMode].Desc;

        resolution = PlayerPrefs.GetInt("resolution");
        resolutionText.text = resolutions[resolution].Description; 
        SetScreenConf();

        UpdateDynamicTexts();

    }



    string UpdateText(string _key)
    {
        return service.GetLocalizedString(_key);
    }

    void UpdateDynamicTexts()
    {
        languageText.text = UpdateText(languages[language].LanguageDesc);
        screenModeText.text = UpdateText(screenModes[screenMode].Desc);
    }

    void GetResolutions()
    {
        Resolution[] _resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            resolutions.Add(new Resolutions(i, _resolutions[i].width, _resolutions[i].height));
            if (!PlayerPrefs.HasKey("resolution") && _resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                PlayerPrefs.SetInt("resolution", i);
            }
        }
    }

    IEnumerator InitializeLocalizationService()
    {
        service = SharedServices.GetService<ILocalizationService>();
        if (service == null)
        {
            service = new LocalLocalizationService();
            service.Initialize(() => Debug.Log("LocalizationService Initialized"));
            SharedServices.RegisterService<ILocalizationService>(service);
        }
        while (!service.Initialized)
        {
            yield return new WaitForSeconds(1.0f);
        }
    }


}
