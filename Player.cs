using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 8f, jumpForce = 6f;
    public bool isGrounded = true, isDead = false;
    private bool facingRight = true;
    public float maxHealth = 100;
    public float currentHealth;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D myBody;
    private BoxCollider2D boxColli;
    public Canvas die;

    void Awake() {
        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        myBody = gameObject.GetComponent<Rigidbody2D>();
        boxColli = gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        playerMove();
        playerJump();
    }

    void playerMove() {
        if (gameObject.GetComponent<PlayerAttack>().attacking == false && GameObject.Find("GameManager").GetComponent<Event>().choosen) {
            float h = Input.GetAxis("Horizontal");

            transform.position += new Vector3(h, 0, 0) * Time.deltaTime * speed;

            if (h != 0) {
                anim.SetBool("Run", true);
            }
            else {
                anim.SetBool("Run", false);
            }
            if ((h > 0 && !facingRight) || (h < 0 && facingRight)) {
                anim.SetBool("Run", true);
                facingRight = !facingRight;
                transform.Rotate(new Vector3(0, 180, 0));
            }
        }
        else if (!GameObject.Find("GameManager").GetComponent<Event>().choosen) {
            anim.SetBool("Run", false);
        }
        
    }

    void playerJump() {
        if (Input.GetButtonDown("Jump") && isGrounded && gameObject.GetComponent<PlayerAttack>().attacking == false) {
            anim.SetBool("Jump", true);
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }

    public void takeDamage(float damage) {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0) {
            playerDie();
        }
    }

    void playerDie() {
        anim.SetTrigger("isDead");
        Cursor.visible = true;
        die.gameObject.SetActive(true);
        isDead = true;
        GameObject.FindWithTag("Spawner").GetComponent<Spawner>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<Player>().enabled = true;
    }
}
