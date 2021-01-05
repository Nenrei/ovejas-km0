using UnityEngine;
[System.Serializable]
public class Language
{
    [SerializeField] int id;
    [SerializeField] string languageDesc;
    [SerializeField] string lang;

    public int Id { get => id; set => id = value; }
    public string LanguageDesc { get => languageDesc; set => languageDesc = value; }
    public string Lang { get => lang; set => lang = value; }
}
