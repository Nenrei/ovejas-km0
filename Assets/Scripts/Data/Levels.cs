using UnityEngine;
[System.Serializable]

public class Levels
{
    [SerializeField] float time;
    [SerializeField] int sheeps;

    public float Time { get => time; set => time = value; }
    public int Sheeps { get => sheeps; set => sheeps = value; }
}
