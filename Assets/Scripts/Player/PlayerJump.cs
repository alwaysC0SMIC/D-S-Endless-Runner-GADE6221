using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //VARIABLES
    private Rigidbody rigid;
    private BoxCollider hitBox;
    private static float jumpHeight = 8F;
    //private static float jumpSpeed = 10F;
    private static float stompSpeed = -65F;

    public bool stomp = false;
    private CameraShake shake;

    //PLAYER LOCK
    private bool playerUnlock = false;

    //PARTICLE SYSTEM FOR GROUND IMPACT
    //[SerializeField] ParticleSystem groundSlam = null;

    //JUMP BUFFER
    float jumpBufferTime = 0.15F;
    float jumpBufferCounter;

    //ANIMATION
    Animator animator;

    void Start()
    {
        //groundSlam.Stop();
        rigid = GetComponent<Rigidbody>();
        hitBox = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        shake = GameObject.Find("ISO CAMERA").GetComponent<CameraShake>();

        //UNLOCKS PLAYER MOVEMENT AFTER EXITING HOUSE
        Invoke("unlockPlayerMovement", 2);
    }

    void Update()
    {
        //JUMP BUFFER COUNTER
        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0) {
            Jump(jumpHeight);
        }

        if(playerUnlock)
        { 
        ////JUMP INPUT
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        //STOMP
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftControl)) && !IsGrounded()) {
            Stomp(stompSpeed);
        }

        //SLIDE INPUT
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftControl)) && IsGrounded())
        {
            //ENTER SLIDE HEIGHT
            hitBox.size = new Vector3(0.025F, 0.015F, 0.025F);
        } else if ((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftControl)) && IsGrounded()) {
            //COME UP FROM SLIDE TO NORMAL
            hitBox.size = new Vector3(0.025F, 0.025F, 0.025F);
        }
        }
    }

    //UNLOCKS PLAYER MOVEMENT
    private void unlockPlayerMovement()
    {
        playerUnlock = true;
    }

    //METHOD FOR JUMP ACTION
    private void Jump(float jumpHeight) {
        if (!IsGrounded())
        {
            return;
        }   
        rigid.velocity = new Vector3(rigid.position.x, jumpHeight, rigid.position.z);
        jumpBufferCounter = 0;
    }

    //METHOD FOR STOMP ACTION
    private void Stomp(float stompSpeed)
    {
        rigid.velocity = Vector3.zero;
        rigid.velocity = new Vector3(rigid.position.x, stompSpeed, rigid.position.z);

        stomp = true;

        //SHAKE FX
        //shake.Shake(); 
    }

    //private void StompFX() {
    //    groundSlam.Play();
    //}

    private bool IsGrounded() {
        //return Mathf.Approximately(rigid.velocity.y, 0F);
        //Debug.Log(rigid.position.y);
        return rigid.position.y <= 1.127501;
    }

}
