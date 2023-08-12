using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public Player player;
    public float stride;
    public AudioGetter sounds;
    Vector2 lastPosition;
    public AudioSource source;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector2 delta = (Vector2)transform.position - lastPosition;
        if (delta.sqrMagnitude > stride * stride)
        {
            if (player.twist.target == 0) player.twist.target = 0.1f;
            player.twist.target *= -1;
            lastPosition = Vector2.MoveTowards(lastPosition, transform.position, stride);
            source.PlayOneShot(sounds.Get());
        }
    }
}
