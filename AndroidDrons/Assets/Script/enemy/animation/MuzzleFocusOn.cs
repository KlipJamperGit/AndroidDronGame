using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFocusOn : MonoBehaviour
{
    // ќб'Їкт, за €ким спостер≥гаЇмо
    public Transform muzzle;

    void Update()
    {
        // ѕерев≥р€Їмо, чи встановлено ц≥ль
        if (muzzle != null)
        {
            Vector3 targetPosition = new Vector3(muzzle.position.x, muzzle.position.y, muzzle.position.z);

            // «мусити об'Їкт дивитис€ на ц≥ль
            transform.LookAt(targetPosition) ;
        }
    }
}
