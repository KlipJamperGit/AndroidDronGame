
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCollision;
    [SerializeField] private Rigidbody Drone;
    [SerializeField] private GameObject[] Controls;
    [SerializeField] public FixedJoystick joystick;
    [SerializeField] public DynamicJoystick joystickLook;
    [SerializeField] public VariableJoystick joystickHeight;

    void OnCollisionEnter(Collision collision)
    {
        if (playerCollision == collision.gameObject)
        {
            return;
        }
        Debug.Log(collision.gameObject.name);
        player.SetActive(true);
        Drone.freezeRotation = false;
        joystick.StopAllCoroutines();
        joystickLook.StopAllCoroutines();
        joystickHeight.StopAllCoroutines(); 
        foreach (var c in Controls)
        {
            c.gameObject.SetActive(false);
        }
    }
}
