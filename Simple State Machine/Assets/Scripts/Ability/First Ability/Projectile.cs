using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private LayerMask targetLayer;
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;

    private Rigidbody2D rb;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    public void Initialize(Vector2 dir, float spd, float dmg, float life, LayerMask layer)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        lifetime = life;
        targetLayer = layer;

        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & targetLayer) != 0)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit();
            }

            Destroy(gameObject);
        }
    }
}
