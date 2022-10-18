using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ScreenScalar : MonoBehaviour
{
    public Vector2 baseRes;

    [Range(-1,1)]
    public int adaptPosition;

    public Camera camera;
    public Camera camera2;
    //public Vector3 cameraPos;
    public float defaultHeight;
    public float defaultWidth;

    public bool maintainWidth;
    // Start is called before the first frame update
    //void Start()
    void Update()
    {
        defaultHeight = camera.orthographicSize;
        defaultWidth = defaultHeight * camera.aspect;

        if (maintainWidth)
        {
            camera.orthographicSize = defaultWidth / camera.aspect;
            camera2.orthographicSize = camera.orthographicSize;

            camera.transform.position = new Vector3(camera.transform.position.x, adaptPosition * (defaultHeight - camera.orthographicSize), camera.transform.position.z);
        }
        else {
            camera.transform.position = new Vector3(adaptPosition * (defaultWidth - camera.orthographicSize * camera.aspect), camera.transform.position.y, camera.transform.position.z);
        }
        //transform.localScale = new Vector3(baseRes.y / Screen.height, baseRes.x / Screen.width, 1);
    }

}
