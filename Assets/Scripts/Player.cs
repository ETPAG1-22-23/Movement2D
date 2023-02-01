using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animController;
    float horizontal_value;
    float vertical_value;
    Vector2 ref_velocity = Vector2.zero;

    float jumpForce = 12f;

    [SerializeField] float moveSpeed_horizontal = 400.0f;
    [SerializeField] bool is_jumping = false;
    [SerializeField] bool can_jump = false;
    [SerializeField] bool is_crouching = false;
    [SerializeField] bool can_crouch = false;
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
   
        if (Input.GetButtonDown("Jump") && can_jump)
        {
            is_jumping = true;
            animController.SetBool("Jumping", true);
        }

        if (Input.GetButtonDown("Vertical") && can_crouch) // Definie l'etat du player comme accroupi et change l'animation
        {
            is_crouching = true;
            animController.SetBool("Crouching", true);
        }
        if (Input.GetButtonUp("Vertical") && can_crouch) // Definie l'etat du player comme pas accroupi et change l'animation
        {
            is_crouching = false;
            animController.SetBool("Crouching", false);
        }
    }
    void FixedUpdate()
    {
        if (is_jumping && can_jump)
        {           
            is_jumping = false;
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            can_jump = false;
            can_crouch = false; // ne peut plus crouch (car en train de sauter)
        }

        if(is_crouching && can_crouch)
        {
            Debug.Log("accroupi baby"); // test accroupi ou non
        }

        Vector2 target_velocity = new Vector2(horizontal_value * moveSpeed_horizontal * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, target_velocity, ref ref_velocity, 0.05f);
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        can_jump = true;
        animController.SetBool("Jumping", false);
        can_crouch = true; // peut s'accroupir car au sol
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        animController.SetBool("Jumping", false);        
    }
}