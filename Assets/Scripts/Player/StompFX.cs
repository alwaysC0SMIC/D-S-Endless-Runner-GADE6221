using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompFX : MonoBehaviour
{
    //VARIABLES
    private ParticleSystem particles;
    private GameObject player;
    private Rigidbody playerRB;
    private World world;
    private AudioManager am;

    private bool playSlideParticles = false;

    //FX BUFFER
    //private float timer = 1F;
    //private float timerCounter;

    void Awake()
    {
        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
        particles = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody>();
        particles.Stop();
        am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {

        //PLAYS STOMP WHEN ACTIVATED
        if (player.GetComponent<PlayerJump>().stomp && player.transform.position.y <= 1.2F)
        {
            //timerCounter = timer;
            particles.Stop();
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            particles.Play();
            am.playStompSFX();
            player.GetComponent<PlayerJump>().stomp = false;
        }
        else {
            if (player.GetComponent<PlayerJump>().isSlide && player.transform.position.y <= 1.2F && !playSlideParticles)
            {
                particles.Stop();
                transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                particles.Play();
                playSlideParticles = true;
                Invoke("SlideParticleReset", 1F);
            }

            //AVOIDS PARTICLE SYSTEM GOING TO FAR INTO DESTROYER COLLIDER
            if (transform.position.z > -10) {
                transform.Translate(Vector3.back * world.gameSpeed * Time.deltaTime, Space.World);
            }
        }

        //else {
        //    transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
        //}    

    }

    private void SlideParticleReset() {
        playSlideParticles = false;
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        particles.Play();
    //    }
    //}
}
