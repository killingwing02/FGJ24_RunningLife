using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LeanTweenType deadType;

    [Header("Prefabs")]
    [SerializeField] private GameObject deadFx;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalPoint;
    private bool isMidAir;
    private bool isSliding;
    private bool isInvisablity;
    private bool isDead;

    private void Awake()
    {
        originalPoint = transform.position;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Initialize();
        GameManager.Instance.onGameStart.AddListener(OnGameStart);
    }

    public void Initialize()
    {
        isMidAir = false;
        isSliding = false;
        isInvisablity = false;
        isDead = false;

        LeanTween.cancel(gameObject, false);
        LeanTween.cancel(playerSprite.gameObject, false);

        transform.position = originalPoint;
        transform.GetChild(0).rotation = Quaternion.identity;

        playerSprite.color = Color.white;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isMidAir)
        {
            rb.AddForce(jumpForce * Vector2.up);

            animator.Play("Jump");
            animator.SetBool("Jump", true);
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

        #region Damage Check
        // Damage check
        if (isInvisablity || isDead) return;

        hit = Physics2D.Raycast(transform.position, Vector2.right, .35f);
        if (hit.collider != null && hit.collider.CompareTag("Blocker"))
        {
            Debug.Log("<color=red>Hit</color>: " + hit.collider.name);
            isInvisablity = true;

            // TODO: Move damage to blocker's script.
            GameManager.Instance.RemoveCurrentHp(5);
            LeanTween.alpha(playerSprite.gameObject, 0, .1f).setLoopPingPong(5).setOnComplete(() => isInvisablity = false);
            // Skip damage check if already get hurt.
            return;
        }

        // Damage check from head
        if (isSliding) return;

        var startPoint = transform.position;
        startPoint.y += .85f;
        hit = Physics2D.Raycast(startPoint, Vector2.right, .35f);
        if (hit.collider != null && hit.collider.CompareTag("Blocker"))
        {
            Debug.Log("<color=red>Hit</color>: " + hit.collider.name);
            isInvisablity = true;

            // TODO: Move damage to blocker's script.
            GameManager.Instance.RemoveCurrentHp(5);
            LeanTween.alpha(playerSprite.gameObject, 0, .1f).setLoopPingPong(5).setOnComplete(() => isInvisablity = false);
        }
        #endregion
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

    public void PlayerDiedAnimation()
    {
        isDead = true;
        playerSprite.color = Color.red;
        animator.speed = 0f;
        LeanTween.cancel(playerSprite.gameObject, false);
        LeanTween.rotateZ(transform.GetChild(0).gameObject, 90f, .4f).setEase(deadType);
        LeanTween.delayedCall(1f, () =>
        {
            playerSprite.color = Color.clear;
            Instantiate(deadFx, playerSprite.transform.position, Quaternion.identity);
        });
    }

    private void OnGameStart()
    {
        animator.speed = 1f;
    }
}
