using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RandomRotator : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        if (Application.isPlaying) return;

        Randomize();
    }

    [ContextMenu("Randomize")]
    public void Randomize()
    {
        List<int> chunks = new List<int>();
        for (int i = 0; i < 360; i++) chunks.Add(i);
        Collider2D hitbox = GetComponent<Collider2D>();
        bool success = false;
        Collider2D[] colliders = new Collider2D[1];
        do
        {
            int ri = Random.Range(0, chunks.Count);
            int chunk = chunks[ri];
            chunks.RemoveAt(ri);

            transform.rotation = Quaternion.Euler(0, 0, chunk + Random.value);
            if (hitbox.OverlapCollider(new ContactFilter2D() { useTriggers = false }, colliders) == 0)
            {
                success = true;
                break;
            }
        }
        while (chunks.Count > 0);

        if (!success) Debug.LogWarning("Couldn't find rotation that fit", gameObject);
    }

    [ContextMenu("Rerandomize All")]
    public void RandomizeAll()
    {
        RandomRotator[] all = FindObjectsOfType<RandomRotator>();
        foreach (RandomRotator rot in all) rot.Randomize();
    }
#endif
}
