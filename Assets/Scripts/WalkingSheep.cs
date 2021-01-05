using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSheep : MonoBehaviour
{

    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform despawnPoint;
    [SerializeField] float speed = 4;
    [SerializeField] Animator anim;

    private void Awake()
    {
        speed = Random.Range(speed - 0.1f, speed+0.1f);
        Invoke("StartWalking", Random.Range(0f, 2f));
    }

    void Update()
    {

        Vector3 target = new Vector3(despawnPoint.localPosition.x - 10, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        if (transform.localPosition.x <= despawnPoint.localPosition.x)
        {
            transform.localPosition = new Vector3(spawnPoint.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            speed = Random.Range(speed - 0.1f, speed + 0.1f);
        }
    }

    void StartWalking()
    {
        anim.SetBool("iddle", false);
        anim.SetBool("walking", true);
    }
}
