using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 4.0f;
    Rigidbody2D rb;

    private float health = 200;
    private float startHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        if (horizontal > 0)
        {
            GetComponent<Animator>().Play("Right");
        }
        else if (horizontal < 0)
        {

            GetComponent<Animator>().Play("Left");
        }
        else if (vertical > 0)
        {
            GetComponent<Animator>().Play("Up");
        }
        else if (vertical < 0)
        {
            GetComponent<Animator>().Play("Down");
        }
        else if (horizontal == 0 && vertical == 0)
        {
            GetComponent<Animator>().Play("Idle");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= collision.gameObject.GetComponent<EnemyScript>().GetHitStrength();
            if (health < 1)
            {
                // DIE
            }
        }
    }
}