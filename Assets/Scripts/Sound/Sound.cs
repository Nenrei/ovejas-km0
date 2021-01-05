using UnityEngine;

[System.Serializable]
public class Sound{
    [Tooltip("Name of the sound. Each name has to be different between each other.")]
    public string name;

    public AudioClip clip;
}

