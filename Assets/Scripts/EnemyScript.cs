using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public static int hp = 100;
    public Material material;
    public Transform player;
    private Rigidbody rb;
    private Vector3 movement;
    private float speed = 0.5f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        material.color = Color.green;
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        movement = direction;
    }
    void FixedUpdate()
    {
        MoveEnemy(movement);
    }
    void MoveEnemy(Vector3 direction)
    {
        rb.MovePosition((Vector3)transform.position + (direction * speed * Time.deltaTime));

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            hp--;
            material.color = Color.red;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                Invoke("ResetMaterial", 0.1f);
            }
        }
    }
    
    void ResetMaterial()
    {
        material.color = Color.green;
    }
}
