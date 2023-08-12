using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float maxRot;
    ScrollingNoise centerRotationNoise;
    ScrollingNoise laserOriginNoise;
    [HideInInspector]
    public Dampener centerSpin;
    public Transform center;
    public LineRenderer laser;
    public LayerMask hittable;
    public ParticleSystem startParticles;
    public ParticleSystem endParticles;

    public Timings timings;
    

    RaycastHit2D[] hits = new RaycastHit2D[1];

    public bool Firing 
    { 
        get => firing; 
        set
        {
            laser.enabled = value;
            firing = value;
        } 
    }
    bool firing = false;

    void Start()
    {
        centerRotationNoise = gameObject.AddComponent<ScrollingNoise>();
        laserOriginNoise = gameObject.AddComponent<ScrollingNoise>();
        laserOriginNoise.scrollSpeed = 10;
        centerSpin = gameObject.AddComponent<Dampener>().Init(0, 0.5f);

        laser.useWorldSpace = true;

        Firing = !timings.useTimings;
        if (timings.useTimings) StartCoroutine(UseTimings());
    }

    void Update()
    {
        center.Rotate(0, 0, (centerRotationNoise.GetSignedValue() + centerSpin) * maxRot * Time.deltaTime);

        if (!Firing) return;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(hittable);
        Vector3 laserOrigin = center.position + center.right * laserOriginNoise.GetSignedValue() * 0.025f;
        Physics2D.CircleCast(laserOrigin, 0.25f, transform.up, filter, hits);
        if (hits[0])
        {
            //print(hits[0].collider.gameObject.name);
            Vector3 contact = hits[0].point + (hits[0].normal * 0.25f);
            laser.SetPositions(new Vector3[] { laserOrigin, contact });
            endParticles.transform.position = contact;
            endParticles.transform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,hits[0].normal) + 180);
        }
    }

    public IEnumerator StartFiring()
    {
        startParticles.Play();
        centerSpin.target = 2;
        yield return new WaitForSeconds(Timings.preTime);
        Firing = true;
        endParticles.Play();
    }

    public IEnumerator StopFiring()
    {
        startParticles.Stop();
        yield return new WaitForSeconds(Timings.preTime);
        Firing = false;
        centerSpin.target = 0;
        endParticles.Stop();
    }


    public IEnumerator UseTimings()
    {
        while (timings.useTimings)
        {
            yield return new WaitForSeconds(timings.offTime);
            yield return StartFiring();
            yield return new WaitForSeconds(timings.onTime);
            yield return StopFiring();
        }
    }

    [System.Serializable]
    public class Timings
    {
        public bool useTimings;
        public float offTime;
        public const float preTime = 0.4f;
        public float onTime;
    }
}
