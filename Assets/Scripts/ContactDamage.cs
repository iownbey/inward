using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage;
    public Vector2 recoil;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.GetComponent<IDamageable>()?.Damage(damage,recoil);
    }
}
