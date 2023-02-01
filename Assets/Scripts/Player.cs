using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animController;
    float horizontal_value;
    Vector2 ref_velocity = Vector2.zero;

    float jumpForce = 12f;

    [SerializeField] TrailRenderer tr;
    [SerializeField] float moveSpeed_horizontal = 400.0f;
    [SerializeField] bool is_jumping = false;
    [SerializeField] bool grounded = false;
    [SerializeField] bool is_crouching = false;
    [Range(0, 1)][SerializeField] float smooth_time = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        //Debug.Log(Mathf.Lerp(current, target, 0));
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_value = Input.GetAxis("Horizontal");

        if(horizontal_value > 0) sr.flipX = false;
        else if (horizontal_value < 0) sr.flipX = true;
        
        animController.SetFloat("Speed", Mathf.Abs(horizontal_value));
   
        if (Input.GetButtonDown("Jump") && grounded)
        {
            is_jumping = true;
            animController.SetBool("Jumping", true);
        }

        if (Input.GetButton("Vertical") && grounded)
        {
            is_crouching = true;
            // anim
        }
        else
        {
            is_crouching = false;
        }

    }
    void FixedUpdate()
    {
        if (is_jumping && grounded)
        {           
            is_jumping = false;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            grounded = false;
        }
        if (is_crouching && grounded)
        {
            //rigidbody
            Debug.Log("je crouch !");
        }
        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        grounded = true;
        animController.SetBool("Jumping", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        animController.SetBool("Jumping", false);        
    }

}