using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeadTiltWalk : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de avance en metros por segundo.")]
    public float moveSpeed = 10.0f;

    [Tooltip("Fuerza de gravedad aplicada en el eje Y.")]
    public float gravity = 9.81f;

    [Header("Umbrales de Inclinación")]
    [Tooltip("Ángulo de inclinación hacia abajo necesario para empezar a caminar (en grados).")]
    [Range(10f, 60f)]
    public float walkThresholdAngle = 20.0f;

    [Tooltip("Ángulo límite inferior; si miras completamente al suelo, el jugador se detiene.")]
    [Range(60f, 85f)]
    public float maxStopAngle = 80.0f;

    [Header("Referencias")]
    [Tooltip("Cámara principal que rota con el visor Cardboard.")]
    public Transform vrCamera;

    private CharacterController characterController;
    private float verticalVelocity = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (vrCamera == null && Camera.main != null)
        {
            vrCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        if (vrCamera == null) return;

        float pitch = vrCamera.eulerAngles.x;
        if (pitch > 180f)
        {
            pitch -= 360f;
        }

        Vector3 moveDirection = Vector3.zero;

        if (pitch >= walkThresholdAngle && pitch <= maxStopAngle)
        {
            Vector3 forward = vrCamera.forward;
            forward.y = 1.0f;
            forward.Normalize();

            moveDirection = forward * moveSpeed;
        }

        if (characterController.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}