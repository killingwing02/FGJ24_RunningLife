using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;
    private bool isJumped = false;
    private bool isInvisablity = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumped)
        {
            rb.AddForce(jumpForce * Vector2.up);
            isJumped = true;
            Debug.Log("Jumped!");
        }

        // Damage check
        if (isInvisablity) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, .35f);
        if (hit.collider != null && hit.collider.CompareTag("Blocker"))
        {
            Debug.Log("<color=red>Hit</color>: " + hit.collider.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Blocker"))
        {
            isJumped = false;
        }
    }
}
