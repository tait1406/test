//test pull

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 4f, minDist = 4f;

    private Transform player;
    private SpriteRenderer sr;
    private Rigidbody2D myBody;
    private Animator anim;
    public float maxHealth = 100;
    public float currentHealth;
    public float deathTime = 1.33f;
    private bool facingRight;

    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;
        anim = gameObject.GetComponent<Animator>();
        currentHealth = maxHealth;
        facingRight = true;
    }

    
    void Update()
    {
        float direction = player.transform.position.x - transform.position.x;
        if (Vector3.Distance(transform.position, player.position) >= minDist) {
            if (!gameObject.GetComponent<SkeletonAttack>().attacking && GameObject.FindWithTag("Player").GetComponent<Player>().currentHealth > 0) {
                anim.SetBool("Walk", true);
                if ((direction > 0 && !facingRight) || (direction < 0 && facingRight)) {
                    facingRight = !facingRight;
                    transform.Rotate(new Vector3(0, 180, 0));
                }
                if (direction > 0) {
                    myBody.velocity = new Vector2(speed, myBody.velocity.y);
                }

                if (direction < 0) {
                    myBody.velocity = new Vector2(-speed, myBody.velocity.y);
                }
            }
            else if (GameObject.FindWithTag("Player").GetComponent<Player>().currentHealth <= 0) {
                anim.SetBool("Walk", false);
            }
            
        }
        else {
            anim.SetBool("Walk", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Limit")
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<BoxCollider2D>());
    }

    public void takeDamage(float damage) {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        if (currentHealth <= 0) {
            enemyDie();
        }
    } 

    void enemyDie() {
        GameObject.FindWithTag("Manager").GetComponent<Manager>().scr++;
        GameObject.FindWithTag("Spawner").GetComponent<Spawner>().killedMons++;
        anim.SetBool("isDead", true);
        this.enabled = false;
        GetComponent<SkeletonAttack>().enabled = false;
        GameObject.FindWithTag("Spawner").GetComponent<Spawner>().currentNumberOfMons--;
        Destroy(gameObject, deathTime);
    }
}
