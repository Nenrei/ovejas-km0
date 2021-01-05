using UnityEngine;
[System.Serializable]

public class Resolutions
{
    [SerializeField] int id;
    [SerializeField] int width;
    [SerializeField] int height;
    string description;

    public Resolutions(int id, int width, int height)
    {
        this.id = id;
        this.width = width;
        this.height = height;
    }

    public int Id { get => id; set => id = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public string Description { get => width + " x " + height; set => description = value; }
}
