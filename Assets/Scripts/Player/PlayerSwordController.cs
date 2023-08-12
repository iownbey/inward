using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    public Player player;
    public Rigidbody2D rb;

    public float attackDamage;
    public float attackForce;

    public SpriteRenderer sword;
    public Transform swordAnchor;
    public float leftSwingAngle;
    public float rightSwingAngle;
    Dampener swordRot;
    Timer showSword;
    const float showTime = 1;
    public Slice sliceR;
    public Slice sliceL;
    public AudioSource sliceAudioSource;
    public AudioGetter sliceAudio = new AudioGetter();
    public AudioGetter hitAudio = new AudioGetter();

    bool Slicing { get => sliceR.slicing || sliceL.slicing; }
    Slice lastSlice;

    const float sliceForce = 3f;

    private void OnEnable()
    {
        sword.enabled = true;
    }

    private void Start()
    {
        swordRot = gameObject.AddComponent<Dampener>().Init(0, 0.01f);
        showSword = gameObject.AddComponent<Timer>();
        showSword.Set(3);
        showSword.OnFinish += ReturnToNeutral;
    }

    // Update is called once per frame
    void Update()
    {
        swordAnchor.localRotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(leftSwingAngle, rightSwingAngle, swordRot));
        transform.localScale = Vector3.one * (1 + Mathf.Pow((float)(swordRot - 0.5)*2,2) * 0.25f);
        sword.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, showSword.time / showTime);
        if (!Slicing && Player.c.Player.Slice.triggered && !Player.player.shield.Shielding)
        {
            showSword.Set(showTime);
            rb.velocity = Vector2.zero;
            rb.AddRelativeForce(Vector2.up * sliceForce, ForceMode2D.Impulse);

            lastSlice = (lastSlice == sliceL) ? sliceR : sliceL;
            swordRot.target = (lastSlice == sliceL) ? 1 : 0;
            player.twist.value = (lastSlice == sliceL) ? -1 : 1;
            StartCoroutine(lastSlice.Animate(0.075f));

            List<Collider2D> potentialHits = new List<Collider2D>();
            List<Hit> hits = new List<Hit>();
            lastSlice.hitbox.enabled = true;
            lastSlice.hitbox.OverlapCollider(new ContactFilter2D(), potentialHits);
            lastSlice.hitbox.enabled = false;
            potentialHits.ForEach(a =>
            {
                IDamageable[] ds = a.GetComponents<IDamageable>();
                System.Array.ForEach(ds, d =>
                {
                    Hit h = new Hit(a.transform, d);
                    if (!hits.Contains(h)) hits.Add(h);
                });
            });

            hits.ForEach(d =>
            {
                d.damageable.Damage(attackDamage, ((Vector2)d.transform.position - rb.position).normalized * attackForce);
            });

            sliceAudioSource.PlayOneShot(((hits.Count == 0) ? sliceAudio : hitAudio).Get());
        }
    }

    public void ReturnToNeutral()
    {
        if (!enabled) return;
        lastSlice = sliceR;
        swordRot.target = 0;
    }

    [System.Serializable]
    public class Slice
    {
        public bool slicing;
        public LineRenderer line;
        public Collider2D hitbox;

        public IEnumerator Animate(float duration)
        {
            slicing = true;
            line.enabled = true;
            
            float time = 0;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;

                float blend = 1 - (time / duration);
                Gradient gradient = new Gradient();
                float b = blend * blend;
                float a = blend * (2f - blend);
                gradient.SetKeys(
                    new GradientColorKey[]
                    {
                        new GradientColorKey(Color.black, 0),
                        new GradientColorKey(Color.black, 1)
                    },
                    new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(0, 1),
                        new GradientAlphaKey(0, Mathf.Clamp01(a + 0.1f)),
                        new GradientAlphaKey(1, a),
                        new GradientAlphaKey(1, b),
                        new GradientAlphaKey(0, Mathf.Clamp01(b - 0.1f)),
                        new GradientAlphaKey(0, 0),
                    }
                );
                line.colorGradient = gradient;
                return time >= duration;
            });
            line.enabled = false;
            slicing = false;
        }
    }

    struct Hit
    {
        public Transform transform;
        public IDamageable damageable;

        public Hit(Transform transform, IDamageable damageable)
        {
            this.transform = transform;
            this.damageable = damageable;
        }
    }
}
