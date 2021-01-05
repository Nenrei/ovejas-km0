using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    UIHighlightGroup highlightGroup;

    private void Start()
    {
        highlightGroup = GetComponent<UIHighlightGroup>();
    }

    void Update()
    {
        if (highlightGroup.IsVisible)
        {
            if (Input.GetButtonDown("XboxA") || Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
    }
    public void Back()
    {
        if (menuManager != null)
        {
            menuManager.Back();
        }
    }
}
