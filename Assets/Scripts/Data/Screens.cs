using UnityEngine;
[System.Serializable]

public class Screens
{
    [SerializeField] int id;
    [SerializeField] string desc;
    [SerializeField] FullScreenMode screenMode;

    public int Id { get => id; set => id = value; }
    public string Desc { get => desc; set => desc = value; }
    public FullScreenMode ScreenMode { get => screenMode; set => screenMode = value; }
}
