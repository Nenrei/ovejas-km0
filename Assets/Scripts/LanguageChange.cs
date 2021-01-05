using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JackSParrot.Utils;

namespace JackSParrot.Services.Localization
{
    public class LanguageChange : MonoBehaviour
    {
        public void ChangeLanguage()
        {
            var service = SharedServices.GetService<ILocalizationService>();

            service.UpdateLanguage();

            foreach (LocalizedText text in GameObject.FindObjectsOfType<LocalizedText>())
            {
                text._tmpText.text = service.GetLocalizedString(text._key);
            }
        }

    }
}
