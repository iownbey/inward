using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    public const string playerTag = "Player";

    public static Player player;
    public static int securityLevel = 0;
    public static int attempts = 0;

    public Color dashColor;
    public Color rechargeColor;

    public static Controls c;
    public Rigidbody2D rb;
    const float movementCoefficient = 30;

    const float dashCoefficient = 20;
    const float dashCooldownDuration = 1;
    const float dashTimeDuration = 0.5f;

    Timer dashCooldown;
    Timer dashTime;
    ColorController colorer;

    public bool canDash = false;
    public bool Dashing { get => !dashTime.IsFinished; }
    List<IInteractable> interactables = new List<IInteractable>();
    bool frozen = false;

    public PlayerSwordController sword;
    public PlayerShieldController shield;
    public GameObject flashlight;

    public PlayerUI ui;

    [HideInInspector]
    public AngleDampener twist;

    // Start is called before the first frame update

    private void Awake()
    {
        player = this;
        twist = gameObject.AddComponent<AngleDampener>().Init(0,0.05f);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        c = new Controls();
        c.Enable();

        dashCooldown = gameObject.AddComponent<Timer>();
        dashTime = gameObject.AddComponent<Timer>();
        dashCooldown.OnFinish += () => { colorer.Flash(rechargeColor, 8); };
        colorer = gameObject.AddComponent<ColorController>();

        ui.SetAttempt(++attempts);
        ui.SetId(securityLevel);
        ui.ShowText("I need to get out of here.");
    }

    // Update is called once per frame
    void Update()
    {
        if (c.Player.Interact.triggered && !frozen && interactables.Count > 0)
        {
            interactables[0].Interact(this);
        }

        if (canDash && c.Player.Dash.triggered && dashCooldown.IsFinished)
        {
            rb.AddForce(c.Player.Movement.ReadValue<Vector2>() * dashCoefficient, ForceMode2D.Impulse);
            dashCooldown.Set(dashCooldownDuration);
            dashTime.Set(dashTimeDuration);
            colorer.Flash(dashColor, 4);
        }
    }

    private void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(c.Player.MousePos.ReadValue<Vector2>());
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, mousePos - (Vector2)transform.position) + (twist * 15)));
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    IInteractable i = collision.collider.GetComponent<IInteractable>();
    //    if (i != null) interactables.Add(i);

    //    if (!Dashing)
    //    {
    //        //if the colliding body has a damager, take the hit.
    //        collision.collider.IfHasComponent<Collider2D, Damager>((d) => { TakeHit(d, collision.GetContact(0).point); });
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out var i)) interactables.Add(i);
    }

    //void TakeHit(Damager d, Vector2 hitPos)
    //{
    //    colorer.Flash(Color.red, 8);
    //    healthTimer.time += d.damage * totalHealth;
    //    rb.AddForceAtPosition((rb.position - hitPos).normalized * d.rebound, hitPos);
    //}

    private void OnCollisionExit2D(Collision2D collision) => OnTriggerExit2D(collision.collider);
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactables.Remove(collision.gameObject.GetComponent<IInteractable>());
    }

    public void Freeze()
    {
        frozen = true;
        c.Player.Movement.Disable();
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public void Unfreeze()
    {
        frozen = false;
        c.Player.Movement.Enable();
        rb.isKinematic = false;
    }

    public IEnumerator WaitForInput()
    {
        print("Waiting...");

        //yield return new WaitUntil(() => { return c.Player.Interact.triggered; });

        bool done = false;
        void callback(InputAction.CallbackContext ctx) { done = true; }
        c.Player.Slice.performed += callback;
        yield return new WaitUntil(() => { return done == true; });
        c.Player.Slice.performed -= callback;
        print("done");
    }
}
