using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MyObjectController : MonoBehaviour
{
    [Header("Materiales")]
    public Material gazedMaterial;
    private Material defaultMaterial;
    private Renderer rend;
    private bool wasGazed = false;

    [Header("Movimiento (Atracción y Retorno)")]
    [Tooltip("Velocidad de atracción y retorno.")]
    public float moveSpeed = 6.0f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform targetPlayer = null;
    private bool isRestoring = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            defaultMaterial = rend.material;
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (targetPlayer != null)
        {
            Vector3 targetOffset = (transform.position - targetPlayer.position).normalized * 1.2f;
            Vector3 destination = targetPlayer.position + targetOffset;

            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }
        else if (isRestoring)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, moveSpeed * 60f * Time.deltaTime);

            if (Vector3.Distance(transform.position, originalPosition) < 0.02f)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                isRestoring = false;
            }
        }
    }

    public void OnPointerEnter()
    {
        wasGazed = true;
        SetMaterial(true);
    }

    public void OnPointerExit()
    {
        if (wasGazed)
        {
            SetMaterial(false);
            wasGazed = false;
            gameObject.SetActive(false);
        }
    }

    private void SetMaterial(bool gazedAt)
    {
        if (rend != null && gazedMaterial != null)
        {
            rend.material = gazedAt ? gazedMaterial : defaultMaterial;
        }
    }

    public void AttractTo(Transform player)
    {
        gameObject.SetActive(true);
        isRestoring = false;
        targetPlayer = player;
    }

    public void RestoreObject()
    {
        gameObject.SetActive(true);
        SetMaterial(false);
        wasGazed = false;

        targetPlayer = null;
        isRestoring = true;
    }
}