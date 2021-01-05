using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAboveObject : MonoBehaviour
{
    public Transform target;
    [SerializeField] GameObject cursorImage;
    [SerializeField] GameObject xboxImage;
    

    void Update()
    {

        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
            if(GetComponent<CanvasGroup>().alpha == 0)
                GetComponent<CanvasGroup>().alpha = 1;
        }
    }

    public void HideText()
    {
        target = null;
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);

        cursorImage.SetActive(false);
        xboxImage.SetActive(false);
    }

    public void ShowText(Transform _target, bool keyboardAndMouse)
    {

        if (keyboardAndMouse)
            cursorImage.SetActive(true);
        else
            xboxImage.SetActive(true);

        target = _target;
        gameObject.SetActive(true);
    }
}
