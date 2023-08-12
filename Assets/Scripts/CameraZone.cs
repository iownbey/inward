using UnityEngine;
using Cinemachine;

public class CameraZone : MonoBehaviour
{
    new CinemachineVirtualCamera camera;

    private void Start()
    {
        camera = GetComponentInChildren<CinemachineVirtualCamera>();
        camera.m_Priority = 9;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag)) camera.m_Priority = 11;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Player.playerTag)) camera.m_Priority = 9;
    }
}
