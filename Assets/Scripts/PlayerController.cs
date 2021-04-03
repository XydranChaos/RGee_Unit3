using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbPlayer;
    public float gravityModifier;
    public float jumpForce;
    private bool onGround = true;
    public bool gameOver = false;

    private Animator animPlayer;
    public ParticleSystem expSystem;
    public ParticleSystem dirtSystem;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    private AudioSource asPlayer;
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        animPlayer = GetComponent<Animator>();

        asPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //conditions met to Jump
        bool spaceDown = Input.GetKeyDown(KeyCode.Space);
        if(spaceDown && onGround && !gameOver)
        {
            //jump
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
            animPlayer.SetTrigger("Jump_trig");
            //stop particle system from running on jump
            dirtSystem.Stop();
            //play jump sound
            asPlayer.PlayOneShot(jumpSound, 3.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            //play dirt particle when player on ground
            dirtSystem.Play();
        }
        //game is over with this condition met
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            animPlayer.SetBool("Death_b", true);
            animPlayer.SetInteger("DeathType_int", 2);
            expSystem.Play();
            dirtSystem.Stop();
            //play crash sound
            asPlayer.PlayOneShot(crashSound, 3.0f);
        }
    }
}
