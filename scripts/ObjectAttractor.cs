using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectAttractor : MonoBehaviour
{
    [Tooltip("Si se deja vacío, busca automáticamente el objeto llamado 'Player'.")]
    public Transform playerTransform;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
    }

    public void OnPointerEnter()
    {
        if (playerTransform == null) return;

        MyObjectController[] allItems = Resources.FindObjectsOfTypeAll<MyObjectController>();

        foreach (var item in allItems)
        {
            if (item.gameObject.scene.isLoaded)
            {
                item.AttractTo(playerTransform);
            }
        }
    }
}