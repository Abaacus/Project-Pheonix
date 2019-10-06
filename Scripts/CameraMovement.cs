using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum CameraType { followPlayer, cutscene };
    public CameraType cameraType;
    public Transform target;
    public float zoom = 10f;

    void LateUpdate()
    {
        switch (cameraType)
        {
            default:
                cameraType = CameraType.followPlayer;
                break;

            case CameraType.followPlayer:
                Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
                transform.position = newPos;
                transform.GetComponent<Camera>().orthographicSize = zoom;
                break;

            case CameraType.cutscene:
                //
                break;
        }
    }
}
