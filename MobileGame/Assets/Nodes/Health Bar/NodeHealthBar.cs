using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHealthBar : MonoBehaviour
{
    [SerializeField] Node node;
    SpriteRenderer nodeRenderer;

    SpriteRenderer healthRenderer;
    Material healthMat;
    // Start is called before the first frame update
    void Start()
    {
        nodeRenderer = node.GetComponent<SpriteRenderer>();
        healthRenderer = GetComponent<SpriteRenderer>();
        healthMat = healthRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        healthRenderer.color = nodeRenderer.color;
        healthMat.SetFloat("_Fullness",Mathf.Clamp01(((float)node.currentHP-1) / ((float)node.maxHP-1)));
    }
}
