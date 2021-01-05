using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighlightGroup : MonoBehaviour
{

    [SerializeField] bool horizontalGroup;
    public List<HighlightSettingOnSelect> highlightItems;

    int selectedItem;
    bool isVisible;
    bool pulsedButton;

    public int SelectedItem { get => selectedItem; set => selectedItem = value; }
    public bool IsVisible { get => isVisible; set => isVisible = value; }
    public bool PulsedButton { get => pulsedButton; set => pulsedButton = value; }

    // Start is called before the first frame update
    void Awake()
    {
        selectedItem = -1;
        foreach (HighlightSettingOnSelect item in highlightItems)
        {
            item.HighlightGroup = this;
        }
        if (highlightItems.Count == 1) {
            SelectedItem = 0;
            highlightItems[SelectedItem].Highlight();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (IsVisible)
        {
            if (SelectedItem > -1 && ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0)) && !Cursor.visible)
            {
                highlightItems[SelectedItem].NoHighlight();
                Cursor.visible = true;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal == 0 && vertical == 0)
            {
                horizontal = Input.GetAxisRaw("XboxH");
                vertical = Input.GetAxisRaw("XboxV");
            }

            if (PulsedButton && horizontal == 0 && vertical == 0)
            {
                PulsedButton = false;
            }
            else if (!PulsedButton)
            {
                float valueToCheck = horizontalGroup ? horizontal*-1 : vertical;
                if (valueToCheck != 0)
                {
                    Cursor.visible = false;
                    PulsedButton = true;

                    if (SelectedItem > -1)
                        highlightItems[SelectedItem].NoHighlight();

                    if (valueToCheck < 0)
                    {
                        if (SelectedItem >= highlightItems.Count - 1)
                            SelectedItem = 0;
                        else
                            SelectedItem++;
                    }
                    else if (valueToCheck > 0)
                    {
                        if (SelectedItem <= 0)
                            SelectedItem = highlightItems.Count - 1;
                        else
                            SelectedItem--;

                    }
                    highlightItems[SelectedItem].Highlight();

                }
            }
        }
    }

    public void UpdateSelectedItem(HighlightSettingOnSelect item)
    {
        int index = highlightItems.IndexOf(item);

        if (SelectedItem == index || index <= -1 )
        {
            if(highlightItems.Count > 1 && index >= highlightItems.Count - 1)
                return;
        }
        if (SelectedItem > -1 && SelectedItem < highlightItems.Count)
            highlightItems[SelectedItem].NoHighlight();

        SelectedItem = index;
        highlightItems[SelectedItem].Highlight();
    }
}
