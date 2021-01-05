using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Sheep : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Space(20)]
    [SerializeField] float sheepDistanceRun = 7.0f;
    [SerializeField] float catchDistance = 1.0f;
    [SerializeField] float followSpeed = 4f;
    [SerializeField] float catchedTriggerRadious = 1.5f;
    [SerializeField] float[] runSpeed;

    [Space(20)]
    [SerializeField] UiAboveObject catchSheepText;
    [SerializeField] Transform uiTarget;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Animator anim;
    [SerializeField] Sound[] effects = default;
    Transform deliverPoint;

    NavMeshAgent agent;
    //Outline outline;

    bool runFromPlayer;
    bool followPlayer;
    bool canBeCatched;
    bool catched;
    bool delivered;
    bool move;

    float distance;

    bool keyboardAndMouse;

    [SerializeField, Range(0, 1f)] float stepsInterval = .5f;
    float currentTime;

    private AudioSource sonido;

    void Start()
    {
        move = true;
        agent = GetComponent<NavMeshAgent>();
        //outline = GetComponent<Outline>();
        //outline.enabled = false;
        anim.enabled = false;

        sonido = GetComponent<AudioSource>();

        Invoke("Cencerro", UnityEngine.Random.Range(5f, 10f));
        Invoke("Balido", UnityEngine.Random.Range(5f, 10f));
        
        
    }

    private void OnEnable()
    {
        move = true;
        agent = GetComponent<NavMeshAgent>();
        //outline = GetComponent<Outline>();
        //outline.enabled = false;

        runFromPlayer = false;
        followPlayer = false;
        canBeCatched = false;
        catched = false;
        delivered = false;
        move = true;
        agent.ResetPath();
        particles.gameObject.SetActive(false);
        anim.enabled = false;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().radius = 5;

        Invoke("EnableAnim", UnityEngine.Random.Range(0f, 5f));
    }

    void Update()
    {
        if (move)
        {
            DetectController();

            if (!delivered)
            {
                if (runFromPlayer)
                {
                    distance = Vector3.Distance(transform.position, player.transform.position);

                    Vector3 dirToPlayer = (transform.position - player.transform.position).normalized;
                    dirToPlayer = dirToPlayer * sheepDistanceRun;

                    Vector3 newPos = transform.position + dirToPlayer;
                    //newPos = newPos + new Vector3(sheepDistanceRun, 0, sheepDistanceRun);

                    agent.SetDestination(newPos);
                    if (!player.GetComponent<MenuPlayerMovement>().ChatchedSheep)
                    {
                        if (distance <= catchDistance)
                        {
                            if (!catchSheepText.gameObject.activeInHierarchy)
                                catchSheepText.ShowText(uiTarget, keyboardAndMouse);
                            canBeCatched = true;
                            //outline.enabled = true;
                        }
                        else
                        {
                            catchSheepText.HideText();
                            canBeCatched = false;
                            //outline.enabled = false;
                        }
                        currentTime += Time.deltaTime;
                        if (currentTime >= stepsInterval)
                        {
                            PlayEffect("paso" + UnityEngine.Random.Range(1, 6));
                            currentTime = 0;
                        }
                    }
                }else if (canBeCatched)
                {
                    WalkConf(false);
                }

                if (canBeCatched && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("XboxA")))
                {
                    runFromPlayer = false;
                    followPlayer = true;
                    canBeCatched = false;
                    //outline.enabled = false;
                    catched = true;

                    //outline.enabled = false;
                    WalkConf(false);

                    catchSheepText.HideText();
                    GetComponent<SphereCollider>().radius = catchedTriggerRadious;
                    player.GetComponent<MenuPlayerMovement>().ChatchedSheep = true;

                    CancelInvoke("Balido");
                    CancelInvoke("Cencerro");
                    SoundManager.instance.PlayEffect("yatetengo");
                    PlayEffect("balido" + UnityEngine.Random.Range(1, 4));
                }

                if (followPlayer)
                {
                    agent.SetDestination(player.transform.position);
                    WalkConf(Vector3.Distance(transform.position, agent.destination) > 2);
                    currentTime += Time.deltaTime;
                    if (currentTime >= stepsInterval)
                    {
                        PlayEffect("paso" + UnityEngine.Random.Range(1, 6));
                        currentTime = 0;
                    }
                }
            }
            else if (Vector3.Distance(transform.position, deliverPoint.position) < 1)
            {
                agent.ResetPath();
                move = false;
                deliverPoint.gameObject.SetActive(false);
                WalkConf(false);
            }
            else
			{
                currentTime += Time.deltaTime;
                if (currentTime >= stepsInterval)
                {
                    PlayEffect("paso" + UnityEngine.Random.Range(1, 6));
                    currentTime = 0;
                }
            }

            if(agent.destination == null)
            {
                WalkConf(false);

			}
			
        }
        else
        {
            WalkConf(false);
        }

    }

    void WalkConf(bool isWalking)
    {
        particles.gameObject.SetActive(isWalking);
        anim.SetBool("walking", isWalking);
        anim.SetBool("iddle", !isWalking);
    }

    void EnableAnim()
    {
        anim.enabled = true;
    }

    void DetectController()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float horizontalX = Input.GetAxisRaw("XboxH");
        float verticalX = Input.GetAxisRaw("XboxV");

        if (horizontal != 0 || vertical != 0)
            keyboardAndMouse = true;
        else if (horizontalX != 0 || verticalX != 0)
            keyboardAndMouse = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!catched)
            {
                WalkConf(true);
                agent.speed = runSpeed[0];
                runFromPlayer = true;
                StartCoroutine(ReduceSpeed());
                PlayEffect("balido" + UnityEngine.Random.Range(1, 4));
            

            if (!player.GetComponent<MenuPlayerMovement>().ChatchedSheep)
                {
                    SoundManager.instance.PlayEffect("adondevas");
                }
               
            
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && catched)
        {
            followPlayer = false;
            agent.ResetPath();
            WalkConf(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!catched)
            {
                runFromPlayer = false;
                canBeCatched = false;

                //outline.enabled = false;

                StopCoroutine(ReduceSpeed());
                StartCoroutine(StopRunning());

                catchSheepText.HideText();
            }
            else
            {
                agent.speed = followSpeed;
                followPlayer = true;
            }
        }
    }

   

    
    public void DeliverSheep(Transform deliverZone)
    {
        deliverPoint = deliverZone;
        delivered = true;
        agent.SetDestination(deliverPoint.position);
        GetComponent<SphereCollider>().enabled = false;
        player.GetComponent<MenuPlayerMovement>().ChatchedSheep = false;

        SoundManager.instance.PlayEffect("yiha");
    }

    IEnumerator ReduceSpeed()
    {
        //var em = particles.emission;
        //em.rateOverTime = new ParticleSystem.MinMaxCurve(agent.speed, agent.speed);
        yield return new WaitForSeconds(0.3f);
        agent.speed = runSpeed[1];
        //em.rateOverTime = new ParticleSystem.MinMaxCurve(agent.speed, agent.speed);
        yield return new WaitForSeconds(0.3f);
        agent.speed = runSpeed[2];
        //em.rateOverTime = new ParticleSystem.MinMaxCurve(agent.speed, agent.speed);
    }
    IEnumerator StopRunning()
    {
        yield return new WaitUntil(()=> { return Vector3.Distance(transform.position, agent.destination) < 0.5; });
        runFromPlayer = false;
        agent.ResetPath();
        WalkConf(false);
    }

    void Balido()
	{
            
        PlayEffect("balido" + UnityEngine.Random.Range(1, 4));
        Invoke("Balido", UnityEngine.Random.Range(5f, 10f));
        
    }
    void Cencerro()
    {
        PlayEffect("cencerro" + UnityEngine.Random.Range(1, 4));
        Invoke("Cencerro", UnityEngine.Random.Range(5f, 10f));
    }

    public void RestartSheep()
    {
        runFromPlayer = false;
        followPlayer = false;
        canBeCatched = false;
        catched = false;
        delivered = false;
        move = true;
        agent.ResetPath();
        gameObject.SetActive(false);
        particles.gameObject.SetActive(false);
        anim.enabled = false;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().radius = 5;
    }

    void PlayEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Effect: " + name + " not found!");
            return;
        }
        sonido.clip = s.clip;
        sonido.PlayOneShot(sonido.clip);
    }

    
}
