using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //VARIABLES
    private Rigidbody rigid;
    private BoxCollider hitBox;
    public float jumpHeight = 7F;
    public float jumpSpeed = 10F;
    public float stompSpeed = -50F;

    //PARTICLE SYSTEM FOR GROUND IMPACT
    [SerializeField] ParticleSystem groundSlam = null;

    //JUMP BUFFER
    float jumpBufferTime = 0.15F;
    float jumpBufferCounter;

    //ANIMATION
    Animator animator;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        hitBox = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        groundSlam.Stop();
    }

    void Update()
    {
        //JUMP BUFFER COUNTER
        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0) {
            Jump(jumpHeight);
        }

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
        //Debug.Log(IsGrounded() + " " + rigid.velocity.y);
        //Debug.Log(rigid.position.y);
    }

    //METHOD FOR JUMP ACTION
    private void Jump(float jumpHeight) {
        if (!IsGrounded()) 
            return;
        rigid.velocity = new Vector3(rigid.position.x, jumpHeight, rigid.position.z);
        //rigid.AddForce(new Vector3(rigid.position.x, jumpHeight, rigid.position.z), ForceMode.Impulse);

            jumpBufferCounter = 0;
    }

    //METHOD FOR STOMP ACTION
    private void Stomp(float stompSpeed)
    {
        rigid.velocity = Vector3.zero;
        //rigid.MovePosition(new Vector3(rigid.position.x, 1.125002F, rigid.position.z) * Time.deltaTime * 10F);
        rigid.velocity = new Vector3(rigid.position.x, stompSpeed, rigid.position.z);

        StompFX();

        if (IsGrounded())
        {
            StompFX();
        }
        //rigid.AddForce(new Vector3(rigid.position.x, stompSpeed, rigid.position.z), ForceMode.Impulse);  
    }

    private void StompFX() {
        groundSlam.Play();
    }

    private bool IsGrounded() {
        //return Mathf.Approximately(rigid.velocity.y, 0F);
        //Debug.Log(rigid.position.y);
        return rigid.position.y <= 1.127501;
    }

}
