using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSlideMovement : MonoBehaviour
{
    //!JUMP NEEDS TO BE SMOOTHENED OUT WITH A CURVE!

    //HEIGHT PROPERTIES
    public float jumpHeight = 4F;
    public float normalHeight = 0F;
    public float slideHeight = -0.01F;

    //SPEED PROPERTIES
    public float jumpSpeed = 50F;
    public float stompSpeed = 300F;
    [Range(0, 30)]
    public float fallSpeed = 10F;

    //MOVEMENT BOOLEANS
    bool movingJump = false;
    bool movingNormal = false;
    bool movingStomp = false;
    bool movingSlide = false;

    void Update()
    {
        //HEIGHT VECTORS
        Vector3 jump = new Vector3(transform.position.x, jumpHeight, transform.position.z);
        Vector3 normal = new Vector3(transform.position.x, normalHeight, transform.position.z);
        Vector3 slide = new Vector3(transform.position.x, slideHeight, transform.position.z);

        //STOMP ATTACK INPUT - TAKES PRIORITY (LIKELY TO BE SPAMMED)
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && transform.position.y > normal.y)
        {
            movingStomp = true;
        }
        //JUMP INPUT
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && movingJump == false && transform.position.y <= normalHeight)
        {
            movingJump = true;
        }
        //SLIDE INPUT
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && transform.position.y == normal.y && movingStomp == false && movingJump == false)
        {
            movingSlide = true;
        }
        
        //STOMP MOVEMENT ACTION
        if (movingStomp)
        {
            transform.position = Vector3.MoveTowards(transform.position, normal, stompSpeed * Time.deltaTime);
            if (transform.position.y == normal.y)
            {
                movingSlide = true;
                movingJump = false;
                movingStomp = false;
                movingNormal = false;
            }
        }
        //JUMP MOVEMENT ACTION
        else if (movingJump)
        {

            //Original:
            transform.position = Vector3.MoveTowards(transform.position, jump, jumpSpeed * Time.deltaTime);
            //rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

            if (transform.position.y == jump.y)
            {
                movingJump = false;
                movingNormal = true;
            }
        }
        //FALL MOVEMENT ACTION
        else if (movingNormal)
        {
            //Original:
            transform.position = Vector3.MoveTowards(transform.position, normal, fallSpeed * Time.deltaTime);

            //Eased:
            //transform.position = Vector3.SmoothDamp(transform.position, normal, ref velocity, jumpSpeed * Time.deltaTime);

            if (transform.position.y == normal.y)
            {
                movingNormal = false;
            }
        }
        //SLIDE MOVEMENT ACTION
        else if (movingSlide)
        {
            transform.position = Vector3.MoveTowards(transform.position, slide, jumpSpeed * Time.deltaTime);
            if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) && transform.position.y < normal.y)
            {
                movingSlide = false;
                movingNormal = true;
            }
        }
    }
}
