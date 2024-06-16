using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DemonBoss : MonoBehaviour
{
    private float maxBossHP = 5;
    public float bossHP;
    [SerializeField] Animator animator;

    private CameraShake shake;
    private World world;
    PlayerDodgeMovement pdm;

    [SerializeField] GameObject[] bossProjectiles;  //1 - 4 are the side projectiles, 5 - 8 are upcoming debris

    //BOSS LOOK VARIABLES
    public Transform target;
    private Vector3 lookOffset = new Vector3(0F, 1F, 0F);
    public float looksSpeed = 7.0f;

    //BOSS ATTACK VARIABLES
    private bool attackBool = false;
    private float minAttackTime = 2F;   
    private float maxAttackTime = 4F;

    private float debrisOffset = 15F;
    private bool hasDebrisAttack = false;

    private float debrisTimerCounter;
    private float debrisTimer = 4F;

    private AudioManager am;

    void Awake()
    {
        ResetBossHP();
        animator = GetComponent<Animator>();
        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pdm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDodgeMovement>();
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        //Debug.Log(debrisTimerCounter);

        world.activeBossHP = bossHP;

        //KEEPS BOSSFIGHT ACTIVE
        if (world != null && bossHP > 0)
        {
            world.bossFight = true;
        }
        //IF BOSS IS NOT ALIVE
        else {
            world.bossFight = false;
            world.levelCounter = 0;
            world.bossSpawn = false;

            if (pdm != null) {
                pdm.addScore(3);
            }
            world.currentLevel++;
            world.levelGateSpawned = false;

            world.numOfLevelsForBoss += 3;

            Destroy(this.gameObject);
        }

        //MAKES BOSS ALWAYS FACE PLAYER AFTER ARRIVING
        //REF: https://discussions.unity.com/t/how-can-i-make-a-game-object-look-at-another-object/98932
        //if (target != null)
        //{
        //    Vector3 dir = (target.position/4 - transform.position);
        //    Quaternion lookRot = Quaternion.LookRotation(dir);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * looksSpeed);
        //}

        //BOSS ATTACK
        if (!attackBool)
        {
            float attackTime = UnityEngine.Random.Range(minAttackTime, maxAttackTime);
            Invoke("ExecuteAttack", attackTime);
            attackBool = true;
        }

        //DEBRIS TIMER
        if (hasDebrisAttack) {
            debrisTimerCounter -= Time.deltaTime;
            if (debrisTimerCounter <= 0) {
                hasDebrisAttack = false;
            }
        }
    }

    //POSSIBLE BOSS ATTACKS:
    //X-AXIS JUMP AND SLIDE THROWS
    //THROWING UPCOMING OBSTACLES
    //TEMPORARY LANE DENIAL

    private void ExecuteAttack() {

        int attackTypeIndex;
        //DOESNT TRIGGER DEBRIS ATTACK UNLESS BUFFER IS OVER
        if (hasDebrisAttack)
        {
            
            attackTypeIndex = Random.Range(0, 1);
        }
        else
        {
            attackTypeIndex = Random.Range(0, 2);
        }

        switch (attackTypeIndex)
        {
            //X-AXIS ATTACK IS TWICE AS LIKELY THAN DEBRIS
            case 0:
                xAxisAttack();
                break;
            case 1:
                debrisScatter();
                Invoke("cameraShake", 1F);
                break;
        }
        attackBool = false;
    }

    //ATTACK LAUNCHED FROM BOSS ALONG X AXIS AT PLAYER (EITHER JUMP/SLIDE) - SOME ARE PARRIABLE
    private void xAxisAttack() {

        am.playBossProjectileSFX();

        int obstacleIndex = Random.Range(0, 3);
        float projHeight = 1F;

        switch (obstacleIndex) {
            //HIGH PROJ
            case 0:
                projHeight = 2F;
                break;
            //HIGH PROJ - PARRY
            case 2:
                projHeight = 2F;
                break;
        }

        GameObject activeProjectile = Instantiate(bossProjectiles[obstacleIndex], new Vector3(-5F, projHeight, 0F), Quaternion.Euler(0, 0, 0));
        //Debug.Log("Spawned Projectile + " + activeProjectile.transform.position);
    }

    private void debrisScatter() {

        am.playBossSlamSFX();

        //DEBRIS 1
        int obstacleIndex = Random.Range(4, bossProjectiles.Length);
            Instantiate(bossProjectiles[obstacleIndex], new Vector3(0F, 20F, debrisOffset), Quaternion.Euler(0, 0, 0));
            //DEBRIS 2
            obstacleIndex = Random.Range(4, bossProjectiles.Length);
            Instantiate(bossProjectiles[obstacleIndex], new Vector3(0F, 20F, debrisOffset + 7F), Quaternion.Euler(0, 0, 0));
            //DEBRIS 3
            obstacleIndex = Random.Range(4, bossProjectiles.Length);
            Instantiate(bossProjectiles[obstacleIndex], new Vector3(0F, 20F, debrisOffset + 14F), Quaternion.Euler(0, 0, 0));

        hasDebrisAttack = true;
        debrisTimerCounter = debrisTimer;
    }

    public void takeDamage() {
        bossHP -= 1;
    }

    public void ResetBossHP() {
        bossHP = maxBossHP;
    }

    private void cameraShake() {
        shake.Shake();
    }
}
