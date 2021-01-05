using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{

    [SerializeField] Animator anim;
    [SerializeField] float delay;

    void Start()
    {
        Invoke("StartAnimation", delay);
    }

    void StartAnimation()
    {
        anim.enabled = true;
    }
}
