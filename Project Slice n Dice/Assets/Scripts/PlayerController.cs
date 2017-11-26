using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float dashTimer; //this value affects how long the speed boost while "dashing" lasts
    public float fallMultiplier = 2.5f; //this value affects how fast you fall for jump juice
    private Rigidbody2D myRigidbody;


    public bool grounded;
    public LayerMask whatIsGround;
    public bool dashing;
    public bool landing;

    private Collider2D myCollider;


    private Animator myAnimator;

    //Required to make player fall faster than he jump (jump juice, makes jump feel good)
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start ()
    {
		myRigidbody = GetComponent < Rigidbody2D >();

        myCollider = GetComponent<Collider2D>();

        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        HandleLayers();

        //jumping + grounded condition
        grounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);

        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) )
        {
            if (grounded)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                myAnimator.SetTrigger("Jump");
            }
        }

        if (myRigidbody.velocity.y <0)
        {
            landing = true;
        } else
        {
            landing = false;
        }
        if (landing == true)
        {
            myAnimator.SetBool("Land", true);
        }

       

        //jump juice
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //press shift to dash, can only dash if you aren't already dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!dashing)
            {
                moveSpeed = moveSpeed * 3;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                dashTimer = 0.4f;
            }
        }

        //dashing is true when your speed is greater than 10, otherwise it's false
        if (myRigidbody.velocity.x > 10)
        {
            dashing = true;
        } else
            {
            dashing = false;
            }

        // when dashtimer becomes greater than 0, start subtracting over time)
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;

        }

        //dashtimer becomes 0 when lower than 0, so it can't be lower than 0; if dashtimer is 0, you are back to normal speed
        if (dashTimer < 0)
        {
            dashTimer = 0;
            if (dashTimer == 0)
            {
                moveSpeed = 5;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            }
        }


            myAnimator.SetFloat ("Speed", myRigidbody.velocity.x);
        myAnimator.SetBool("Grounded", grounded);
	}

    private void HandleLayers()
    {
        if (grounded == false)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }

        if (grounded == true)
        {
            myAnimator.ResetTrigger("Jump");
            myAnimator.SetBool("Land", false);
        }

        if (dashing == true)
        {
            myAnimator.SetBool("Dashing", true);
        }
        else
        {
            myAnimator.SetBool("Dashing", false);
        }
       
    }


}
