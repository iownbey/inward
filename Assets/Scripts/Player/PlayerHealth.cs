using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public GameObject blackScreen;
    public AudioGetter deathSound;
    void IDamageable.Damage(float damage, Vector2 force)
    {
        PlayerHealth runner = new GameObject("Dead Player").AddComponent<PlayerHealth>();
        runner.StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        blackScreen.SetActive(true);
        BackgroundMusicController.Stop();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound.Get(), transform.position);
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(1);
    }
}
