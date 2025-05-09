using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        // Canvas'� her zaman ana kameraya do�ru d�nd�r
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}