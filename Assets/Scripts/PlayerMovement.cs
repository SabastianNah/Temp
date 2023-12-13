using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{
    //player rb
    private Rigidbody rb;
    //where player respawns
    private Vector3 respawnPoint;

    public Transform orientation;

    public float speed = 10.0f;
    public float groundDrag;

    public float horizontalInput;
    public float verticalInput;

    public KeyCode jumpKey = KeyCode.Space;
    public bool readyToJump;
    public float jumpForce;
    public bool extraJump;

    public bool onGround;
    public bool onWall;
    public bool jumpy;

    public LayerMask whatIsGround;
    public float distToGround;

    //Karan Edits
    public Animator animator;

    private bool facingRight = true;

    //Sabastian Edits
    public GameObject winTextObject;
    public bool levelCompleted;


    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        //get and store player rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //make player's current position be where they respawn on start
        respawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        readyToJump = true;
        extraJump = false;
        jumpForce = 14f;
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    private void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, distToGround + 0.8f);
        MyInput();
        SpeedControl();

        if (onGround)
        {
            rb.drag = groundDrag;
            animator.SetBool("Grounded", true);
        }
        else
        {
            rb.drag = 0;
            animator.SetBool("Grounded", false);
        }

        if (onWall && !onGround)
        {
            animator.SetBool("WallSliding", true);
        }
        else
        {
            animator.SetBool("WallSliding", false);
        }

        if ((horizontalInput > 0 && !facingRight) || (horizontalInput < 0 && facingRight))
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKeyDown(jumpKey))
        {
            if (onGround && readyToJump)
            {
                Jump();
                readyToJump = false;
            }
            else if (onWall && !onGround)
            {
                jumpForce = 16f;
                Jump();
                jumpForce = 14f;
                onWall = false;
                extraJump = true;
            }
        }
        Invoke(nameof(ResetJump), 0);
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (onGround)
            rb.AddForce(moveDirection.normalized * speed * 15f, ForceMode.Force);

        // in air
        else if (!onGround)
            rb.AddForce(moveDirection.normalized * speed * 15f, ForceMode.Force);
    }

    private void Jump()
    {
        // reset y velocity
        if(jumpy){
            jumpForce = 25f;
        }
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        jumpForce = 14f;

        animator.SetTrigger("WallJump");
        StartCoroutine(CancelWallJump());
    }

    // cancel wall jump animation
    private IEnumerator CancelWallJump()
    {
        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("WallJump");
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jumpable"))
        {
            onWall = true;
            MyInput();
        }
        else if (other.gameObject.CompareTag("JumpPad")){
            jumpy = true;
        }
        else if (other.gameObject.CompareTag("EndFlag"))
        {
            Time.timeScale = 0f;
            winTextObject.SetActive(true);
        }
        else if (other.gameObject.CompareTag("CheckPoint")){
            respawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (other.gameObject.CompareTag("Respawn")){
            transform.position = respawnPoint;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jumpable"))
        {
            onWall = false;
        }
        else if(other.CompareTag("JumpPad")){
            jumpy = false;
        }
    }

    // flip character looking direction for animation
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}