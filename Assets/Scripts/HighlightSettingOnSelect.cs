using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightSettingOnSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool isMenuItem;
    [SerializeField] Animator menuItem;

    [SerializeField] Color highlightColor;
    [SerializeField] Color normalColor;
    [SerializeField] Image leftArrow;
    [SerializeField] Image rightArrow;
    [SerializeField] Image frame;
    [SerializeField] Image slider;
    [SerializeField] TextMeshProUGUI text;

    UIHighlightGroup highlightGroup;

    public UIHighlightGroup HighlightGroup { get => highlightGroup; set => highlightGroup = value; }

    public void Highlight()
    {
        if (isMenuItem)
        {
            menuItem.SetBool("highlited", true);
        }
        else
        {
            if (leftArrow != null)
                leftArrow.color = highlightColor;
            if (rightArrow != null)
                rightArrow.color = highlightColor;
            if (slider != null)
                slider.color = highlightColor;
            if (frame != null)
                frame.color = highlightColor;
            if (text != null)
                text.color = highlightColor;
        }
    }

    public void NoHighlight()
    {
        if (isMenuItem)
        {
            menuItem.SetBool("highlited", false);
        }
        else
        {
            if (leftArrow != null)
                leftArrow.color = normalColor;
            if (leftArrow != null)
                rightArrow.color = normalColor;
            if (slider != null)
                slider.color = normalColor;
            if (frame != null)
                frame.color = normalColor;
            if (text != null)
                text.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
        if(HighlightGroup != null)
            HighlightGroup.UpdateSelectedItem(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NoHighlight();
    }


}
