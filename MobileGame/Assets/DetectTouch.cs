using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTouch : MonoBehaviour
{
    BoxCollider2D box;
    bool canTap;
    // Start is called before the first frame update
    void Start()
    {
        canTap = true;
        box = GetComponent<BoxCollider2D>();
        Input.simulateMouseWithTouches = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && canTap)
        {
            canTap = false;
            Vector2 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position, Camera.MonoOrStereoscopicEye.Mono);
            Rect gridRect = new Rect(transform.position, box.size);
            if (gridRect.Contains(touchPosWorld))
            {
                GameManager.Instance.grid.CreateGrid();
            }
        }
        if (Input.touchCount == 0) {
            canTap = true;
        }
    }
}
