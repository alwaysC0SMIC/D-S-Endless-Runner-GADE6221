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

    private bool isSlide = false;
    private static float slideTimePeriod = 1F;
    private float slideTimeCounter;

    public bool stomp = false;
    public bool animStomp = false;
    private CameraShake shake;

    //PLAYER LOCK
    private bool playerUnlock = false;

    //PARTICLE SYSTEM FOR GROUND IMPACT
    //[SerializeField] ParticleSystem groundSlam = null;

    //JUMP BUFFER
    float jumpBufferTime = 0.15F;
    float jumpBufferCounter;

    //ANIMATION
    private World world;
    Animator animator;
    private PlayerDodgeMovement pdm;
    private bool attackAnim = false;

    void Start()
    {
        //groundSlam.Stop();
        rigid = GetComponent<Rigidbody>();
        hitBox = GetComponent<BoxCollider>();
        animator = GetComponentInChildren<Animator>();
        pdm = GetComponent<PlayerDodgeMovement>();
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();

        //UNLOCKS PLAYER MOVEMENT AFTER EXITING HOUSE
        Invoke("unlockPlayerMovement", 2);
    }

    void Update()
    {
        //Debug.Log(isSlide + "   " + slideTimeCounter);

        //CHARACTER ANIMATIONS
        if (world.playerIsDead)
        {
            animator.SetTrigger("death");
        }
        animator.SetBool("attack", attackAnim);
        animator.SetBool("stomp", animStomp);
        animator.SetFloat("vertical", transform.position.y);
        animator.SetFloat("horizontal", pdm.direction);


        //Debug.Log(pdm.direction);

        //animator.SetBool("stomp", stomp);
        //Debug.Log(IsGrounded());

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
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftControl)) && IsGrounded() && !isSlide)
        {
                Slide();
        }

            //CANCELS SLIDING IF PLAYER JUMPS DURING TIME
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) && isSlide)
            {
                isSlide = false;
            }    
        }

        //SLIDE HITBOX CHANGES
        if (isSlide)
        {
            hitBox.size = new Vector3(0.025F, 0.015F, 0.025F);
        }
        else {
            hitBox.size = new Vector3(0.025F, 0.025F, 0.025F);
        }

        //SLIDE TIMER
        if (isSlide) {
            if (slideTimeCounter > 0)
            {
                slideTimeCounter -= Time.deltaTime;
            }
            else {
                slideTimeCounter = 0;
                isSlide = false;
            }
        }
    }

    //UNLOCKS PLAYER MOVEMENT
    public void unlockPlayerMovement()
    {
        playerUnlock = true;
    }

    public void lockPlayerMovement()
    {
        playerUnlock = false;
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
        animStomp = true;
        Invoke("resetStompAnim", 0.12F);
        //SHAKE FX
        //shake.Shake(); 
    }

    private void Slide() {
        isSlide = true;
        slideTimeCounter = slideTimePeriod;
    }

    private void slideReset() {
        isSlide = false;  
    }

    //private void StompFX() {
    //    groundSlam.Play();
    //}

    public void attackEnemy() {
        attackAnim = true;
        Invoke("enemyAttackAnimEnd", 0.01F);
    }

    private void enemyAttackAnimEnd() {
        attackAnim = false;
    }

    private void resetStompAnim() {
        animStomp = false;
    }

    private bool IsGrounded() {
        //return Mathf.Approximately(rigid.velocity.y, 0F);
        //Debug.Log(rigid.position.y);
        return rigid.position.y <= 1.127501;
    }
}
