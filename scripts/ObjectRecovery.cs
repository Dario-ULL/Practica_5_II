using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectRecovery : MonoBehaviour
{
    public void OnPointerEnter()
    {
        MyObjectController[] allItems = Resources.FindObjectsOfTypeAll<MyObjectController>();

        foreach (var item in allItems)
        {
            if (item.gameObject.scene.isLoaded)
            {
                item.RestoreObject();
            }
        }
    }
}