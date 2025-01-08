using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    public Vector3 convertToCamSpace(Vector3 VectorToRotate)
    {
        float currentYvalue = VectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;


        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardzProduct = VectorToRotate.z * cameraForward;
        Vector3 cameraRightxProduct = VectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardzProduct + cameraRightxProduct;
        vectorRotatedToCameraSpace.y = currentYvalue;

        return vectorRotatedToCameraSpace;
    }
}
