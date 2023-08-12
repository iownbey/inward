using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHealth : MonoBehaviour, IDamageable
{
    public float health;

    public List<Object> destroy;
    Rigidbody2D rb;
    public List<Transform> disconnect;
    public List<GameObject> create;

    public void Damage(float damage, Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        health -= damage;

        if (health < 0)
        {
            destroy.ForEach(a => Destroy(a));
            disconnect.ForEach(a => a.parent = null);
            create.ForEach(a => Instantiate(a, transform.position, transform.rotation));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
