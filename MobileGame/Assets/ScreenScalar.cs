using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScalar : MonoBehaviour
{
    public Vector2 baseRes;
    public Camera cam;
    // Start is called before the first frame update
    void Update()
    {
        transform.localScale = new Vector3(baseRes.y / Screen.height, baseRes.x / Screen.width, 1);
    }

}
