using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldController : MonoBehaviour
{
    Transform shield;

    Dampener shieldBlend;

    public bool Shielding { get => enabled && shieldBlend > 0.5f; }

    // Start is called before the first frame update
    void Start()
    {
        shieldBlend = gameObject.AddComponent<Dampener>().Init(0, 0.03f);
    }

    // Update is called once per frame
    void Update()
    {
        shieldBlend.target = Player.c.Player.Shield.ReadValue<float>() > 0.5f ? 1 : 0;
        shield.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), shieldBlend);
        transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -90), Quaternion.identity, shieldBlend);

        if (Player.c.Player.Shield.triggered) Player.player.sword.ReturnToNeutral();
    }

    private void OnEnable()
    {
        shield = transform.GetChild(0);
        shield.gameObject.SetActive(true);
    }
}
