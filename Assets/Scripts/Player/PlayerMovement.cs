using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    public AudioGetter sounds;
    public AudioSource source;

    Timer stepTimer;
    public MovementProfile walkingProfile;
    public MovementProfile runningProfile;

    MovementProfile CurrentProfile => running ? runningProfile : walkingProfile;

    bool running = false;

    private void Start()
    {
        stepTimer = gameObject.AddComponent<Timer>();
    }

    private void Update()
    {
        Vector2 movement = Player.c.Player.Movement.ReadValue<Vector2>();

        bool isMoving = movement != Vector2.zero;

        running = Player.c.Player.Dash.IsPressed();

        if (stepTimer.IsFinished)
        {
            if (isMoving)
            {
                source.PlayOneShot(sounds.Get());
                if (player.twist.target == 0) player.twist.target = 0.1f;
                player.twist.target = -player.twist.target;

                stepTimer.time = CurrentProfile.stepInterval;
            }
            else
            {
                player.twist.target = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        MovementProfile profile = CurrentProfile;
        Vector2 movement = Player.c.Player.Movement.ReadValue<Vector2>();
        float stepDampening = (stepTimer.time / profile.stepInterval * (1 - profile.minStrideCoefficient)) + profile.minStrideCoefficient;
        player.rb.AddForce(profile.force * stepDampening * movement);
    }

    [System.Serializable]
    public class MovementProfile
    {
        public float force;
        public float minStrideCoefficient;
        public float stepInterval;
    }
}
