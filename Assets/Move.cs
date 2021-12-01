using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public float walkSpeed;
    private float moveInput;
    public bool isGrounded;
    private Rigidbody2D rb;
    public LayerMask groundMask;

    public PhysicsMaterial2D bounceMat, normalMat;
    public bool canJump = true;
    public float jumpValue = 0.0f;
    public float fallValue = 0.0f;
    public float hp = 100.0f;
    public int Respawn;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource groundSound;


    [SerializeField] private Text heathBar;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        heathBar.text = hp.ToString();
    }
    

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if(jumpValue ==0.0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
        }


        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(0.9f, 0.4f), 0f, groundMask);

        if(jumpValue == 0)
        {
            rb.sharedMaterial = bounceMat;
        }else
        {
            rb.sharedMaterial = normalMat;
        }

        if (isGrounded == false)
        {
            fallValue += 80.0f*Time.deltaTime;
        }

        if (isGrounded)
        {
            if (fallValue >= 70 && isGrounded)
            {
                hp -= fallValue/7;
                fallValue = 0.0f;
                heathBar.text = hp.ToString();
            }
            
            rb.sharedMaterial = normalMat;
            fallValue = 0.0f;
        }

        if (Input.GetKey("space") && isGrounded && canJump)
        {
            jumpValue += 80.0f*Time.deltaTime;
        }

        if(Input.GetKeyDown("space") && isGrounded && canJump)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }

        if(jumpValue >= 30f && isGrounded && canJump)
        {
            float tempx = moveInput * walkSpeed;
            float tempy = 30;
            jumpSound.Play();
            rb.velocity = new Vector2(tempx, tempy);
            Invoke("ResetJump", 0f);
        }

        if(Input.GetKeyUp("space"))
        {
            if(isGrounded && canJump)
            {
                jumpSound.Play();
                rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
                jumpValue = 0.0f;
            }
            canJump = true;
        }




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        groundSound.Play();
        
        if (collision.gameObject.CompareTag("Crown"))
        {
            Restart();
        }
    

    }

    void Restart()
    {
        SceneManager.LoadScene(Respawn);
    }



    void ResetJump()
    {
        canJump = false;
        jumpValue = 0;
    }
}
