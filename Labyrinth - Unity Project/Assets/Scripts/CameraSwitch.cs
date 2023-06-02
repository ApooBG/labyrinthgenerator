using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] Camera FPSCamera;
    [SerializeField] GameObject labyrinth;

    [SerializeField] FirstPersonMovement characterMovement;
    [SerializeField] Crouch characterCrouch;
    [SerializeField] Jump characterJump;
    [SerializeField] FirstPersonLook characterLook;
    [SerializeField] GameObject player;

    [SerializeField] float speed;

    Animator animator;
    bool canMoveCamera;
    Quaternion playerRotation;
    float time;

    private void Start()
    {
        animator = GetComponent<Animator>();
        canMoveCamera = false;
        time = 0;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.K)) //switches camera
        {
            if (!FPSCamera.enabled && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > animator.GetCurrentAnimatorStateInfo(0).length || FPSCamera.enabled) //if the animation is not playing or if FPS Camera is enabled switch the camera 
                if (time > 1F && !canMoveCamera) //ensures that there won't be spamming the switching camera function
                {
                    ToggleFPSCamera();
                    time = 0;
                }
        }

        if (canMoveCamera) 
        {
            StartCoroutine(MoveCameraToPlayer());
        }

        else
        {
            MoveCamera();
        }

        ToggleCharacterMovement();

        if (!FPSCamera.enabled)
        {
            //for some reason the FPS Controller is starting to rotate when I switch to labyrinth's camera, this prevents that
            player.transform.rotation = playerRotation; 
        }
    }

    void ToggleFPSCamera()
    {
        FPSCamera.enabled = !FPSCamera.enabled;

        if (!FPSCamera.enabled) //if labyrinth camera is switched on, play the animation
        {
            playerRotation = player.transform.rotation;
            animator.SetTrigger("Switch"); 
        }

        else 
        {
            transform.position = FPSCamera.transform.position; 
            animator.applyRootMotion = false; //otherwise the animation playes at the wrong place
            animator.SetTrigger("Back");
        }
    }


    //keybinds for the labyrinth camera
    void MoveCamera() //keybinds of movement
    {
        if (!FPSCamera.enabled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftShift) && transform.position.y > 4) //boundary so the player cannot go under the labyrinth
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
        }
    }

    
    void ToggleCharacterMovement() //the player won't be able to move if in labyrinth camera
    {
        characterCrouch.enabled = FPSCamera.enabled;
        characterMovement.enabled = FPSCamera.enabled;
        characterJump.enabled = FPSCamera.enabled;
        characterLook.enabled = FPSCamera.enabled;
    }

    public void MoveTowardsPlayer() //a method thats linked with the camera animation
    {
        canMoveCamera = true;
    }

    IEnumerator MoveCameraToPlayer()
    {
        animator.applyRootMotion = true; //applying root motion so the camera is able to be controlled
        var step = 0.1f * Time.deltaTime; // calculate distance to move

        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z); //target position which is xz of the player and y of the camera
        while (canMoveCamera) 
        {
            if (Math.Round(transform.position.x, 2) == Math.Round(target.x, 2) && Math.Round(transform.position.z, 2) == Math.Round(target.z,2)) //math.round to the second number because otherwise it's looking to be the exact precise, which is not happening in some scenarious
            {
                canMoveCamera = false;
                yield break;
            }

            if (Input.GetKeyDown(KeyCode.Space)) //if clicked space, the animation is skipped
            {
                SkipCameraAnimation();
                break;
            }

            transform.position = Vector3.MoveTowards(transform.position, target, step); //move the camera towards the target position
            yield return null; 
        }
    }

    void SkipCameraAnimation() 
    {
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = target;
        canMoveCamera = false;
    }
}
