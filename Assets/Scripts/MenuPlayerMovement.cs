﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerMovement : MonoBehaviour{

    [SerializeField] Transform cam;
    [SerializeField] float speed = 7.5f;
    CharacterController controller;
    [SerializeField] public Animator anim;

    [SerializeField] float gravityScale = 9.8f;
    [SerializeField] LayerMask whatIsGround = -1;
    [SerializeField] float feetRadius;
    [SerializeField] Transform feetPos;
    [SerializeField] ParticleSystem particles;

    bool chatchedSheep;

    bool isGrounded;
    Vector3 velocity;

    float yVelocity;
    float yVelocityVelocity;

    [SerializeField, Range(0, .3f)] float turnSmoothTime = .1f;
    float turnSmoothVelocity;


    [SerializeField, Min(0)] float gravity = 15f;

    float currentX;
    float xDirVelocity;
    float currentY;
    float yDirVelocity;
    float velocityChangeVelocity;

    [SerializeField, Range(0, 1f)] float stepsInterval = .5f;
    float currentTime;

    Vector3 originPosition;

    public bool ChatchedSheep { get => chatchedSheep; set => chatchedSheep = value; }

    void Awake(){
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originPosition = transform.position;
    }

    void Update()
    {
        if (anim != null && Time.timeScale == 1)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if(horizontal == 0 && vertical == 0)
            {
                horizontal = Input.GetAxisRaw("XboxH");
                vertical = Input.GetAxisRaw("XboxV");
            }

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= .1f)
            {
                currentTime += Time.deltaTime;
                if(currentTime >= stepsInterval){
                    SoundManager.instance.PlayEffect("paso" + Random.Range(1, 6));
                    currentTime = 0;
                }

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                Vector3 directionX = direction.x * transform.forward;
                Vector3 directionZ = direction.z * transform.forward;
                currentX = Mathf.SmoothDamp(currentX, directionX.x, ref xDirVelocity, .1f);
                currentY = Mathf.SmoothDamp(currentY, directionZ.z, ref xDirVelocity, .1f);
                anim.SetBool("running", true);
                yDirVelocity = 1;
               // particles.gameObject.SetActive(true);
            }
            else{
                currentX = Mathf.SmoothDamp(currentX, 0, ref xDirVelocity, .1f);
                currentY = Mathf.SmoothDamp(currentY, 0, ref xDirVelocity, .1f);
                yDirVelocity = 0;
                //particles.gameObject.SetActive(false);
                anim.SetBool("running", false);
            }

            //float vel = Mathf.SmoothDamp(anim.GetFloat("VelocityY"), yDirVelocity, ref velocityChangeVelocity, .1f);
            //anim.SetFloat("VelocityY", vel);

            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void RestartPlayer()
    {
        ChatchedSheep = false;
        GetComponent<CharacterController>().enabled = false;
        transform.position = originPosition;
        GetComponent<CharacterController>().enabled = true;
        anim.SetBool("running", false);
    }


}