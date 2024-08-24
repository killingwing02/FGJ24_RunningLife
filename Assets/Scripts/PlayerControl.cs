using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private SpriteRenderer playerSprite;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isMidAir = false;
    private bool isSliding = false;
    private bool isInvisablity = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isMidAir)
        {
            rb.AddForce(jumpForce * Vector2.up);
            
            animator.Play("Jump");
            animator.SetBool("Jump", true);

            Debug.Log("Jumped!");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding)
        {
            animator.SetBool("Slide", true);

            isSliding = true;
            Debug.Log("Sliding");
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && isSliding)
        {
            animator.SetBool("Slide", false);
            isSliding = false;
        }

        // TODO: Not really working...
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit.collider != null)
        {
            var color = Color.blue;
            if (transform.position.y - hit.point.y > .755f && !isMidAir)
            {
                color = Color.green;
                isMidAir = true;

                if (!animator.GetBool("Jump"))
                    animator.Play("Jump", 0, 6f / 11f);
            }
            else if (transform.position.y - hit.point.y <= .755f && isMidAir)
            {
                isMidAir = false;
            }

            Debug.DrawLine(transform.position, hit.point, color);
        }

        // Damage check
        if (isInvisablity) return;

        hit = Physics2D.Raycast(transform.position, Vector2.right, .35f);
        if (hit.collider != null && hit.collider.CompareTag("Blocker"))
        {
            Debug.Log("<color=red>Hit</color>: " + hit.collider.name);
            isInvisablity = true;
            LeanTween.alpha(playerSprite.gameObject, 0, .1f).setLoopPingPong(5).setOnComplete(() => isInvisablity = false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interact = collision.GetComponent<IInteractable>();

        if (interact != null)
        {
            interact.OnInteract();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Blocker"))
        {
            animator.SetBool("Jump", false);
            isMidAir = false;
        }
    }
}
